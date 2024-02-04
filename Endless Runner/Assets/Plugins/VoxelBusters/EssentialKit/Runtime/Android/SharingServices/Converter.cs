#if UNITY_ANDROID
using System;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    internal static class Converter
    {
        public static MailComposerResultCode from(NativeMailComposerResult result)
        {
            switch(result)
            {
                case NativeMailComposerResult.Cancelled:
                    return MailComposerResultCode.Cancelled;
                case NativeMailComposerResult.Sent:
                    return MailComposerResultCode.Sent;
                case NativeMailComposerResult.Unknown:
                    return MailComposerResultCode.Unknown;
                default:
                    return MailComposerResultCode.Unknown;
            }
        }

        public static MessageComposerResultCode from(NativeMessageComposerResult result)
        {
            switch (result)
            {
                case NativeMessageComposerResult.Cancelled:
                    return MessageComposerResultCode.Cancelled;
                case NativeMessageComposerResult.Sent:
                    return MessageComposerResultCode.Sent;
                case NativeMessageComposerResult.Unknown:
                    return MessageComposerResultCode.Unknown;
                default:
                    return MessageComposerResultCode.Unknown;
            }
        }

        internal static ShareSheetResultCode from(NativeShareSheetResult result)
        {
            switch (result)
            {
                case NativeShareSheetResult.Cancelled:
                    return ShareSheetResultCode.Cancelled;
                case NativeShareSheetResult.Done:
                    return ShareSheetResultCode.Done;
                case NativeShareSheetResult.Unknown:
                    return ShareSheetResultCode.Unknown;
                default:
                    return ShareSheetResultCode.Unknown;
            }
        }

        internal static NativeSocialShareComposerType from(SocialShareComposerType composerType)
        {
            switch (composerType)
            {
                case SocialShareComposerType.Facebook:
                    return NativeSocialShareComposerType.Facebook;
                case SocialShareComposerType.Twitter:
                    return NativeSocialShareComposerType.Twitter;
                case SocialShareComposerType.WhatsApp:
                    return NativeSocialShareComposerType.Whatsapp;
                default:
                    throw VBException.SwitchCaseNotImplemented(composerType);
            }
        }

        internal static SocialShareComposerResultCode from(NativeSocialShareComposerResult result)
        {
            switch (result)
            {
                case NativeSocialShareComposerResult.Cancelled:
                    return SocialShareComposerResultCode.Cancelled;
                case NativeSocialShareComposerResult.Done:
                    return SocialShareComposerResultCode.Done;
                case NativeSocialShareComposerResult.Unknown:
                    return SocialShareComposerResultCode.Unknown;
                default:
                    return SocialShareComposerResultCode.Unknown;
            }
        }
    }
}
#endif
