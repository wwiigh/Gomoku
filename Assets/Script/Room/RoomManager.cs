using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_Text room_name;
    public TMP_Text players;
    public Button start_button;
    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotspot = new Vector2(9,4);
    // Start is called before the first frame update
    
    void Start()
    {
        if(PhotonNetwork.CurrentLobby == null)
        {
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            UpdatePlayer();
            room_name.text = PhotonNetwork.CurrentRoom.Name;
        }
        start_button.interactable = PhotonNetwork.IsMasterClient;
        Cursor.SetCursor(null,hotspot,cursorMode);
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        start_button.interactable = PhotonNetwork.IsMasterClient;
        // base.OnMasterClientSwitched(newMasterClient);
    }
    public void UpdatePlayer()
    {
        string player_list = "";
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            player_list += "玩家姓名: "+ player.Value.NickName + "\r\n";
        }
        players.text = player_list;
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayer();
        // base.OnPlayerEnteredRoom(newPlayer);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayer();
        // base.OnPlayerLeftRoom(otherPlayer);
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
        // base.OnLeftRoom();
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Gomoku");
    }
    

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient == false)return;
        if(PhotonNetwork.CurrentRoom.PlayerCount != 2)start_button.interactable = false;
        else start_button.interactable = true;
    }
}
