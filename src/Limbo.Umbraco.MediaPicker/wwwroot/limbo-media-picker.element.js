// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// Lit based property editor UI replacing the old AngularJS "mediapicker3" view reuse. Mirrors Umbraco's own
// media picker property editor UI (umb-property-editor-ui-media-picker), but is registered for the
// "Limbo.Umbraco.MediaPicker" property editor schema.

import { html } from '@umbraco-cms/backoffice/external/lit';
import { UmbLitElement } from '@umbraco-cms/backoffice/lit-element';
import { UmbChangeEvent } from '@umbraco-cms/backoffice/event';
import { UMB_PROPERTY_CONTEXT } from '@umbraco-cms/backoffice/property';
import { UMB_VALIDATION_EMPTY_LOCALIZATION_KEY, UmbFormControlMixin } from '@umbraco-cms/backoffice/validation';

// Importing the media package registers the <umb-input-rich-media> element used below.
import '@umbraco-cms/backoffice/media';

const MEDIA_ENTITY_TYPE = 'media';

export class LimboMediaPickerElement extends UmbFormControlMixin(UmbLitElement, undefined) {

    // [CHANGE: QA review fix] Related: limbo-media-picker-type-converter.element.js, umbraco-package.json, Controllers/Api/MediaPickerController.cs
    // "value" must NOT be redeclared here: UmbFormControlMixin already declares it as a reactive property with a
    // custom accessor, and redeclaring it makes Lit generate a new accessor that shadows the mixin's one.
    static properties = {
        mandatory: { type: Boolean },
        mandatoryMessage: { type: String },
        readonly: { type: Boolean, reflect: true },
        _startNode: { state: true },
        _focalPointEnabled: { state: true },
        _preselectedCrops: { state: true },
        _allowedMediaTypes: { state: true },
        _multiple: { state: true },
        _min: { state: true },
        _max: { state: true },
        _alias: { state: true },
        _variantId: { state: true }
    };

    constructor() {

        super();

        this.mandatoryMessage = UMB_VALIDATION_EMPTY_LOCALIZATION_KEY;
        this.readonly = false;

        this._focalPointEnabled = false;
        this._preselectedCrops = [];
        this._multiple = false;
        this._min = 0;
        this._max = Infinity;

        this.consumeContext(UMB_PROPERTY_CONTEXT, (context) => {
            this.observe(context?.alias, (alias) => (this._alias = alias));
            this.observe(context?.variantId, (variantId) => (this._variantId = variantId?.toString() || 'invariant'));
        });

    }

    // Parses the data type configuration the same way as Umbraco's own media picker UI
    set config(config) {

        if (!config) return;

        this._allowedMediaTypes = config.getValueByAlias('filter')?.split(',') ?? undefined;
        this._focalPointEnabled = Boolean(config.getValueByAlias('enableLocalFocalPoint'));
        this._multiple = Boolean(config.getValueByAlias('multiple'));
        this._preselectedCrops = config.getValueByAlias('crops') ?? [];

        const startNodeId = config.getValueByAlias('startNodeId') ?? '';
        this._startNode = startNodeId ? { unique: startNodeId, entityType: MEDIA_ENTITY_TYPE } : undefined;

        const minMax = config.getValueByAlias('validationLimit');
        this._min = minMax?.min ?? 0;
        this._max = minMax?.max ?? Infinity;

    }

    firstUpdated() {
        this.addFormControlElement(this.shadowRoot.querySelector('umb-input-rich-media'));
    }

    focus() {
        return this.shadowRoot?.querySelector('umb-input-rich-media')?.focus();
    }

    #onChange(event) {
        const isEmpty = event.target.value?.length === 0;
        this.value = isEmpty ? undefined : event.target.value;
        this.dispatchEvent(new UmbChangeEvent());
    }

    render() {
        return html`
            <umb-input-rich-media
                .alias=${this._alias}
                .allowedContentTypeIds=${this._allowedMediaTypes}
                .focalPointEnabled=${this._focalPointEnabled}
                .value=${this.value ?? []}
                .max=${this._max}
                .min=${this._min}
                .preselectedCrops=${this._preselectedCrops}
                .startNode=${this._startNode}
                .variantId=${this._variantId}
                .required=${this.mandatory}
                .requiredMessage=${this.mandatoryMessage}
                ?multiple=${this._multiple}
                @change=${this.#onChange}
                ?readonly=${this.readonly}>
            </umb-input-rich-media>
        `;
    }

}

export default LimboMediaPickerElement;

customElements.define('limbo-media-picker', LimboMediaPickerElement);
