using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Board : MonoBehaviourPunCallbacks
{
    public float edge_length = 0.8f;
    
    public GameObject piece;
    PhotonView _pv;
    List<GameObject> pieces = new List<GameObject>();
    bool clear_obj = false;    
    // Start is called before the first frame update
    void Start()
    {
        _pv = GetComponent<PhotonView>();
        // init();
    }

    // Update is called once per frame
    void Update()
    {
        // if(clear_obj == true)return;
        // piece[] allpiece = FindObjectsOfType<piece>();
        // if(allpiece.Length == 0)return;
        // foreach(var item in allpiece)
        // {
        //     if(item._pv.IsMine == false)item.gameObject.SetActive(false);
        // }
        // clear_obj = true;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0;
        worldPosition.y *= 100.0f;
        worldPosition.x *= 100.0f;
        // bool check = Check_Pos(worldPosition);
        // if(check == true)
        // {
        //     Cursor.SetCursor(cursor_touch,hotspot,cursorMode);
        // }
        // else
        // {
        //     Cursor.SetCursor(null,hotspot,cursorMode);
        // }
        // Debug.Log(worldPosition);
    }
    public void init()
    {
        // if(PhotonNetwork.IsMasterClient == false)return;
        for(int i=-4;i<=4;i++)
        {
            // Debug.Log("i "+i);
            for(int j=-4;j<=4;j++)
            {
                Debug.Log(" now start instantiate here");
                // Debug.Log("j "+j);
                GameObject obj = Instantiate(piece,new Vector3(i*edge_length,j*edge_length,0),Quaternion.identity);
                // GameObject obj = PhotonNetwork.Instantiate("Piece",new Vector3(i*edge_length,j*edge_length,0),
                // Quaternion.identity);
                obj.GetComponent<piece>().index_x = i;
                obj.GetComponent<piece>().index_y = j;
                pieces.Add(obj);
                // Color color = obj.GetComponent<SpriteRenderer>().color;
                // obj.GetComponent<SpriteRenderer>().color = new Color(color.r,color.g,color.b,0);
                // obj.SetActive(false);
                // break;
            }
            // break;
        }
    }
    public void Reset()
    {
        foreach (var item in pieces)
        {
            Destroy(item);
        }
        init();
    }

    bool Check_Pos(Vector3 mouse_world_pos)
    {
        float x = mouse_world_pos.x;
        float y = mouse_world_pos.y;
        int int_x = (int)x;        
        Debug.Log(x + " " + y);
        int int_y = (int)y;
        Debug.Log(int_x + " " + int_y);
        bool ok_x = Check_Pos(int_x);
        if(ok_x==false)return false;
        bool ok_y = Check_Pos(int_y);
        if(ok_y==false)return false;
        return true;
    }
    bool Check_Pos(int pos)
    {
        if(pos < 0)pos *= -1;
        int index = (int)(pos / edge_length);
        int last = (int)(pos - index * edge_length);

        if(index > 5)return false;
        if(last < 10 || last > edge_length-10)return true;
        else return false;
    }
    
}
