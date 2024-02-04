using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Class internally used to pass data options into sharing functions.
    /// </summary>
    public class ShareItem 
    {
        #region Constants

        private     static  readonly    ShareItem   kInvalidItem    = new ShareItem() { m_itemType = ShareItemType.None };

        #endregion

        #region Fields

        private     ShareItemType       m_itemType;

        private     string              m_text;

        private     URLString?          m_url;

        private     byte[]              m_rawData;

        private     string              m_imagePath;

        private     string              m_mimeType;
        
        private     string              m_fileName;

        #endregion

        #region Properties

        public ShareItemType ItemType => m_itemType;

        #endregion

        #region Create methods

        /// <summary>
        /// Option used to share a text content.
        /// </summary>
        /// <param name="text">Text.</param>
        public static ShareItem Text(string text)
        {
            // check arguments
            if (text == null)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Text is null.");
                return kInvalidItem;
            }

            // create new instance
            return new ShareItem()
            {
                m_itemType      = ShareItemType.Text,
                m_text          = text,
            };
        }

        /// <summary>
        /// Option used to share a url.
        /// </summary>
        /// <param name="url">URL.</param>
        public static ShareItem URL(URLString url)
        {
            // check arguments
            if (!url.IsValid)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Url is invalid.");
                return kInvalidItem;
            }

            // create new instance
            return new ShareItem()
            {
                m_itemType      = ShareItemType.URL,
                m_url           = url,
            };
        }

        /// <summary>
        /// Option used to share an image content.
        /// </summary>
        /// <param name="image">Image.</param>
        public static ShareItem Image(Texture2D image, TextureEncodingFormat textureEncodingFormat, string fileName)
        {
            // check arguments
            if (image == null)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Image is null.");
                return kInvalidItem;
            }
            if (fileName == null)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "File name is null.");
                return kInvalidItem;
            }

            // create instance
            var     data    = image.Encode(textureEncodingFormat, out string mimeType);
            return new ShareItem()
            {
                m_itemType      = ShareItemType.ImageData,
                m_rawData       = data,
                m_mimeType      = mimeType,
                m_fileName      = fileName,
            };
        }

        /// <summary>
        /// Option used to share an file content (image).
        /// </summary>
        /// <param name="data">Image data.</param>
        public static ShareItem File(byte[] data, string mimeType, string fileName)
        {
            // check arguments
            if (data == null)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Data is null.");
                return kInvalidItem;
            }
            if (mimeType == null)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "Mime type is null.");
                return kInvalidItem;
            }
            if (fileName == null)
            {
                DebugLogger.LogWarning(EssentialKitDomain.Default, "File name is null.");
                return kInvalidItem;
            }

            // create new instance
            return new ShareItem()
            {
                m_itemType      = ShareItemType.FileData,
                m_rawData       = data,
                m_mimeType      = mimeType,
                m_fileName      = fileName,
            };
        }

        /// <summary>
        /// Option used to a share screenshot.
        /// </summary>
        public static ShareItem Screenshot()
        {
            return new ShareItem()
            {
                m_itemType      = ShareItemType.Screenshot,
            };
        }

        #endregion

        #region Internal methods

        public string GetText()
        {
            // validate request
            if (m_itemType != ShareItemType.Text)
            {
                DebugLogger.LogError(EssentialKitDomain.Default, "Invalid request.");
                return null;
            }

            return m_text;
        }

        public URLString? GetURL()
        {
            // validate request
            if (m_itemType != ShareItemType.URL)
            {
                DebugLogger.LogError(EssentialKitDomain.Default, "Invalid request.");
                return null;
            }

            return m_url;
        }

        public byte[] GetFileData(out string mimeType, out string fileName)
        { 
            // set default reference values
            mimeType    = null;
            fileName    = null;

            // validate request
            if ((m_itemType != ShareItemType.ImageData) && (m_itemType  != ShareItemType.FileData))
            {
                DebugLogger.LogError(EssentialKitDomain.Default, "Invalid request.");
                return null;
            }

            // set reference values
            mimeType    = m_mimeType;
            fileName    = m_fileName;

            return m_rawData;
        }

        #endregion

        #region Nested types

        public enum ShareItemType
        {
            None    = 0,

            Text,

            URL,

            ImageData,
            
            FileData,

            Screenshot,
        }

        #endregion
    }
}