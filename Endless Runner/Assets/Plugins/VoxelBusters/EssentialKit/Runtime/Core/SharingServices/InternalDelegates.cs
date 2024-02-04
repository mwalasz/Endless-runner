using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;

namespace VoxelBusters.EssentialKit.SharingServicesCore
{
    public delegate void MailComposerClosedInternalCallback(MailComposerResultCode resultCode, Error error);

    public delegate void MessageComposerClosedInternalCallback(MessageComposerResultCode resultCode, Error error);

    public delegate void ShareSheetClosedInternalCallback(ShareSheetResultCode resultCode, Error error);

    public delegate void SocialShareComposerClosedInternalCallback(SocialShareComposerResultCode resultCode, Error error);
}