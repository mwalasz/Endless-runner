using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.EssentialKit;
using static UnityEngine.Networking.UnityWebRequest;

public class SendInvitation : MonoBehaviour
{
    [SerializeField] private string invitationMessage = "Hey, I am playing this game. Join me!";
    public void SendText(string email)
    {
        MailComposer composer = MailComposer.CreateInstance();
        composer.SetToRecipients(email);
        //composer.SetCcRecipients(new string[1] { null });
        //composer.SetBccRecipients(new string[1] { null });

        composer.SetSubject("Invitation to play");
        composer.SetBody(invitationMessage, false);//Pass true if string is html content
        composer.SetCompletionCallback((result, error) => {
            DebugText.Instance.Log("Mail composer was closed. Result code: " + result.ResultCode);
            Debug.Log("Mail composer was closed. Result code: " + result.ResultCode);
        });
        composer.Show();
    }
}
