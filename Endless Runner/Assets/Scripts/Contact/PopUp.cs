using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI popUpText;

    internal void SetPopUp(string text)
    {
        popUpText.text = text;
    }
}
