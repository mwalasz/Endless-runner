using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    /// <summary>
    /// Object represents an immutable, read-only object that combines a string value with a platform.
    /// </summary>
    [Serializable, Obsolete("This class is deprecated. Instead use RuntimePlatformConstant.", true)]
    public class PlatformConstant
    {
        #region Fields

        [SerializeField]
        private     NativePlatform          m_platform      = NativePlatform.Unknown;
        
        [SerializeField]
        private     string                  m_value         = string.Empty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the runtime platform associated with string value.
        /// </summary>
        /// <value>The enum value indicates the platform to which string value belongs.</value>
        public NativePlatform Platform
        {
            get => m_platform;
            private set => m_platform = value;
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <value>The string value.</value>
        public string Value
        {
            get => m_value;
            private set => m_value = value;
        }

        #endregion

        #region Constructors

        public PlatformConstant(NativePlatform platform, string value)
        {
            // set properties
            m_platform  = platform;
            m_value     = value;
        }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns a new instance of <see cref="PlatformConstant"/>, containing a string value functional only on iOS platform.
        /// </summary>
        /// <returns>The instance of <see cref="PlatformConstant"/>.</returns>
        /// <param name="value">The string value associated with iOS platform.</param>
        public static PlatformConstant iOS(string value)
        {
            return new PlatformConstant(NativePlatform.iOS, value);
        }

        /// <summary>
        /// Returns a new instance of <see cref="PlatformConstant"/>, containing a string value functional only on tvOS platform.
        /// </summary>
        /// <returns>The instance of <see cref="PlatformConstant"/>.</returns>
        /// <param name="value">The string value associated with tvOS platform.</param>
        public static PlatformConstant tvOS(string value)
        {
            return new PlatformConstant(NativePlatform.tvOS, value);
        }

        /// <summary>
        /// Returns a new instance of <see cref="PlatformConstant"/>, containing a string value functional only on Android platform.
        /// </summary>
        /// <returns>The instance of <see cref="PlatformConstant"/>.</returns>
        /// <param name="value">The string value associated with Android platform.</param>
        public static PlatformConstant Android(string value)
        {
            return new PlatformConstant(NativePlatform.Android, value);
        }

        /// <summary>
        /// Returns a new instance of <see cref="PlatformConstant"/>, containing a string value functional on all supported platform.
        /// </summary>
        /// <returns>The instance of <see cref="PlatformConstant"/>.</returns>
        /// <param name="value">The string value associated with all supported platforms.</param>
        public static PlatformConstant All(string value)
        {
            return new PlatformConstant(NativePlatform.All, value);
        }

        /// <summary>
        /// Returns a new instance of <see cref="PlatformConstant"/>, containing a string value associated with active platform.
        /// </summary>
        /// <returns>The instance of <see cref="PlatformConstant"/>.</returns>
        /// <param name="value">The string value associated with active platform.</param>
        public static PlatformConstant Current(string value)
        {
            var     currentPlatform     = PlatformMappingServices.GetActivePlatform();
            return new PlatformConstant(currentPlatform, value);
        }

        #endregion

        #region Public methods

        public bool IsEqualToPlatform(NativePlatform other)
        {
            return ((other & m_platform) != 0);
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