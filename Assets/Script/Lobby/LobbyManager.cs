using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField InputRoomName;
    public TMP_InputField InputPlayerName;
    public TMP_Text RoomList;
    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotspot = new Vector2(9,4);
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("lobby "+ PhotonNetwork.CurrentLobby);
        if(PhotonNetwork.IsConnected == false)
        {
            SceneManager.LoadScene("Start");
        }
        else
        {
            if(PhotonNetwork.CurrentLobby == null)
            {
                Debug.Log("now joining");
                PhotonNetwork.JoinLobby();
            }
        }
        Cursor.SetCursor(null,hotspot,cursorMode);
    }
    // public override void OnEnable()
    // {
    //     if(PhotonNetwork.IsConnected == false)
    //     {
    //         SceneManager.LoadScene("Start");
    //     }
    //     else
    //     {
    //         if(PhotonNetwork.CurrentLobby == null)
    //         {
    //             PhotonNetwork.JoinLobby();
    //         }
    //     }
    //     Cursor.SetCursor(null,hotspot,cursorMode);
    //     base.OnEnable();
    // }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        // base.OnConnectedToMaster();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Join lobby");
        // base.OnJoinedLobby();
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Join room");
        SceneManager.LoadScene("Room");
        // base.OnJoinedRoom();
    }
    public string GetRoomName()
    {
        string room_name = InputRoomName.text;
        room_name = room_name.Trim();
        return room_name;
    }
    public string GetPlayerName()
    {
        string player_name = InputPlayerName.text;
        player_name = player_name.Trim();
        return player_name;
    }
    public void LeaveLoby()
    {
        // Application.Quit();
        SceneManager.LoadScene("Start");
    }
    public void CreateRoom()
    {
        string room_name = GetRoomName();
        string player_name = GetPlayerName();
        if(room_name.Length > 0 && player_name.Length > 0)
        {
            PhotonNetwork.CreateRoom(room_name);
            PhotonNetwork.LocalPlayer.NickName = player_name;
        }
        else 
        {
            Debug.Log("Invalid Room Name or player name");
        }
    }
    public void JoinRoom()
    {
        string room_name = GetRoomName();
        string player_name = GetPlayerName();
        if(room_name.Length > 0 && player_name.Length > 0)
        {
            PhotonNetwork.JoinRoom(room_name);
            PhotonNetwork.LocalPlayer.NickName = player_name;
        }
        else 
        {
            Debug.Log("Invalid Room Name or player name");
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        string totalroom = "";
        foreach (var roominfo in roomList)
        {
            if(roominfo.PlayerCount < 1)continue;
            totalroom += roominfo.Name + " 人數:" + roominfo.PlayerCount+"人" + "\r\n";   
        }
        RoomList.text = totalroom;
    }
    public override void OnLeftLobby()
    {
        SceneManager.LoadScene("Start");
        // PhotonNetwork.Disconnect();
        base.OnLeftLobby();
    }
        
}
