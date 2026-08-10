// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// Lit based settings UI replacing the old AngularJS TypeConverter.html view and overlay. Renders a dropdown
// with the type converters returned by the package's management API.

import { css, html, nothing } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { UMB_AUTH_CONTEXT } from '@umbraco-cms/backoffice/auth';

// Ensures the <uui-select> and <uui-loader> elements used below are registered.
import '@umbraco-cms/backoffice/external/uui';

export class LimboMediaPickerTypeConverterElement extends UmbLitElement {

    static properties = {
        value: { attribute: false },
        _converters: { state: true },
        _loaded: { state: true },
        _error: { state: true }
    };

    #loading = false;

    constructor() {
        super();
        this._converters = [];
        this._loaded = false;
    }

    connectedCallback() {
        super.connectedCallback();
        // [CHANGE: QA review fix] Related: Json/MediaPickerTypeConverterJsonConverter.cs, Limbo.Umbraco.MediaPicker.csproj
        // connectedCallback runs again every time the element is re-attached (eg. when switching between the tabs of
        // the data type workspace), so only fetch once - and don't leave a stale error message behind on a retry.
        if (this.#loading || (this._loaded && !this._error)) return;
        this.#load();
    }

    // The value was saved either as { type: "<alias>" } or, in very old versions, as the raw (assembly
    // qualified) type name - so we normalize it here
    get #selectedType() {
        if (typeof this.value === 'string' && this.value.length > 0) return this.value.split(', Version')[0];
        return this.value?.type ?? '';
    }

    get #selectedConverter() {
        const type = this.#selectedType;
        return type ? this._converters.find((x) => x.type === type) : undefined;
    }

    async #load() {

        this.#loading = true;
        this._error = undefined;

        try {

            const authContext = await this.getContext(UMB_AUTH_CONTEXT);
            const config = authContext?.getOpenApiConfiguration?.();

            const base = config?.base ?? '';
            const token = config?.token ? await config.token() : await authContext?.getLatestToken();

            // [CHANGE: QA review fix] Related: limbo-media-picker.element.js, umbraco-package.json, Controllers/Api/MediaPickerController.cs
            // Pass the credentials mode from the OpenAPI configuration ('include') so the request also works with
            // the cookie based backoffice authentication introduced in Umbraco 17 (matches the documented pattern
            // in UmbAuthContext.getOpenApiConfiguration).
            const response = await fetch(`${base}/umbraco/management/api/v1/limbo/media-picker/converters`, {
                credentials: config?.credentials ?? 'include',
                headers: token ? { 'Authorization': `Bearer ${token}` } : {}
            });

            if (!response.ok) throw new Error(`${response.status} ${response.statusText}`);

            this._converters = await response.json();

        } catch (error) {
            this._error = `Failed fetching the available type converters: ${error.message}`;
        }

        this.#loading = false;
        this._loaded = true;

    }

    #onChange(event) {
        const type = event.target.value;
        this.value = type ? { type } : null;
        this.dispatchEvent(new UmbChangeEvent());
    }

    render() {

        if (!this._loaded) return html`<uui-loader></uui-loader>`;
        if (this._error) return html`<div class="warning">${this._error}</div>`;

        const selectedType = this.#selectedType;
        const selected = this.#selectedConverter;

        const options = [
            { name: 'No type converter', value: '', selected: !selectedType },
            ...this._converters.map((converter) => ({
                name: `${converter.name} (${converter.description})`,
                value: converter.type,
                selected: converter.type === selectedType
            }))
        ];

        return html`
            <uui-select label="Type converter" .options=${options} @change=${this.#onChange}></uui-select>
            ${selectedType && !selected ? html`
                <div class="warning">The selected item converter was not found.</div>
            ` : nothing}
            ${selected?.obsolete ? html`
                <div class="warning">${selected.obsolete.message ? `The selected item converter has been marked as obsolete: ${selected.obsolete.message}` : 'The selected item converter has been marked as obsolete.'}</div>
            ` : nothing}
        `;

    }

    static styles = css`
        :host {
            display: block;
        }
        uui-select {
            width: 100%;
        }
        .warning {
            margin-top: var(--uui-size-space-2, 6px);
            padding: var(--uui-size-space-3, 9px) var(--uui-size-space-4, 12px);
            background-color: var(--uui-color-warning, #fbd142);
            color: var(--uui-color-warning-contrast, #000);
            border-radius: var(--uui-border-radius, 3px);
        }
    `;

}

export default LimboMediaPickerTypeConverterElement;

customElements.define('limbo-media-picker-type-converter', LimboMediaPickerTypeConverterElement);
