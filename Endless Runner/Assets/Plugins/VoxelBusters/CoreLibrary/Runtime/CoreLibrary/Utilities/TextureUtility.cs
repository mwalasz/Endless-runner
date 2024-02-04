using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public static class TextureUtility
    {
        #region Encode methods

        public static string GetMimeType(TextureEncodingFormat encodeFormat)
        {
            switch (encodeFormat)
            {
                case TextureEncodingFormat.JPG:
                    return MimeType.kJPGImage;

                case TextureEncodingFormat.PNG:
                    return MimeType.kPNGImage;

                default:
                    throw VBException.SwitchCaseNotImplemented(encodeFormat);
            }
        }

        public static byte[] Encode(this Texture2D texture)
        {
            string mimeType;
            return Encode(texture, out mimeType);
        }

        public static byte[] Encode(this Texture2D texture, out string mimeType)
        {
            switch (texture.format)
            {
                case TextureFormat.Alpha8:
                case TextureFormat.ARGB32:
                case TextureFormat.ARGB4444:
                case TextureFormat.RGBA32:
                case TextureFormat.RGBA4444:
                case TextureFormat.BGRA32:
                case TextureFormat.RGBAHalf:
                case TextureFormat.RGBAFloat:
                case TextureFormat.PVRTC_RGBA2:
                case TextureFormat.PVRTC_RGBA4:
                    mimeType = MimeType.kPNGImage;
                    return texture.EncodeToPNG();

                default:
                    mimeType = MimeType.kJPGImage;
                    return texture.EncodeToJPG();
            }
        }

        public static byte[] Encode(this Texture2D texture, TextureEncodingFormat encodeFormat)
        {
            switch (encodeFormat)
            {
                case TextureEncodingFormat.JPG:
                    return ImageConversion.EncodeToJPG(texture);

                case TextureEncodingFormat.PNG:
                    return ImageConversion.EncodeToPNG(texture);

                default:
                    throw VBException.SwitchCaseNotImplemented(encodeFormat);
            }
        }

        public static byte[] Encode(this Texture2D texture, TextureEncodingFormat encodeFormat, out string mimeType)
        {
            string  textEncodingName;
            return Encode(texture, encodeFormat, out mimeType, out textEncodingName);
        }

        public static byte[] Encode(this Texture2D texture, TextureEncodingFormat encodeFormat, out string mimeType, out string textEncodingName)
        {
            // set default values
            mimeType            = null;
            textEncodingName    = TextEncodingFormat.kUTF8;

            // convert to specified format
            byte[]  data        = null;
            switch (encodeFormat)
            {
                case TextureEncodingFormat.PNG:
                    data        = texture.EncodeToPNG();
                    mimeType    = MimeType.kPNGImage;
                    break;

                case TextureEncodingFormat.JPG:
                    data        = texture.EncodeToJPG();
                    mimeType    = MimeType.kJPGImage;
                    break;
            }

            return data;
        }

        #endregion

        #region Static methods

        public static string TakeScreenshot(string fileName)
        {
            return TakeScreenshot(Application.persistentDataPath, fileName);
        }

        public static string TakeScreenshot(string directory, string fileName)
        {
            string filePath = directory + "/" + fileName;

            IOServices.CreateDirectory(directory);

            // delete existing file
            IOServices.DeleteFileOrDirectory(filePath);

            // start Capturing
            ScreenCapture.CaptureScreenshot(fileName);

            return filePath;
        }

        #endregion
    }
}