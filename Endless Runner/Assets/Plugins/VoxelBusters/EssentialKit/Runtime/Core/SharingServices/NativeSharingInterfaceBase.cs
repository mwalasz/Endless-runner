using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public abstract class NativeSharingInterfaceBase : NativeFeatureInterfaceBase, INativeSharingInterface
    {
        #region Constructors

        protected NativeSharingInterfaceBase(bool isAvailable)
            : base(isAvailable)
        { }

        #endregion

        #region INativeSharingInterface implementation

        public abstract bool CanSendMail();
        
        public abstract INativeMailComposer CreateMailComposer();

        public abstract bool CanSendText();

        public abstract bool CanSendAttachments();

        public abstract bool CanSendSubject(); 

        public abstract INativeMessageComposer CreateMessageComposer();

        public abstract INativeShareSheet CreateShareSheet();
        
        public abstract bool IsSocialShareComposerAvailable(SocialShareComposerType composerType);

        public abstract INativeSocialShareComposer CreateSocialShareComposer(SocialShareComposerType composerType);

        #endregion
    }
}