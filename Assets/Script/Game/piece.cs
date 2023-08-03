using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using HashTable = ExitGames.Client.Photon.Hashtable;
public class piece : MonoBehaviourPunCallbacks
{
    public int index_x;
    public int index_y;
    public Texture2D cursor_touch;
    public Texture2D cursor_normal;
    public GameObject piece_black;
    public GameObject piece_white;
    GameObject piece_here = null;
    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotspot = new Vector2(9,4);
    GameManager gameManager;
    // public PhotonView _pv;
    // Start is called before the first frame update
    void Start()
    {
        // _pv = GetComponent<PhotonView>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.has_winner == true)
        {
            this.gameObject.SetActive(false);
        }
    }
    public void OnClick()
    {
        // if(_pv.IsMine == false)return;
        // this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1);
        if(check_my_turn()==false)return;
        if(piece_here!=null)return;
        if(gameManager.oneplayer == true)
        {
            GameManager.Piece_color _color;
            if(gameManager.player == GameManager.Player.black)
            {
                GameObject obj = Instantiate(piece_black,new Vector3(index_x*0.8f,index_y*0.8f,0),Quaternion.identity);    
                gameManager.Set_Piece(index_x + 4,index_y + 4,GameManager.Piece_color.black);
                _color = GameManager.Piece_color.black;
                piece_here = obj;
            }
            else
            {
                GameObject obj =  Instantiate(piece_white,new Vector3(index_x*0.8f,index_y*0.8f,0),Quaternion.identity);    
                gameManager.Set_Piece(index_x + 4,index_y + 4,GameManager.Piece_color.white);
                _color = GameManager.Piece_color.white;
                piece_here = obj;
            }
            gameManager.Change_Player(index_x + 4,index_y + 4,_color);
            return;
        }
        HashTable hashtable = new HashTable();
        GameManager manager = FindObjectOfType<GameManager>(); 
        GameManager.Piece_color color;
        if(manager.player == GameManager.Player.black)
        {
            GameObject obj = PhotonNetwork.Instantiate("Black",new Vector3(index_x*0.8f,index_y*0.8f,0),Quaternion.identity);    
            manager.Set_Piece(index_x + 4,index_y + 4,GameManager.Piece_color.black);
            color = GameManager.Piece_color.black;
            piece_here = obj;
        }
        else
        {
            GameObject obj = PhotonNetwork.Instantiate("White",new Vector3(index_x*0.8f,index_y*0.8f,0),Quaternion.identity);    
            manager.Set_Piece(index_x + 4,index_y + 4,GameManager.Piece_color.white);
            color = GameManager.Piece_color.white;
            piece_here = obj;
        }
        manager.Change_Player(index_x + 4,index_y + 4,color);
        // manager.Refresh_Board();
        // hashtable.Add("player",manager.player);
        // hashtable.Add("x", index_x);
        // hashtable.Add("y", index_y);
        // hashtable.Add("color", 1);
        // PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
        this.enabled = false;
    }
    public void Change_normal_mouse()
    {
        if(check_my_turn()==false || piece_here!=null)
        {
            Cursor.SetCursor(null,hotspot,cursorMode);
            return;
        }
        Cursor.SetCursor(null,hotspot,cursorMode);
        // Debug.Log("here");
    }
    public void Change_touch_mouse()
    {
        if(check_my_turn()==false  || piece_here!=null)
        {
            Cursor.SetCursor(null,hotspot,cursorMode);
            return;
        }
        // Debug.Log("here2");
        Cursor.SetCursor(cursor_touch,hotspot,cursorMode);
    }
    public bool check_my_turn()
    {
        GameManager manager = FindObjectOfType<GameManager>();
        return manager.My_identity == manager.player;
    }
    void OnDestroy()
    {
        if(piece_here != null)
        {
            Destroy(piece_here);
        }
        piece_here = null;
    }
}
