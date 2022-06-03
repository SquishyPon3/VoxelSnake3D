using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    TileManager TheTileManager;
    public List<TileTransform> BodyList;
    public Vector3 TailLastPos;
    public GameObject SnakeBodyPiece;

    // Start is called before the first frame update
    void Start()
    {
        TheTileManager = GameObject.Find("World").GetComponent<TileManager>();

        foreach (Transform bodyTrans in transform)
        {
            BodyList.Add(bodyTrans.GetComponent<TileTransform>());
        }

        TailLastPos = BodyList[BodyList.Count - 1].Position;
    }

    // Update is called once per frame
    void Update()
    {
        GetMoveInput();
    }

    void GetMoveInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
            Move(Vector3.forward);

        if (Input.GetKeyDown(KeyCode.S))
            Move(Vector3.back);

        if (Input.GetKeyDown(KeyCode.A))
            Move(Vector3.left);

        if (Input.GetKeyDown(KeyCode.D))
            Move(Vector3.right);

        if (Input.GetKeyDown(KeyCode.Space))
            Move(Vector3.up);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            Move(Vector3.down);

        if (Input.GetKeyDown(KeyCode.Q))
            AddNewBody();
    }

    void Move(Vector3 moveDir)
    {
        Vector3 _lastPos = Vector3.zero;

        for (int i = 0; i < BodyList.Count; i++)
        {
            Vector3 _oldPos = BodyList[i].Position;

            if (i == 0)
            {
                if (BodyList.Count > 1)
                {                    
                    if (TheTileManager.WorldTileGrid[_oldPos + moveDir] != null)
                    {
                        print("here");
                        for (int j = 0; j < BodyList.Count; j++)
                        {
                            if (j < BodyList.Count - 1)
                            {
                                if (TheTileManager.WorldTileGrid[_oldPos + moveDir]
                                    .GetTileTranformList().Contains(BodyList[j]))
                                    return;
                            }
                        }
                    }                    
                }
                if (!BodyList[i].TryMove(moveDir))
                {                   
                    return;
                }
            }
            else
            {
                // Move in direction of previous body piece's old position.

                BodyList[i].ForceMoveDirect(_lastPos);

                // If the tail piece, set the recorded tail position to the position before moving

                if (i == BodyList.Count - 1)
                {
                    TailLastPos = _oldPos;
                    print(BodyList[i].name);
                }                    
            }

            _lastPos = _oldPos;
        }

        TheTileManager.UpdatePositions();
    }

    public void AddNewBody()
    {
        GameObject _newBody = Instantiate(SnakeBodyPiece, TailLastPos, Quaternion.identity, transform);
        BodyList.Add(_newBody.GetComponent<TileTransform>());
        print("add body");
    }
}
