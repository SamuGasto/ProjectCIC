using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager_2 : MonoBehaviour
{
    [Header("Grid Config")]
    public int gridSize = 20;
    [SerializeField] TileBase baseTile;
    [SerializeField] TileBase preyTile;
    [SerializeField] TileBase predatorTile;
    [SerializeField] Tilemap tilemap_back;
    [SerializeField] Tilemap tilemap_front;

    public int[,] frontGrid;
    public int[,] nextFrontGrid;
    public bool[,] backGrid;
    private bool[,] backNextGrid;
    public HashSet<Vector2Int> activeCells = new HashSet<Vector2Int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitializeGrid()
    {
        backGrid = new bool[gridSize, gridSize];
        backNextGrid = new bool[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                tilemap_back.SetTile(new Vector3Int(x, y), baseTile);
                backGrid[x, y] = false;
            }
        }
    }

    public void InitialPlaceEntity()
    {
        frontGrid = new int[gridSize, gridSize];
        nextFrontGrid = new int[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                tilemap_front.SetTile(new Vector3Int(x, y), baseTile);
                frontGrid[x, y] = 0;
            }
        }
    }

    public void UpdateGrid()
    {
        HashSet<Vector2Int> newActiveCells = new HashSet<Vector2Int>();

        //PROCESO

        activeCells = newActiveCells;
    }
}
