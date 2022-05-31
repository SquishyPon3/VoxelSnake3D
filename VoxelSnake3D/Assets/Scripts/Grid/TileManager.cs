using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public TileGrid WorldTileGrid;
    public List<TileTransform> DynamicTiles;

    // Start is called before the first frame update
    void Start()
    {
        WorldTileGrid = new TileGrid(20, 20, 20);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            print(WorldTileGrid.Grid.Length);
        }
    }
}
