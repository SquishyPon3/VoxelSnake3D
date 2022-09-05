using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    private LinkedList<TileTransform> TileTransforms;

    public Tile(LinkedList<TileTransform> tileTransforms)
    {
        TileTransforms = tileTransforms;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddTileTransform(TileTransform tileTransform)
    {
        TileTransforms.AddLast(tileTransform);
    }

    public void RemoveTileTransform(TileTransform tileTransform)
    {
        TileTransforms.Remove(tileTransform);
    }

    public TileTransform GetTileTranformAtIndex(int index)
    {
        TileTransform _tileTrans;

        if (TileTransforms.First.Value == null)
            return null;

        _tileTrans = TileTransforms.First.Value;

        for (int i = 0; i < index; i++)
        {
            if (TileTransforms.Find(_tileTrans).Next != null)
            {
                _tileTrans = TileTransforms.Find(_tileTrans).Next.Value;
            }
        }

        return _tileTrans;
    }

    public LinkedList<TileTransform> GetTileTranformList()
    {
        return TileTransforms;
    }
}
