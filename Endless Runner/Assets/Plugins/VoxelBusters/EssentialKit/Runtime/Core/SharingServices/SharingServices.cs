using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit.SharingServicesCore;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit
{
    /// <summary>
    /// Provides a cross-platform interface to access sharing services.
    /// </summary>
    public static class SharingServices
    {
        #region Static fields

        [ClearOnReload]
        private     static  INativeSharingInterface    s_nativeInterface    = null;

        #endregion

        #region Static properties

        public static SharingServicesUnitySettings UnitySettings { get; private set; }

        internal static INativeSharingInterface NativeInterface => s_nativeInterface;

        #endregion

        #region Static methods

        public static bool IsAvailable()
        {
            return (s_nativeInterface != null) && s_nativeInterface.IsAvailable;
        }

        public static void Initialize(SharingServicesUnitySettings settings)
        {
            Assert.IsArgNotNull(settings, nameof(settings));

            // Set default properties
            UnitySettings           = settings;

            // Configure interface
            s_nativeInterface       = NativeFeatureActivator.CreateInterface<INativeSharingInterface>(ImplementationSchema.SharingServices, UnitySettings.IsEnabled);
        }

        /// <summary>
        /// Shows the mail composer.
        /// </summary>
        /// <param name="toRecipients">To recipients.</param>
        /// <param name="ccRecipients">Cc recipients.</param>
        /// <param name="bccRecipients">Bcc recipients.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        /// <param name="isHtmlBody">If set to <c>true</c> is html body.</param>
        /// <param name="callback">Callback.</param>
        /// <param name="shareItems">Share items.</param>
        public static void ShowMailComposer(string[] toRecipients = null, string[] ccRecipients = null, string[] bccRecipients = null, string subject = null, string body = null, bool isHtmlBody = false, EventCallback<MailComposerResult> callback = null, params ShareItem[] shareItems)
        { 
            // create a new instance and set specified properties
            var     newComposer     = MailComposer.CreateInstance();
            if (toRecipients != null)
            {
                newComposer.SetToRecipients(toRecipients);
            }
            if (ccRecipients != null)
            {
                newComposer.SetCcRecipients(ccRecipients);
            }
            if (bccRecipients != null)
            {
                newComposer.SetBccRecipients(bccRecipients);
            }
            if (subject != null)
            {
                newComposer.SetSubject(subject);
            }
            if (body != null)
            {
                newComposer.SetBody(body, isHtmlBody);
            }
            if (shareItems != null)
            {
                for (int iter = 0; iter < shareItems.Length; iter++)
                {
                    var     item        = shareItems[iter];
                    var     itemType    = item.ItemType;
                    switch (itemType)
                    {
                        case ShareItem.ShareItemType.ImageData:
                        case ShareItem.ShareItemType.FileData:
                            string mimeType, fileName;
                            var     data    = item.GetFileData(out mimeType, out fileName);
                            newComposer.AddAttachment(data, mimeType, fileName);
                            break;

                        case ShareItem.ShareItemType.Screenshot:
                            newComposer.AddScreenshot("screenshot.png");
                            break;
                    }
                }
            }
            if (callback != null)
            {
                newComposer.SetCompletionCallback(callback);
            }
            newComposer.Show();
        }

        /// <summary>
        /// Shows the message composer.
        /// </summary>
        /// <param name="recipients">Recipients.</param>
        /// <param name="subject">Subject.</param>
        /// <param name="body">Body.</param>
        /// <param name="callback">Callback.</param>
        /// <param name="shareItems">Share items.</param>
        public static void ShowMessageComposer(string[] recipients = null, string subject = null, string body = null, EventCallback<MessageComposerResult> callback = null, params ShareItem[] shareItems)
        { 
            // create a new instance and set specified properties
            var     newComposer     = MessageComposer.CreateInstance();
            if (recipients != null)
            {
                newComposer.SetRecipients(recipients);
            }
            if (MessageComposer.CanSendSubject() && (subject != null))
            {
                newComposer.SetSubject(subject);
            }
            if (body != null)
            {
                newComposer.SetBody(body);
            }
            if (MessageComposer.CanSendAttachments() && (shareItems != null))
            {
                for (int iter = 0; iter < shareItems.Length; iter++)
                {
                    var     item        = shareItems[iter];
                    var     itemType    = item.ItemType;
                    switch (itemType)
                    {
                        case ShareItem.ShareItemType.FileData:
                        case ShareItem.ShareItemType.ImageData:
                            string mimeType, fileName;
                            var     data    = item.GetFileData(out mimeType, out fileName);
                            newComposer.AddAttachment(data, mimeType, fileName);
                            break;

                        case ShareItem.ShareItemType.Screenshot:
                            newComposer.AddScreenshot("screenshot.png");
                            break;
                    }
                }
            }
            if (callback != null)
            {
                newComposer.SetCompletionCallback(callback);
            }
            newComposer.Show();
        }

        public static void ShowShareSheet(EventCallback<ShareSheetResult> callback = null, params ShareItem[] shareItems)
        { 
            // create a new instance and set specified properties
            var     newSheet        = ShareSheet.CreateInstance();
            if (shareItems != null)
            {
                for (int iter = 0; iter < shareItems.Length; iter++)
                {
                    var     item        = shareItems[iter];
                    var     itemType    = item.ItemType;
                    switch (itemType)
                    {
                        case ShareItem.ShareItemType.Text:
                            newSheet.AddText(item.GetText());
                            break;

                        case ShareItem.ShareItemType.URL:
                            newSheet.AddURL(item.GetURL().Value);
                            break;

                        case ShareItem.ShareItemType.ImageData:
                            string mimeType, fileName;
                            var     data    = item.GetFileData(out mimeType, out fileName);
                            newSheet.AddImage(data, mimeType);
                            break;

                        case ShareItem.ShareItemType.Screenshot:
                            newSheet.AddScreenshot();
                            break;
                    }
                }
            }
            if (callback != null)
            {
                newSheet.SetCompletionCallback(callback);
            }
            newSheet.Show();
        }

        public static void ShowSocialShareComposer(SocialShareComposerType composerType, EventCallback<SocialShareComposerResult> callback = null, params ShareItem[] shareItems)
        { 
            // create a new instance and set specified properties
            var     newSheet        = SocialShareComposer.CreateInstance(composerType);
            if (shareItems != null)
            {
                for (int iter = 0; iter < shareItems.Length; iter++)
                {
                    var     item        = shareItems[iter];
                    var     itemType    = item.ItemType;
                    switch (itemType)
                    {
                        case ShareItem.ShareItemType.Text:
                            newSheet.SetText(item.GetText());
                            break;

                        case ShareItem.ShareItemType.URL:
                            newSheet.AddURL(item.GetURL().Value);
                            break;

                        case ShareItem.ShareItemType.ImageData:
                            string mimeType, fileName;
                            var     data    = item.GetFileData(out mimeType, out fileName);
                            newSheet.AddImage(data);
                            break;

                        case ShareItem.ShareItemType.Screenshot:
                            newSheet.AddScreenshot();
                            break;
                    }
                }
            }
            if (callback != null)
            {
                newSheet.SetCompletionCallback(callback);
            }
            newSheet.Show();
        }

        #endregion

        #region Convertion methods

        public static void ConvertGifToShareItem(string filePath, SuccessCallback<ShareItem> onSuccess, ErrorCallback onError)
        {
             // Guard cases
            if (!IOServices.FileExists(filePath))
            {
                onError?.Invoke(new Error("File not found."));
                return;
            }

            // Check 

        }


        #endregion
    }
}