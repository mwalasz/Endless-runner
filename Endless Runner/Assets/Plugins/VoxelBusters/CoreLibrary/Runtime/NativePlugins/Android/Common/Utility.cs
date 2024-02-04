#if UNITY_ANDROID
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace VoxelBusters.CoreLibrary.NativePlugins.Android
{
    public static class Utility
    {
        public static sbyte[] ToSBytes(this byte[] from)
        {
            if (from == null)
                return null;

            int length = from.Length;
            sbyte[] to = new sbyte[length];
            Buffer.BlockCopy(from, 0, to, 0, length); //There is an issue with BlockCopy on some unity versions. This got fixed in later versions : https://issuetracker.unity3d.com/issues/android-sbyte-type-is-considered-to-be-not-primitive-when-compiling-il2cpp-code

            return to;
        }

        public static byte[] ToBytes(this sbyte[] from)
        {
            if (from == null)
                return null;

            return (byte[])(Array)from;
        }

        public static Color GetColor(this AndroidJavaObject nativeObject)
        {
            float red       = nativeObject.Call<int>("getRed") / 255;
            float green     = nativeObject.Call<int>("getRed") / 255;
            float blue      = nativeObject.Call<int>("getRed") / 255;
            float alpha     = nativeObject.Call<int>("alpha")  / 255.0f;

            return new Color(red, green, blue, alpha);
        }

        public static void TakeScreenshot(Action<byte[], string> callback)
        {
            SurrogateCoroutine.WaitForEndOfFrameAndInvoke(() =>
            {
                Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
                string mimeType;
                byte[] data = texture.Encode(TextureEncodingFormat.JPG, out mimeType);
                callback(data, mimeType);
            });
        }
    }
}
#endif