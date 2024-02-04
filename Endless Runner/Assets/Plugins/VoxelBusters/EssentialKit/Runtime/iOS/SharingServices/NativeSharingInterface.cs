#if UNITY_IOS || UNITY_TVOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.SharingServicesCore.iOS
{
    public sealed class NativeSharingInterface : NativeSharingInterfaceBase, INativeSharingInterface
    {
        #region Constructors

        public NativeSharingInterface()
            : base(isAvailable: true)
        { }

        #endregion

        #region Base methods

        public override bool CanSendMail()
        {
            return NativeMailComposer.CanSendMail();
        }

        public override INativeMailComposer CreateMailComposer()
        {
            return new NativeMailComposer();
        }

        public override bool CanSendText()
        {
            return NativeMessageComposer.CanSendText();
        }

        public override bool CanSendAttachments()
        {
            return NativeMessageComposer.CanSendAttachments();
        }

        public override bool CanSendSubject()
        {
            return NativeMessageComposer.CanSendSubject();
        }

        public override INativeMessageComposer CreateMessageComposer()
        {
            return new NativeMessageComposer();
        }

        public override INativeShareSheet CreateShareSheet()
        {
            return new NativeShareSheet();
        }

        public override bool IsSocialShareComposerAvailable(SocialShareComposerType composerType)
        {
            return NativeSocialShareComposer.IsComposerAvailable(composerType);
        }

        public override INativeSocialShareComposer CreateSocialShareComposer(SocialShareComposerType composerType)
        {
            return new NativeSocialShareComposer(composerType);
        }

        #endregion
    }
}
#endif