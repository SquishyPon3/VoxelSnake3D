using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    public TileGrid WorldTileGrid;
    public List<TileTransform> TileTransforms;

    // Start is called before the first frame update
    void Start()
    {
        WorldTileGrid = new TileGrid(20, 20, 20);
        TileTransforms = new List<TileTransform>(FindObjectsOfType<TileTransform>());

        foreach (TileTransform tileTransform in TileTransforms)
        {
            Vector3 _tilePos = tileTransform.transform.position;

            if (WorldTileGrid[_tilePos] == null)
            {
                CreateTileAtIndexPos(_tilePos);
            }
                
            WorldTileGrid[_tilePos].AddTileTransform(tileTransform);
        }

        print(WorldTileGrid.X + WorldTileGrid.Y + WorldTileGrid.Z);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateTileAtIndexPos(Vector3 indexPos)
    {
        LinkedList<TileTransform> _tileTransList = new LinkedList<TileTransform>();
        WorldTileGrid[indexPos] = new Tile(_tileTransList);
    }

    public void RemoveTileAtIndexPos(Vector3 indexPos)
    {

    }

    public void UpdatePositions()
    {
        for (int i = 0; i < TileTransforms.Count; i++)
        {
            TileTransforms[i].Position = TileTransforms[i].TargetPosition;
        }
    }
}
