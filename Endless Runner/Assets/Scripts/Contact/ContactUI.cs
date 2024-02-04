using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Networking.UnityWebRequest;

public class ContactUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image icon;
    private string email;
    [SerializeField] private Button sendButton;
    private SendInvitation sendInvitation;

    private void Start()
    {
        sendInvitation = GetComponent<SendInvitation>();
        sendButton.onClick.AddListener(SendEmail);
        Debug.Log(sendInvitation);
    }

    public void SetContact(string name, bool hasEmail, string email)
    {
        nameText.text = name;
        icon.gameObject.SetActive(hasEmail);
        this.email = email;
    }

    private void SendEmail()
    {
        Debug.Log(sendInvitation);
        sendInvitation.SendText(email);
    }
}
