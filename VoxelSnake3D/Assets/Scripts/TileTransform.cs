using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileTransform : MonoBehaviour
{
    TileManager TheTileManager;
    public bool Dynamic = false;

    private Vector3 TilePosition;
    public Vector3 Position
    {
        get
        {
            return TilePosition;
        }
        set
        {
            RemoveFromCurrentTile();
            TilePosition = value;
            AddToNewTile();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TheTileManager = GameObject.Find("World").GetComponent<TileManager>();
        Position = transform.position;
        
        if (Dynamic)
            TheTileManager.DynamicTiles.Add(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {
        RemoveFromCurrentTile();
    }

    void RemoveFromCurrentTile()
    {
        if (TheTileManager.WorldTileGrid[TilePosition] != null)
        {
            TheTileManager.WorldTileGrid[TilePosition].RemoveTileTransform(this);
        }        
    }

    void AddToNewTile()
    {
        if (TheTileManager.WorldTileGrid[TilePosition] != null)
        {
            TheTileManager.WorldTileGrid[TilePosition].AddTileTransform(this);
        }        
    }
}
