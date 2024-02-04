using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    [System.Serializable]
    public class Asset
    {
        #region Properties

        public byte[] Data { get; private set; }

        public string MimeType { get; private set; }

        public string Name { get; private set; }

        #endregion

        #region Constructors

        public Asset(byte[] data, string mimeType, string name)
        {
            // set properties
            Data        = data;
            MimeType    = mimeType;
            Name        = name;
        }

        #endregion

        #region Create methods

        public static Asset CreatePNGAsset(byte[] data, string name)
        {
            return new Asset(data, CoreLibrary.MimeType.kPNGImage, name);
        }

        public static Asset CreateJPGAsset(byte[] data, string name)
        {
            return new Asset(data, CoreLibrary.MimeType.kJPGImage, name);
        }

        public static Asset CreateMP4Asset(byte[] data, string name)
        {
            return new Asset(data, CoreLibrary.MimeType.kMP4Video, name);
        }

        public static Asset CreatePDFAsset(byte[] data, string name)
        {
            return new Asset(data, CoreLibrary.MimeType.kPDF, name);
        }

        public static Asset CreateTextAsset(byte[] data, string name)
        {
            return new Asset(data, CoreLibrary.MimeType.kPlainText, name);
        }

        #endregion
    }
}