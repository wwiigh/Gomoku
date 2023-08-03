using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using HashTable = ExitGames.Client.Photon.Hashtable;
public class GameManager : MonoBehaviourPunCallbacks
{
    public bool oneplayer = false;
    public Button leave_game;
    public Button one_player_leave;
    public Button one_player_to_title;
    public Button one_player_again;
    public TMP_Text title;
    public Player My_identity;
    public enum Player{
        black,white
    }public enum Piece_color{
        black,white,none
    }
    public Player player = Player.black; 
    public Player winner = Player.black; 
    Piece_color[,] pieces = new Piece_color[9,9];
    int last_place_x;
    int last_place_y;
    public bool has_winner = false;
    // Start is called before the first frame update
    void Start()
    {
        if(oneplayer == true)
        {
            title.text = "現在是黑棋";
            My_identity = Player.black;
            init();
            return;
        }
        if(PhotonNetwork.CurrentRoom == null)
        {
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            Debug.Log("now start");
            init();
        }
        if(PhotonNetwork.IsMasterClient == true)
        {
            title.text = "你是黑棋";
            My_identity = Player.black;
        }
        else
        {
            title.text = "你是白棋";
            My_identity = Player.white;
        }
    }
    public void init()
    {
        this.GetComponent<Board>().init();
        for(int i=0;i<9;i++)
        {
            for(int j=0;j<9;j++)
            {
                pieces[i,j] = Piece_color.none;
            }
        }
    }
    public void Set_Piece(int x,int y,Piece_color color)
    {
        pieces[x,y] = color;
        last_place_x = x;
        last_place_y = y;
    }
    public void Change_Player(int x,int y,Piece_color color)
    {
        if(player == Player.black)
        {
            player = Player.white;
            if(oneplayer == true)
            {
                title.text = "現在是白棋";
                My_identity = Player.white;
            }
        }
        else
        {
            player = Player.black;
            if(oneplayer == true)
            {
                title.text = "現在是黑棋";
                My_identity = Player.black;
            }
        }
        if(oneplayer == true)
        {
            CheckWinner();
            return;
        }
        HashTable hashtable = new HashTable();
        hashtable.Add("player", player);
        Refresh_Board(x,y,color,hashtable);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
    public void Refresh_Board(int x,int y,Piece_color color,HashTable hashtable)
    {
        // HashTable hashtable = new HashTable();
        hashtable.Add("x", x);
        hashtable.Add("y", y);
        hashtable.Add("color", color);
        // PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        // pieces[x,y] = color;

    }
    public void Set_Player(Player _player)
    {
        player = _player;
    }
    // Update is called once per frame
    void Update()
    {
        if(has_winner == true)
        {
            if(winner == Player.black)title.text = "黑色贏了";
            else title.text = "白色贏了";
            if(oneplayer == false)
            {
                leave_game.gameObject.SetActive(true);
            }
            else
            {
                one_player_leave.gameObject.SetActive(true);
                one_player_to_title.gameObject.SetActive(true);
                one_player_again.gameObject.SetActive(true);
            }
        }
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, HashTable changedProps)
    {
        // Debug.Log(targetPlayer);
        Debug.Log("change player");
        GameManager.Player player = (GameManager.Player)changedProps["player"];
        int x = (int)changedProps["x"];
        int y = (int)changedProps["y"];
        Piece_color color = (Piece_color)changedProps["color"];
        Set_Piece(x,y,color);
        Set_Player(player);
        CheckWinner();
    }

    private void CheckWinner()
    {
        int center_x = last_place_x;
        int center_y = last_place_y;

        for(int have = 0;have <= 2;have ++)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (check_already_have(-i, -j, have, center_x, center_y) == false) continue;
                    if (j == 0 && i == 0) continue;
                    int count = 1;
                    while (count < 5 - have)
                    {
                        int target_x = center_x + count * i;
                        int target_y = center_y + count * j;
                        if (target_x < 0 || target_x >= 9) break;
                        if (target_y < 0 || target_y >= 9) break;
                        if(player == Player.black && pieces[target_x,target_y] == Piece_color.white)
                        {
                            count ++;
                        }
                        else if(player == Player.white && pieces[target_x,target_y] == Piece_color.black)
                        {
                            count ++;
                        }
                        else break;
                    }

                    if (count == 5 - have)
                    {
                        if(player == Player.black)winner = Player.white;
                        else winner = Player.black;
                        has_winner = true;
                    }
                }
            }
        }
    }
    private bool check_already_have(int i,int j,int have, int n_x,int n_y)
    {
        int count = 0;
        while(count < have)
        {
        int target_x = n_x + (count+1) * i;
            int target_y = n_y + (count+1) * j;
            if (target_x < 0 || target_x >= 9) break;
            if (target_y < 0 || target_y >= 9) break;
            if(player == Player.black && pieces[target_x,target_y] == Piece_color.white)
            {
                count ++;
            }
            else if(player == Player.white && pieces[target_x,target_y] == Piece_color.black)
            {
                count ++;
            }
            else break;
        }
        if (count == have)
        {
            return true;
        }
        return false;
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
    public void Reset()
    {
        this.GetComponent<Board>().Reset();
        for(int i=0;i<9;i++)
        {
            for(int j=0;j<9;j++)
            {
                pieces[i,j] = Piece_color.none;
            }
        }
        if(oneplayer == true) 
        {
            title.text = "現在是黑棋";
            My_identity = Player.black;
        }
        else
        {
            if(PhotonNetwork.IsMasterClient == true)
            {
                title.text = "你是黑棋";
                My_identity = Player.black;
            }
            else
            {
                title.text = "你是白棋";
                My_identity = Player.white;
            }
        }
        player = Player.black;
        one_player_leave.gameObject.SetActive(false);
        one_player_to_title.gameObject.SetActive(false);
        one_player_again.gameObject.SetActive(false);
        leave_game.gameObject.SetActive(false);
        has_winner = false;
    }
    public void Retrun_to_start()
    {
        SceneManager.LoadScene("Start");
    }
    public void ExitGames()
    {
        Application.Quit();
    }
}
