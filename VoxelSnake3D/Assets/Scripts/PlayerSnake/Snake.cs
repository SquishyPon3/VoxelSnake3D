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

        if (Input.GetKeyDown(KeyCode.F))
        {
            foreach (TileTransform _bodyTileTrans in BodyList)
            {
                TheTileManager.FindTilePosInDir(_bodyTileTrans.Position, Vector3.down);
            }
        }
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }
    public void MoveRight()
    {
        Move(Vector3.right);
    }
    public void MoveForward()
    {
        Move(Vector3.forward);
    }
    public void MoveBack()
    {
        Move(Vector3.back);
    }
    public void MoveUp()
    {
        Move(Vector3.up);
    }
    public void MoveDown()
    {
        Move(Vector3.down);
    }


    void Move(Vector3 moveDir)
    {        
        Vector3 _lastPos = Vector3.zero;

        for (int i = 0; i < BodyList.Count; i++)
        {
            Vector3 _oldPos = BodyList[i].Position;
            Vector3 _targetPos = _oldPos + moveDir;

            if (i == 0)
            {
                if (BodyList.Count > 1)
                {                    
                    if (TheTileManager.WorldTileGrid[_targetPos] != null)
                    {
                        if (TheTileManager.CheckTileContains(_targetPos, BodyList))
                            return;
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
        
        if (!CheckOnFloor())
        {
            fall();
        }
    }

    public void AddNewBody()
    {
        GameObject _newBody = Instantiate(SnakeBodyPiece, TailLastPos, Quaternion.identity, transform);
        BodyList.Add(_newBody.GetComponent<TileTransform>());
        print("add body");
    }

    public bool CheckOnFloor()
    {
        foreach(TileTransform bodyTrans in BodyList)
        {
            Vector3 _posBelow = bodyTrans.Position + Vector3.down;
            Tile _tileBelow = TheTileManager.WorldTileGrid[_posBelow];

            if (_tileBelow != null)
            {               
                if (_tileBelow.GetTileTranformList().Count > 0 
                    && !TheTileManager.CheckTileContains(_posBelow, BodyList)
                    && TheTileManager.CheckTileIsActive(_posBelow))
                {                    
                    return true;
                }                
            }

            // Trying to get this to work correctly. Unusual behaviour currently.
            //TheTileManager.FindTilePosInDir(bodyTrans.Position, Vector3.down);
        }

        print("not grounded");
        return false;
    }

    public bool CheckOnFloor(Vector3 pos)
    {
        Vector3 _posBelow = pos + Vector3.down;
        Tile _tileBelow = TheTileManager.WorldTileGrid[_posBelow];

        if (_tileBelow != null)
        {
            if (_tileBelow.GetTileTranformList().Count > 0
                    && !TheTileManager.CheckTileContains(_posBelow, BodyList)
                    && TheTileManager.CheckTileIsActive(_posBelow))
            {                
                return true;
            }
        }

        return false;
    }

    public void fall()
    {
        //int _nearestDist = 0;

        //foreach (TileTransform _snakeTileTrans in BodyList)
        //{
        //    float _dist =  _snakeTileTrans.Position.y;

        //    if (_snakeTileTrans.Position.y > TheTileManager.FindTilePosInDir(_snakeTileTrans.Position, Vector3.down).y)
        //    {
        //        _nearestDist = (int)(_dist + 0.5f);
        //    }
        //}

        //foreach (TileTransform _snakeTileTrans in BodyList)
        //{
        //    print(_nearestDist);
        //    _snakeTileTrans.ForceMove(Vector3.down, _nearestDist - 1);
        //}

        //foreach (TileTransform _snakeTileTrans in BodyList)
        //{
        //    // Few bugs where the snake can fall into itself because it does not
        //    // update whether or not it is on the floor until after having
        //    // moved everything at once.

        //    Vector3 _finalPos = TheTileManager.FindTilePosInDir(_snakeTileTrans.Position, Vector3.down);
        //    _finalPos = new Vector3(_finalPos.x, _finalPos.y + 1, _finalPos.z);

        //    _snakeTileTrans.ForceMoveDirect(_finalPos);          
        //}

        bool Grounded = false;
        bool WillDie = false;
        while (Grounded == false)
        {
            foreach (TileTransform snakeTileTrans in BodyList)
            {
                Vector3 _finalPos = snakeTileTrans.Position + Vector3.down;

                if (CheckOnFloor(_finalPos))
                {
                    Grounded = true;
                }
                if (_finalPos.y <= 0)
                {
                    TheTileManager.LoadTiles();
                    return;
                }
            }

            foreach (TileTransform snakeTileTrans in BodyList)
            {
                Vector3 _finalPos = snakeTileTrans.Position + Vector3.down;
                
                // I don't want to do this, I want it all to move at once
                // for future stuff like enemies. try to find a solution please!
                snakeTileTrans.ForceMoveDirect(_finalPos);
            }

            //TheTileManager.UpdatePositions();
        }

        TheTileManager.UpdatePositions();
    }
}
