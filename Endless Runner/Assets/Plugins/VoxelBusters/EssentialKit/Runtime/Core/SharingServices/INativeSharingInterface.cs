using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public interface INativeSharingInterface : INativeFeatureInterface
    {
        #region Mail composer methods

        bool CanSendMail();

        INativeMailComposer CreateMailComposer();

        #endregion

        #region Message composer methods

        bool CanSendText();

        bool CanSendAttachments();

        bool CanSendSubject();

        INativeMessageComposer CreateMessageComposer();

        #endregion

        #region Share sheet methods

        INativeShareSheet CreateShareSheet();

        #endregion

        #region Social share composer methods

        bool IsSocialShareComposerAvailable(SocialShareComposerType composerType);
        
        INativeSocialShareComposer CreateSocialShareComposer(SocialShareComposerType composerType);

        #endregion
    }
}