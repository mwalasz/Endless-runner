using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.SharingServicesCore.Simulator
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
            return false;
        }

        public override bool CanSendAttachments()
        {
            return false;
        }

        public override bool CanSendSubject()
        {
            return false;
        }

        public override INativeMessageComposer CreateMessageComposer()
        {
            return new NullMessageComposer();
        }

        public override INativeShareSheet CreateShareSheet()
        {
            return new NullShareSheet();
        }

        public override bool IsSocialShareComposerAvailable(SocialShareComposerType composerType)
        {
            return false;
        }

        public override INativeSocialShareComposer CreateSocialShareComposer(SocialShareComposerType composerType)
        {
            return new NullSocialShareComposer(composerType);
        }

        #endregion
    }
}