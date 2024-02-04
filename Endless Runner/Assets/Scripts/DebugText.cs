using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    public static DebugText Instance;
    [SerializeField] private TextMeshProUGUI debugText;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void Log(string message)
    {
        debugText.text = message;
    }
}
