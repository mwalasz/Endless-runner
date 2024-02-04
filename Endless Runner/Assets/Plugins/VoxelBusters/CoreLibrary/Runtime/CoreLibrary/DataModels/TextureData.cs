using System;
using UnityEngine;

namespace VoxelBusters.CoreLibrary
{
    public class TextureData
    {
        #region Fields

        private     byte[]      m_rawData;

        private     Texture2D   m_texture;

        #endregion

        #region Constructors

        public TextureData(byte[] rawData)
        {
            // set properties
            m_rawData   = rawData;
            m_texture   = null;
        }

        public TextureData(Texture2D texture)
        {
            // set properties
            m_rawData   = null;
            m_texture   = texture;
        }

        #endregion

        #region Public methods

        public byte[] GetBytes(TextureEncodingFormat format)
        {
            if (m_rawData != null)
            {
                return m_rawData;
            }
            else if (m_texture != null)
            {
                switch (format)
                {
                    case TextureEncodingFormat.JPG:
                        return m_texture.EncodeToJPG();

                    default:
                        return m_texture.EncodeToPNG();
                }
            }

            throw new VBException("Unknown error");
        }

        public Texture2D GetTexture()
        {
            if (m_texture != null)
            {
                return m_texture;
            }
            else if (m_rawData != null)
            {
                m_texture   = CreateTexture(m_rawData);
                return m_texture;
            }

            throw new VBException("Unknown error");
        }

        private Texture2D CreateTexture(byte[] data)
        {
            var     newTexture  = new Texture2D(4, 4);
            newTexture.LoadImage(data);
            newTexture.Apply();

            return newTexture;
        }

        #endregion
    }
}