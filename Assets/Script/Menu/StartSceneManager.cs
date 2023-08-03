using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class StartSceneManager : MonoBehaviourPunCallbacks
{
    public GameObject panel;
    public GameObject selectpanel;
    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotspot = new Vector2(9,4);
    void Start()
    {
        if(PhotonNetwork.IsConnected == true)
        {
            PhotonNetwork.Disconnect();
        }
        Cursor.SetCursor(null,hotspot,cursorMode);

    }
    public void ClickStart()
    {
        // if(PhotonNetwork.IsConnected == true)
        // {
        //     SceneManager.LoadScene("Lobby");
        //     return;
        // }
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("click start");
        if(PhotonNetwork.IsConnected == true)
        {
            SceneManager.LoadScene("Lobby");

        }
    }
    public void OpenSelect()
    {
        selectpanel.SetActive(true);
    }
    public void ClickOne()
    {
        SceneManager.LoadScene("Gomoku_one_people");
    }
    public void CloseSelect()
    {
        selectpanel.SetActive(false);
    }
    public void ClickLeave()
    {
        Application.Quit();
    }
    public void ClickCredit()
    {
        panel.SetActive(true);
    }
    public void CloseCredit()
    {
        panel.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("connect to server");
        SceneManager.LoadScene("Lobby");
        // base.OnConnectedToMaster();
    }
}
