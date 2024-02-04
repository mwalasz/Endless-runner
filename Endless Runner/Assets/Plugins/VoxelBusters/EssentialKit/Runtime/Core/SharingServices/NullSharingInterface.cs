using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public sealed class NullSharingInterface : NativeSharingInterfaceBase, INativeSharingInterface
    {
        #region Constructors

        public NullSharingInterface()
            : base(isAvailable: false)
        { }

        #endregion

        #region Base methods

        public override bool CanSendMail()
        {
            return false;
        }

        public override INativeMailComposer CreateMailComposer()
        {
            return new NullMailComposer();
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