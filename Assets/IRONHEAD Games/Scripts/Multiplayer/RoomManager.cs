using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class RoomManager : MonoBehaviourPunCallbacks
{
    /*[SerializeField]
    TextMeshProUGUI OccupancyRateText_ForSchool;
    [SerializeField]
    TextMeshProUGUI OccupancyRateText_ForOutdoor;*/
    AvatarSelectionManager avatarManager;

    int currentPlayers = 0;

    string mapType;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        /*if(PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.JoinLobby();
        }*/

        if(PhotonNetwork.IsConnectedAndReady)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region UI CallBack Methods

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();

    }

    public void OnEnterRoomButtonClicked_Outdoor()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties, 0); //0 = no max players
    }

    public void OnEnterRoomButtonClicked_School()
    {
        mapType = MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL;
        ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType } };
        PhotonNetwork.JoinRandomRoom(expectedCustomRoomProperties,0); //0 = no max players
    }
    #endregion

    #region Photon Callback Methods
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        DebugManagerScript.Instance.AddMessage("Error joining random room:" + returnCode.ToString() + ":" + message);

        CreateAndJoinRoom();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        DebugManagerScript.Instance.AddMessage("Joined to lobby");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    public override void OnConnectedToMaster()
    {
        DebugManagerScript.Instance.AddMessage("Connected to servers again");
        PhotonNetwork.JoinLobby();
    }

    //NOTE: Only called when host joins
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        DebugManagerScript.Instance.AddMessage("Player " + PhotonNetwork.NickName + " joined");

        this.currentPlayers++;

        /*if(PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(MultiplayerVRConstants.MAP_TYPE_KEY))
        {
            object mapType;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(MultiplayerVRConstants.MAP_TYPE_KEY, out mapType)) ;
            {
                DebugManagerScript.Instance.AddMessage("Joined room of map type:" + (string)mapType);
            }

            if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_SCHOOL)
            {
                PhotonNetwork.LoadLevel("World School");
            }
            else if ((string)mapType == MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR) 
            {
                PhotonNetwork.LoadLevel("World Outdoor");
            }

        }*/
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        DebugManagerScript.Instance.AddMessage("Remote Player " + newPlayer.NickName + " joined");
    }
    #endregion

    public void CreateAndJoinRoom()
    {
        string randomRoomName = "Room_" + mapType + Random.Range(0, 10000);

        RoomOptions room = new RoomOptions();
        room.IsOpen = true;
        room.IsVisible = true;
        room.MaxPlayers = 4;

        //1. outdoor = "outdoor" map
        //2. school = "school" map

        string[] roomPropsInLobby = {MultiplayerVRConstants.MAP_TYPE_KEY};
        ExitGames.Client.Photon.Hashtable customRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MultiplayerVRConstants.MAP_TYPE_KEY, mapType}  };



        room.CustomRoomPropertiesForLobby = roomPropsInLobby;

        PhotonNetwork.CreateRoom(randomRoomName, room);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomList.Count == 0)
        {
            //OccupancyRateText_ForSchool.text = 0 + " / " + 20;
            //OccupancyRateText_ForOutdoor.text = 0 + " / " + 20;
        }

        foreach(RoomInfo room in roomList)
        {
            DebugManagerScript.Instance.AddMessage(room.Name);

            if(room.Name.Contains(MultiplayerVRConstants.MAP_TYPE_VALUE_OUTDOOR))
            {
                //update outdoor
                //OccupancyRateText_ForOutdoor.text = room.PlayerCount + " / " + 20;
            }
            else
            {
               // OccupancyRateText_ForSchool.text = room.PlayerCount + " / " + 20;
            }
        }
    }


}
