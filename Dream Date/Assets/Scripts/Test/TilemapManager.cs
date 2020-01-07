using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    public Transform target;

    public Tilemap wallTilemap;
    public Tilemap groundTilemap;

    public TileBase wallTile;

    Vector3Int preCellPos;

    [SerializeField]
    List<Vector3Int> savedTilePosList = new List<Vector3Int>();
    List<TileBase> savedTileList = new List<TileBase>();

    //private void Start()
    //{
    //    var obj = new GameObject("New Tilemap");
    //    obj.transform.SetParent(transform);
    //    wallTilemap = obj.AddComponent<Tilemap>();
    //    wallTilemap.tileAnchor = new Vector3(1, 1, 0);
    //    var render = obj.AddComponent<TilemapRenderer>();
    //    render.mode = TilemapRenderer.Mode.Individual;
    //    render.sortingLayerName = "Object";
    //    render.sortingOrder = 1;
    //}


    private void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3Int cellPos = wallTilemap.WorldToCell(pos);
        target.position = wallTilemap.CellToWorld(cellPos) - Vector3.down * 0.5f;

        if (Input.GetMouseButton(0) && preCellPos != cellPos)
        {
            preCellPos = cellPos;
            wallTilemap.SetTile(cellPos, wallTile);
        }

        if (Input.GetMouseButton(1))
        {
            preCellPos = Vector3Int.zero;
            wallTilemap.SetTile(cellPos, null);
        }

        if (Input.GetKeyDown(KeyCode.R))
            ClearTilemap();
        
        if (Input.GetKeyDown(KeyCode.S))
            SaveTilemap();
        if (Input.GetKeyDown(KeyCode.L))
            LoadTilemap();  
    }


    void ClearTilemap()
    {
        wallTilemap.ClearAllTiles();
    }
    void SaveTilemap()
    {
        savedTilePosList.Clear();
        savedTileList.Clear();

        var bounds = wallTilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                savedTilePosList.Add(new Vector3Int(x, y, 0));
                savedTileList.Add(wallTilemap.GetTile(new Vector3Int(x, y, 0)));
            }
    }

    void LoadTilemap()
    {
        ClearTilemap();
        wallTilemap.SetTiles(savedTilePosList.ToArray(),savedTileList.ToArray());
    }
}
