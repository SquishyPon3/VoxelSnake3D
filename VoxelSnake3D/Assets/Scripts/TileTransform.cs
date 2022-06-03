using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTransform : MonoBehaviour
{
    public TileManager TheTileManager;
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

    public Vector3 TargetPosition;

    // Start is called before the first frame update
    void Start()
    {
        TheTileManager = GameObject.Find("World").GetComponent<TileManager>();
        
        TileTransPosition = transform.position;
        TargetPosition = Position;
        Position = transform.position;
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
            //if (Input.GetKeyDown(KeyCode.W))
            //{
            //    Move(Vector3.forward);
            //}
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    Move(Vector3.back);
            //}
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    Move(Vector3.left);
            //}
            //if (Input.GetKeyDown(KeyCode.D))
            //{
            //    Move(Vector3.right);
            //}
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    Move(Vector3.up);
            //}
            //if (Input.GetKeyDown(KeyCode.LeftControl))
            //{
            //    Move(Vector3.down);
            //}
        }        
    }

    private void OnDestroy()
    {
        RemoveFromCurrentTile();
    }

    public void DestroyTileTrans()
    {
        RemoveFromCurrentTile();
        gameObject.SetActive(false);
    }

    void RemoveFromCurrentTile()
    {
        if (TheTileManager.WorldTileGrid[TileTransPosition] != null)
        {
            TheTileManager.WorldTileGrid[TileTransPosition].RemoveTileTransform(this);
            
            if (TheTileManager.WorldTileGrid[TileTransPosition].GetTileTranformList().Count - 1 <= 0)
            {
                // This is what's breaking shit with moving because
                // it probably sets the next pos as null as the snake body part moves into it.

                //TheTileManager.WorldTileGrid[TileTransPosition] = null;
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

        if (!TheTileManager.TileTransforms.Contains(this))
            TheTileManager.TileTransforms.Add(this);
    }

    public bool TryMove(Vector3 dir, int distance = 1)
    {
        // Move all this shit to the tile manager for validation instead
        // eventually because then I can make sure stuff moves when it is supposed to.

        Vector3 _targetPos;

        _targetPos = Position + new Vector3(dir.x * distance, dir.y * distance, dir.z * distance);

        if (TheTileManager.WorldTileGrid[_targetPos] != null)
        {
            if (!TheTileManager.HasPriority(_targetPos, this))
            {
                return false;
            }

            TargetPosition = _targetPos;
            return true;
        }

        TargetPosition = _targetPos;
        return true;
    }

    public void ForceMove(Vector3 dir, int distance = 1)
    {
        Vector3 _targetPos;

        _targetPos = Position + new Vector3(dir.x * distance, dir.y * distance, dir.z * distance);

        TargetPosition = _targetPos;
    }

    public void ForceMoveDirect(Vector3 pos)
    {       
        TargetPosition = pos;
    }


}
