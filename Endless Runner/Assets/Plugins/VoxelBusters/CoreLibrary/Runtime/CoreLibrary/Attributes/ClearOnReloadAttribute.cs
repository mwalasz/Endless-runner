using System;
// Credits: https://github.com/joshcamas/unity-domain-reload-helper

namespace VoxelBusters.CoreLibrary
{
    public enum ClearOnReloadOption
    {
        None,

        Default,

        Custom,
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Event)]
    public class ClearOnReloadAttribute : Attribute
    {
        #region Properties

        public ClearOnReloadOption Option { get; private set; }

        public object CustomValue { get; private set; }

        #endregion

        #region Properties
    
        /// <summary>
        /// Marks field of property to be cleared and assigned given value on reload.
        /// </summary>
        public ClearOnReloadAttribute()
            : this(ClearOnReloadOption.None, null)
        { }
    
        /// <summary>
        /// Marks field of property to be cleared and assigned given value on reload.
        /// </summary>
        /// <param name="customValue">Explicit value which will be assigned to field/property on reload.</param>
        public ClearOnReloadAttribute(object customValue)
            : this(ClearOnReloadOption.Custom, customValue)
        { }
    
        /// <summary>
        /// Marks field of property to be cleared and assigned given value on reload.
        /// </summary>
        /// <param name="option">Option to be used.</param>
        /// <param name="customValue">Explicit value which will be assigned to field/property on reload.</param>
        public ClearOnReloadAttribute(ClearOnReloadOption option, object customValue = null)
        {
            Option          = option;
            CustomValue     = customValue;
        }

        #endregion
    }
}