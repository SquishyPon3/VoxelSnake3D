using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGrid : MonoBehaviour
{
    public Tile[,,] Grid;
    private int GridX, GridY, GridZ;

    public TileGrid(Vector3 value)
    {
        GridX = (int)(value.x * 0.5f);
        GridY = (int)(value.y * 0.5f);
        GridZ = (int)(value.z * 0.5f);

        Grid = new Tile[GridX, GridY, GridZ];
    }

    public TileGrid(int valueX, int ValueY, int ValueZ)
    {
        Grid = new Tile[valueX, ValueY, ValueZ];
        GridX = valueX;
        GridY = ValueY;
        GridZ = ValueZ;
    }

    public int X
    {
        get
        {
            return GridX;
        }
    }

    public int Y
    {
        get
        {
            return GridY;
        }
    }

    public int Z
    {
        get
        {
            return GridZ;
        }
    }

    public Tile this[int indexX, int indexY, int indexZ]
    {
        get
        {
            // Returns tile at Grid point [x,y,z]

            return Grid[indexX, indexY, indexZ];
        }
        set
        {
            Grid[indexX, indexY, indexZ] = value;
        }
    }

    public Tile this[Vector3 index]
    {
        get
        {
            int indexX = (int)(index.x * 0.5f);
            int indexY = (int)(index.x * 0.5f);
            int indexZ = (int)(index.x * 0.5f);

            // Returns tile at Grid point [x,y,z]            

            return Grid[indexX, indexY, indexZ];
        }
        set
        {
            int indexX = (int)(index.x * 0.5f);
            int indexY = (int)(index.x * 0.5f);
            int indexZ = (int)(index.x * 0.5f);

            Grid[indexX, indexY, indexZ] = value;
        }
    }

    public void AddTileTransform(int indexX, int indexY, int indexZ)
    {
        if (Grid[indexX, indexY, indexZ] == null)
            return;

        Tile TileAtIndex = Grid[indexX, indexY, indexZ];    
        
    }

    public void RemoveTileTransformFromTile(Vector3 tileIndex, TileTransform tileTrans)
    {
        this[tileIndex].RemoveTileTransform(tileTrans);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
