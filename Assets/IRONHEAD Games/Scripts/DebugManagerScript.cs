using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugManagerScript : MonoBehaviour
{
    public static DebugManagerScript Instance;

    private List<string> messageBuffer = new List<string>();

    public TMPro.TextMeshProUGUI outputDisplay;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddMessage(string newMessage)
    {
        if(messageBuffer.Count == 6)
        {
            messageBuffer[0] = messageBuffer[1];
            messageBuffer[1] = messageBuffer[2];
            messageBuffer[2] = messageBuffer[3];
            messageBuffer[3] = messageBuffer[4];
            messageBuffer[4] = messageBuffer[5];
            messageBuffer[5] = newMessage;
        }
        else
        {
            messageBuffer.Add(newMessage);
        }

        RefreshDebugDisplay();
    }

    private void RefreshDebugDisplay()
    {
        string display = "";

        foreach(string s in messageBuffer)
        {
            display += s;
            display += "\n";
        }

        this.outputDisplay.text = display;
    }

    public void ClearDebug()
    {
        messageBuffer = new List<string>();
        RefreshDebugDisplay();
    }
}
