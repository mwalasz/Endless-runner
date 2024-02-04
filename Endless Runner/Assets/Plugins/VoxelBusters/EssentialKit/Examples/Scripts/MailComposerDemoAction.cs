using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VoxelBusters.CoreLibrary.NativePlugins.DemoKit;

namespace VoxelBusters.EssentialKit.Demo
{
	public enum MailComposerDemoActionType
	{
		CanSendMail,
		New,
		SetToRecipients,
		SetCcRecipients,
		SetBccRecipients,
		SetSubject,
		SetBody,
		AddScreenshot,
		AddImage,
		Show,
		ResourcePage
	}

	public class MailComposerDemoAction : DemoActionBehaviour<MailComposerDemoActionType> 
	{}
}
