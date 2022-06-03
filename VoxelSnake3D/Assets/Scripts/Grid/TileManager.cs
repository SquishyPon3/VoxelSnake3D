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
                            if (WorldTileGrid[_tilePos] != null)
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
}
