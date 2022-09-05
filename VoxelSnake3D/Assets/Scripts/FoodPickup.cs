using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodPickup : MonoBehaviour
{
    TileTransform TileTrans;
    [SerializeField]
    bool Eaten = false;

    // Start is called before the first frame update
    void Start()
    {
        TileTrans = GetComponent<TileTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Eaten == false)
        {
            if (TileTrans.TheTileManager.WorldTileGrid[TileTrans.Position].GetTileTranformList().Last.Value != this)
            {
                if (TileTrans.TheTileManager.WorldTileGrid[TileTrans.Position].GetTileTranformList().Last.Value.GetComponentInParent<Snake>() != null)
                {
                    TileTrans.TheTileManager.WorldTileGrid[TileTrans.Position].GetTileTranformList().Last.Value.GetComponentInParent<Snake>().AddNewBody();
                    Eaten = true;
                }
            }
        }
        else
        {
            TileTrans.DisableTileTrans();
        }
    }
}
