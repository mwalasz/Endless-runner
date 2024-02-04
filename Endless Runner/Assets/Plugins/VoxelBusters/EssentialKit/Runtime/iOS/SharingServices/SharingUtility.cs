#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    internal static class SharingUtility
    {
        #region Converter methods

        public static MailComposerResultCode ConvertToMailComposerResultCode(MFMailComposeResult result)
        {
            switch (result)
            {
                case MFMailComposeResult.MFMailComposeResultCancelled:
                    return MailComposerResultCode.Cancelled;

                case MFMailComposeResult.MFMailComposeResultSaved:
                    return MailComposerResultCode.Saved;

                case MFMailComposeResult.MFMailComposeResultSent:
                    return MailComposerResultCode.Sent;

                case MFMailComposeResult.MFMailComposeResultFailed:
                    return MailComposerResultCode.Failed;

                default:
                    throw VBException.SwitchCaseNotImplemented(result);
            }
        }

        public static MessageComposerResultCode ConvertToMessageComposerResultCode(MFMessageComposeResult result)
        {
            switch (result)
            {
                case MFMessageComposeResult.MFMessageComposeResultCancelled:
                    return MessageComposerResultCode.Cancelled;

                case MFMessageComposeResult.MFMessageComposeResultSent:
                    return MessageComposerResultCode.Sent;

                case MFMessageComposeResult.MFMessageComposeResultFailed:
                    return MessageComposerResultCode.Failed;

                default:
                    throw VBException.SwitchCaseNotImplemented(result);
            }
        }

        public static SocialShareComposerResultCode ConvertToShareComposerResultCode(SLComposeViewControllerResult result)
        {
            switch (result)
            {
                case SLComposeViewControllerResult.SLComposeViewControllerResultCancelled:
                    return SocialShareComposerResultCode.Cancelled;

                case SLComposeViewControllerResult.SLComposeViewControllerResultDone:
                    return SocialShareComposerResultCode.Done;

                default:
                    throw VBException.SwitchCaseNotImplemented(result);
            }
        }

        #endregion
    }
}
#endif