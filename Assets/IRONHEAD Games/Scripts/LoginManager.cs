using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class LoginManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField PlayerName_InputField;

    #region Unity Methods
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("Calling Photon Connect");
        //PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion


    #region UI callback methods
    public void ConnectToPhotonServer()
    {
        DebugManagerScript.Instance.AddMessage("Attempting to contact Photon Server");
        if (PlayerName_InputField != null)
        {
            PhotonNetwork.NickName = PlayerName_InputField.text;
        }

        PhotonNetwork.ConnectUsingSettings();
    }
    #endregion

    #region Photon Callback methods

    public override void OnConnected()
    {
        Debug.Log("On connect called. Server avail");
        DebugManagerScript.Instance.AddMessage("Callback: OnConnected called. Server avail");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("On connect to master called. Player name =" + PlayerName_InputField.text);
        DebugManagerScript.Instance.AddMessage("On connect to master called. Player name =" + PlayerName_InputField.text);
        DebugManagerScript.Instance.AddMessage("Attempting to load map");

        try
        {
            //PhotonNetwork.LoadLevel("World_Arena");
            PhotonNetwork.LoadLevel("World_School");
            DebugManagerScript.Instance.AddMessage("Map load complete");
        }
        catch
        {
            DebugManagerScript.Instance.AddMessage("Map load failed!");
        }
    }

    public override void OnErrorInfo(ErrorInfo errorInfo)
    {
        DebugManagerScript.Instance.AddMessage("Photon Server Error:" + errorInfo.Info);
    }

    #endregion
}
