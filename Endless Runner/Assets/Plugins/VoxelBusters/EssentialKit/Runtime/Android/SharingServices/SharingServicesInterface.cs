#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins.Android;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Android
{
    public sealed class SharingServicesInterface : NativeSharingInterfaceBase, INativeSharingInterface
    {
        #region Constructors

        public SharingServicesInterface()
            : base(isAvailable: true)
        {
        }

        #endregion

        #region Base methods

        public override bool CanSendMail()
        {
            return MailComposer.CanSendMail();
        }

        public override INativeMailComposer CreateMailComposer()
        {
            return new MailComposer();
        }

        public override bool CanSendText()
        {
            return MessageComposer.CanSendText();
        }

        public override bool CanSendAttachments()
        {
            return MessageComposer.CanSendAttachments();
        }

        public override bool CanSendSubject()
        {
            return MessageComposer.CanSendSubject();
        }

        public override INativeMessageComposer CreateMessageComposer()
        {
            return new MessageComposer();
        }

        public override INativeShareSheet CreateShareSheet()
        {
            return new ShareSheet();
        }

        public override bool IsSocialShareComposerAvailable(SocialShareComposerType composerType)
        {
            return SocialShareComposer.IsComposerAvailable(composerType);
        }

        public override INativeSocialShareComposer CreateSocialShareComposer(SocialShareComposerType composerType)
        {
            return new SocialShareComposer(composerType);
        }

        #endregion
    }
}
#endif