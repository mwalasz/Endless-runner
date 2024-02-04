using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    /// <summary>
    /// Struct value to represent the location of a resource, such as an item on a remote server or the path to a local file.
    /// </summary>
    public struct URLString
    {
        #region Fields

        private     string      m_value;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this <see cref="URLString"/> is valid.
        /// </summary>
        /// <value><c>true</c> if is valid; otherwise, <c>false</c>.</value>
        public bool IsValid { get; private set; }

        public bool IsFilePath { get; private set; }

        #endregion

        #region Static methods

        /// <summary>
        /// Returns a newly created object as a http URL with specified path.
        /// </summary>
        /// <param name="path">The path should be a valid web URL path.</param>
        public static URLString URLWithPath(string path)
        {
            // validate arguments
            Assert.IsNotNullOrEmpty(path, "path");
            Assert.IsFalse(path.StartsWith("file"), "Input value is not http path.");

            // format value if required
            if (false == path.StartsWith("http"))
            {
                path = string.Concat("http://", path);
            }

            return new URLString() { m_value = path, IsValid = true };
        }

        /// <summary>
        /// Returns a newly created object as a file URL with a specified path.
        /// </summary>
        /// <param name="path">The path should be a valid system path.</param>
        public static URLString FileURLWithPath(string path)
        {
            // validate arguments
            Assert.IsNotNullOrEmpty(path, "path");
            Assert.IsFalse(path.StartsWith("http"), "Input value is not local file path.");

            // format value if required
            if (false == path.StartsWith("file")
#if UNITY_ANDROID
                && false == path.StartsWith("jar:file")
#endif
               )
            {
                path = string.Concat("file://", path);
            }

            return new URLString() { m_value = path, IsValid = true, IsFilePath = true, };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the URL representation in string format. This value is null, if given URL is invalid.
        /// </summary>
        /// <returns>The URL string.</returns>
        public override string ToString()
        {
            if (IsValid)
            {
                return m_value;
            }

            return null;
        }

        #endregion
    }
}