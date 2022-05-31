using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTransform : MonoBehaviour
{
    TileManager TheTileManager;
    public bool Dynamic = false;
    public int Prioity = 100;

    [SerializeField]
    private Vector3 TileTransPosition;
    public Vector3 Position
    {
        get
        {
            return TileTransPosition;
        }
        set
        {
            RemoveFromCurrentTile();
            TileTransPosition = value;
            transform.position = TileTransPosition;
            AddToNewTile();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TheTileManager = GameObject.Find("World").GetComponent<TileManager>();
        
        TileTransPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //print(this.name 
            //    + ": " 
            //    + TheTileManager.WorldTileGrid[TileTransPosition]
            //        .GetTileTranformList().Find(this).Value.name 
            //    + " " 
            //    + TheTileManager.WorldTileGrid[TileTransPosition]
            //        .GetTileTranformList().Count);
            print("Start ");
            foreach (TileTransform tileTrans in TheTileManager.WorldTileGrid[TileTransPosition].GetTileTranformList())
            {
                print(tileTrans.name);
            }
            print(" End. ");
        }

        if (Dynamic)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(Vector3.forward);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                Move(Vector3.back);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Move(Vector3.left);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Move(Vector3.right);
            }
        }        
    }

    private void OnDestroy()
    {
        RemoveFromCurrentTile();
    }

    void RemoveFromCurrentTile()
    {
        if (TheTileManager.WorldTileGrid[TileTransPosition] != null)
        {
            TheTileManager.WorldTileGrid[TileTransPosition].RemoveTileTransform(this);
            
            if (TheTileManager.WorldTileGrid[TileTransPosition].GetTileTranformList().Count - 1 <= 0)
            {
                TheTileManager.WorldTileGrid[TileTransPosition] = null;
            }
        }        
    }

    void AddToNewTile()
    {
        if (TheTileManager.WorldTileGrid[TileTransPosition] == null)
        {
            TheTileManager.CreateTileAtIndexPos(TileTransPosition);
        }

        TheTileManager.WorldTileGrid[TileTransPosition].AddTileTransform(this);
    }

    void Move(Vector3 dir, int distance = 1)
    {
        Vector3 _targetPos;

        _targetPos = Position + new Vector3(dir.x * distance, dir.y * distance, dir.z * distance);

        if (TheTileManager.WorldTileGrid[_targetPos] != null)
        {
            foreach (TileTransform tileTransform in TheTileManager.WorldTileGrid[_targetPos].GetTileTranformList())
            {
                if (tileTransform.Prioity >= Prioity)
                {
                    return;
                }
            }

            Position = _targetPos;
            return;
        }

        Position = _targetPos;
    }
}
