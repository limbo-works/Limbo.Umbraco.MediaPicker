using System.Collections.Generic;
using Limbo.Umbraco.MediaPicker.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Editors;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Extensions;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.Factories {

    public class ImageReferenceFactory : IDataValueReferenceFactory, IDataValueReference {

        /// <inheritdoc />
        public IDataValueReference GetDataValueReference() => this;

        /// <inheritdoc />
        public bool IsForEditor(IDataEditor? dataEditor) {
            return dataEditor != null && dataEditor.Alias.InvariantEquals(LimboMediaPickerPropertyEditor.EditorAlias);
        }

        IEnumerable<UmbracoEntityReference> IDataValueReference.GetReferences(object? value) {

            List<UmbracoEntityReference> references = new();

            if (value is not string udis) return references;

            foreach (string udi in udis.Split(',')) {
                if (UdiParser.TryParse(udi, out GuidUdi? guidUdi)) references.Add(new UmbracoEntityReference(guidUdi!));
            }

            return references;

        }

    }

}