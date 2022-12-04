using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private Transform Player;

    int Unitsize = 16;
    int Tile_x = 0;
    int Tile_y = 0;

    void Start()
    {
        Player = Managers.StageManager.Player.GetComponent<Transform>();
    }
   
    void Update()
    {
        switch (Managers.StageManager.stage)
        {
            case Define.Stage.STAGE2:
                Unitsize = 16;
                break;
            case Define.Stage.STAGE3:
                Unitsize = 30;
                break;
            case Define.Stage.STAGE4:
                Unitsize = 25;
                break;
        }

        MoveTile();
    }
    void MoveTile()
    {
        if (Player.transform.position.x > Tile_x + Unitsize)
        {
            Tile_x += Unitsize * 2;
            transform.position = new Vector2(Tile_x, Tile_y);
            print("x : " + Tile_x);
        }
        if (Player.transform.position.x < Tile_x - Unitsize)
        {
            Tile_x -= Unitsize * 2;
            transform.position = new Vector2(Tile_x, Tile_y);
            print("x : " + Tile_x);
        }
        if (Player.transform.position.y > Tile_y + Unitsize)
        {
            Tile_y += Unitsize * 2;
            transform.position = new Vector2(Tile_x, Tile_y);
            print("y : " + Tile_y);
        }
        if (Player.transform.position.y < Tile_y - Unitsize)
        {
            Tile_y -= Unitsize * 2;
            transform.position = new Vector2(Tile_x, Tile_y);
            print("y : " + Tile_y);
        }
    }
}
