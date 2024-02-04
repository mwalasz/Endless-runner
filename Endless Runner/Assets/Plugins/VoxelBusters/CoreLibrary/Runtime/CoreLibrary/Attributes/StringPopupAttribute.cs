using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// Base class to create custom string popup field in the inspector.
    /// </summary>
    [IncludeInDocs]
    public class StringPopupAttribute : PropertyAttribute 
    {
        #region Static fields

        private     static  readonly    string[]    s_emptyOptions      = new string[0];

        #endregion

        #region Fields

        private readonly    string[]    m_fixedOptions;
        
        private readonly    bool        m_usesFixedOptions;

        #endregion

        #region Properties

        public string PreferencePropertyName { get; private set; }

        public bool PreferencePropertyValue { get; private set; }

        public string[] Options => m_usesFixedOptions ? m_fixedOptions : GetDynamicOptions();

        #endregion

        #region Constructors

        public StringPopupAttribute(string preferencePropertyName = null,
                                    bool preferencePropertyValue = true,
                                    params string[] fixedOptions)
        {
            // set properties
            PreferencePropertyName  = preferencePropertyName;
            PreferencePropertyValue = preferencePropertyValue;
            m_fixedOptions          = fixedOptions;
            m_usesFixedOptions      = !fixedOptions.IsNullOrEmpty();
        }

        #endregion

        #region Private methods

        protected virtual string[] GetDynamicOptions() => s_emptyOptions;

        #endregion
    }
}