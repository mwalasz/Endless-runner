using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// Object represents an immutable, read-only object that combines a string value with a platform.
    /// </summary>
    [Serializable]
    public class RuntimePlatformConstant
    {
        #region Fields

        [SerializeField]
        private     RuntimePlatform         m_platform;

        [SerializeField]
        private     string                  m_value         = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the runtime platform associated with string value.
        /// </summary>
        /// <value>The enum value indicates the platform to which string value belongs.</value>
        public RuntimePlatform Platform => m_platform;

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public string Value => m_value;

        #endregion

        #region Constructors

        public RuntimePlatformConstant(RuntimePlatform platform, string value)
        {
            // set properties
            m_platform  = platform;
            m_value     = value;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns a new instance of <see cref="RuntimePlatformConstant"/>, containing a string value functional only on iOS platform.
        /// </summary>
        /// <returns>The instance of <see cref="RuntimePlatformConstant"/>.</returns>
        /// <param name="value">The string value associated with iOS platform.</param>
        public static RuntimePlatformConstant iOS(string value)
        {
            return new RuntimePlatformConstant(RuntimePlatform.IPhonePlayer, value);
        }

        /// <summary>
        /// Returns a new instance of <see cref="RuntimePlatformConstant"/>, containing a string value functional only on tvOS platform.
        /// </summary>
        /// <returns>The instance of <see cref="RuntimePlatformConstant"/>.</returns>
        /// <param name="value">The string value associated with tvOS platform.</param>
        public static RuntimePlatformConstant tvOS(string value)
        {
            return new RuntimePlatformConstant(RuntimePlatform.tvOS, value);
        }

        /// <summary>
        /// Returns a new instance of <see cref="RuntimePlatformConstant"/>, containing a string value functional only on Android platform.
        /// </summary>
        /// <returns>The instance of <see cref="RuntimePlatformConstant"/>.</returns>
        /// <param name="value">The string value associated with Android platform.</param>
        public static RuntimePlatformConstant Android(string value)
        {
            return new RuntimePlatformConstant(RuntimePlatform.Android, value);
        }

        /// <summary>
        /// Returns a new instance of <see cref="RuntimePlatformConstant"/>, containing a string value associated with active platform.
        /// </summary>
        /// <returns>The instance of <see cref="RuntimePlatformConstant"/>.</returns>
        /// <param name="value">The string value associated with active platform.</param>
        public static RuntimePlatformConstant Current(string value)
        {
            var     currentPlatform     = ApplicationServices.GetActiveOrSimulationPlatform();
            return new RuntimePlatformConstant(currentPlatform, value);
        }

        #endregion

        #region Public methods

        public bool IsEqualToPlatform(RuntimePlatform other)
        {
            if (other == m_platform) return true;

            // Special case for Editor
            return other.IsEditor() && m_platform.IsEditor();
        }

        #endregion

        #region Base class methods

        public override string ToString()
        {
            return m_value;
        }

        #endregion
    }
}