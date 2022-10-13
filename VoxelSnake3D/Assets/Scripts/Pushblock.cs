using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushblock : MonoBehaviour
{
    TileTransform TileTrans;

    // Start is called before the first frame update
    void Start()
    {
        TileTrans = GetComponent<TileTransform>();
    }

    // Update is called once per frame
    void Update()
    {
    }   

    public bool TryPush (Vector3 pusherPosition)
    {
        Vector3 _targetPos = TileTrans.TargetPosition + (TileTrans.Position - pusherPosition);

        if (TileTrans.TryMove(TileTrans.Position - pusherPosition))
            return true;
        else
            return false;
    }
}
