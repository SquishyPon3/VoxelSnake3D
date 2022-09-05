using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TileManager : MonoBehaviour
{
    [SerializeField]
    public TileGrid WorldTileGrid;
    public List<TileTransform> TileTransforms;
    [SerializeField]
    public List<TileTransSave> TileTransSaves;

    [SerializeField]
    private Vector3 WorldGridSize;

    [SerializeField]
    private GUIStyle HandleStyle;
    [SerializeField]
    private GUIStyle HandleStyleImportant;

    [SerializeField]
    bool ViewDebug = false;
    [SerializeField]
    bool DebugFillOccupied = false;

    // Start is called before the first frame update
    void Start()
    {
        WorldTileGrid = new TileGrid(WorldGridSize);
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            UpdatePositions();
        }


        if (Input.GetKeyDown(KeyCode.Y))
        {
            SaveTiles();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            LoadTiles();
        }
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
        SaveTiles();

        for (int i = 0; i < TileTransforms.Count; i++)
        {
            TileTransforms[i].Position = TileTransforms[i].TargetPosition;
        }
    }

    public bool HasPriority(Vector3 targetPos, TileTransform tileTransTryMove)
    {
        if (WorldTileGrid[targetPos] != null)
        {
            foreach (TileTransform tileTransform in WorldTileGrid[targetPos].GetTileTranformList())
            {
                if (tileTransform.Prioity >= tileTransTryMove.Prioity)
                {
                    return false;
                }
            }
        }

        return true;
    }

    public bool CheckTileContains(Vector3 tilePos, List<TileTransform> tileTransList)
    {
        foreach (TileTransform _tileTrans in tileTransList)
        {
            if (WorldTileGrid[tilePos].GetTileTranformList().Contains(_tileTrans))
            {
                return true;
            }
        }

        return false;
    }

    public bool CheckTileContains(Vector3 tilePos, TileTransform tileTrans)
    {
        if (WorldTileGrid[tilePos].GetTileTranformList().Contains(tileTrans))
        {
            return true;
        }

        return false;
    }

    public bool CheckTileIsActive(Vector3 tilePos)
    {
        bool containsActive = false;

        foreach (TileTransform _tileTrans in WorldTileGrid[tilePos].GetTileTranformList())
        {
            if (_tileTrans.gameObject.activeSelf == true)
            {
                containsActive = true;
            }
        }

        return containsActive;
    }

    bool DrawnCubes = false;
    [SerializeField]
    string SearchTerm = "";
    [SerializeField]
    bool Search = false;
    private string CurrentSearchTerm = "";
    private void OnDrawGizmos()
    {
        if (Search)
        {
            CurrentSearchTerm = SearchTerm;
            Search = false;
        }

        if (ViewDebug)
        {
            Vector3 _offset = new Vector3(0.5f, 0.5f, 0.5f);

            for (int x = 0; x < WorldGridSize.x; x++)
            {
                for (int y = 0; y < WorldGridSize.y; y++)
                {
                    for (int z = 0; z < WorldGridSize.z; z++)
                    {
                        Vector3 _tilePos = new Vector3(x, y, z);

                        if (Application.isPlaying == true)
                        {
                            // Putting .count tracker in temporarily because not removing empty Tiles until snake is fixed.

                            if (WorldTileGrid[_tilePos] != null && WorldTileGrid[_tilePos].GetTileTranformList().Count > 0)
                            {
                                Vector3 _add = new Vector3(0.1f, -0.1f, 0f);

                                for (int i = 0; i < WorldTileGrid[_tilePos].GetTileTranformList().Count; i++)
                                {
                                    if (WorldTileGrid[_tilePos].GetTileTranformAtIndex(i).name.Contains(CurrentSearchTerm) && CurrentSearchTerm != "")
                                    {
                                        Handles.Label(_tilePos + _offset + _add, i + " : "
                                            + WorldTileGrid[_tilePos].GetTileTranformAtIndex(i).name,
                                            HandleStyleImportant);
                                    }
                                    else
                                    {
                                        Handles.Label(_tilePos + _offset + _add, i + " : "
                                            + WorldTileGrid[_tilePos].GetTileTranformAtIndex(i).name,
                                            HandleStyle);
                                    }

                                    _add += _add;
                                }

                                _tilePos += _offset;

                                if (!DebugFillOccupied)
                                {
                                    Gizmos.color = Color.red;
                                    Gizmos.DrawWireCube(_tilePos, Vector3.one * 1.1f);
                                }
                                else
                                {
                                    Gizmos.color = new Color(Color.red.r, Color.red.g, Color.red.b, 0.5f);
                                    Gizmos.DrawCube(_tilePos, Vector3.one * 1.025f);
                                }
                            }
                            else
                            {
                                _tilePos += _offset;

                                Gizmos.color = new Color(1f, 1f, 1f, 0.15f);
                                Gizmos.DrawWireCube(_tilePos, Vector3.one * .8f);
                            }
                        }
                        else
                        {
                            _tilePos += new Vector3(.5f, .5f, .5f);

                            Gizmos.color = Gizmos.color = new Color(1f, 1f, 1f, 0.2f); ;
                            Gizmos.DrawWireCube(_tilePos, Vector3.one * .8f);
                        }
                    }
                }
            }
        }        
    }    

    public Vector3 FindTilePosInDir(Vector3 startPos, Vector3 dir)
    {
        Vector3 _finalDir = dir;

        //while (WorldTileGrid[startPos + _finalDir] == null
        //    || WorldTileGrid[startPos + _finalDir].GetTileTranformList().Count == 0)
        //{
        //    _finalDir += dir;
        //}

        while (Vector3.Scale(startPos + _finalDir, dir).magnitude < Vector3.Scale(WorldGridSize, dir).magnitude)
        {
            if (WorldTileGrid[startPos + _finalDir] == null 
                    || WorldTileGrid[startPos + _finalDir].GetTileTranformList().Count == 0)
            {
                _finalDir += dir;
                continue;
            }           
            else
            {
                bool tileClear = false;

                foreach (TileTransform tileTrans in WorldTileGrid[startPos + _finalDir].GetTileTranformList())
                {
                    if(LayerMask.LayerToName(tileTrans.gameObject.layer) == "Snake")
                    {
                        tileClear = true;
                    }
                }

                if (tileClear)
                {
                    _finalDir += dir;
                    continue;
                }    
                else
                {
                    break;
                }
            }            
        }

        print("Start: " + startPos + " end: " + (startPos + _finalDir) + " distance: " + Vector3.Distance(startPos, _finalDir));
        return startPos + _finalDir;
    }

    void SaveTiles()
    {
        TileTransSave _newTileSave = new TileTransSave();

        TileTransform[] _tileTransforms = new TileTransform[TileTransforms.Count];
        Vector3[] Positions = new Vector3[TileTransforms.Count];
        Vector3[] TargetPositions = new Vector3[TileTransforms.Count];
        bool[] ActiveStates = new bool[TileTransforms.Count];

        for (int i = 0; i < TileTransforms.Count; i++)
        {
            //_tileTransforms[i] = TileTransforms[i];
            ActiveStates[i] = TileTransforms[i].transform.gameObject.activeSelf;
            Positions[i] = TileTransforms[i].Position;
            TargetPositions[i] = TileTransforms[i].TargetPosition;            
        }

        //_newTileSave.TileTransforms = _tileTransforms;
        _newTileSave.ActiveStates = ActiveStates;
        _newTileSave.Positions = Positions;
        _newTileSave.TargetPositions = TargetPositions;        

        TileTransSaves.Add(_newTileSave);
    }    

    void LoadTiles()
    {
        // One way to possibly delete new tileTansforms such as new player tail pieces
        // could be to remove all game objects from the TileTransforms list which exceed
        // the number of TileTransforms in the previous saves' list.

        // Also need to fix bug with falling not saving correctly, probably because of the nature
        // of falling using ForceMove and the saving system saving before the results of a move.

        for (int i = 0; i < TileTransforms.Count; i++)
        {
            TileTransforms[i].transform.gameObject.SetActive(TileTransSaves[TileTransSaves.Count - 1].ActiveStates[i]);
            TileTransforms[i].Position = TileTransSaves[TileTransSaves.Count - 1].Positions[i];
            TileTransforms[i].TargetPosition = TileTransSaves[TileTransSaves.Count - 1].TargetPositions[i];
        }

        // The reason pickups flash back to inactive is because I never reset their "has been picked up bool."

        TileTransSaves.RemoveAt(TileTransSaves.Count - 1);
    }
}
