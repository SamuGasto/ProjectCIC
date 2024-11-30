using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
public class GridManager : MonoBehaviour
{
    [Header("Grid Config")]
    public int gridSize = 20;
    [SerializeField] TileBase baseTile;
    [SerializeField] TileBase lifeTile;
    [SerializeField] Tilemap tilemap;

    public bool[,] currentGrid;
    private bool[,] nextGrid;
    public HashSet<Vector2Int> activeCells = new HashSet<Vector2Int>();

    public bool useLifeForms = false;


    private static readonly Vector2Int[] neighborOffsets = new Vector2Int[]
    {
        new Vector2Int(-1, -1), new Vector2Int(0, -1), new Vector2Int(1, -1),
        new Vector2Int(-1,  0),                       new Vector2Int(1,  0),
        new Vector2Int(-1,  1), new Vector2Int(0,  1), new Vector2Int(1,  1)
    };



    public void InitializeGrid()
    {
        currentGrid = new bool[gridSize, gridSize];
        nextGrid = new bool[gridSize, gridSize];

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                tilemap.SetTile(new Vector3Int(x, y), baseTile);
                currentGrid[x, y] = false;
            }
        }
    }

    public void UpdateGrid()
    {
        HashSet<Vector2Int> newActiveCells = new HashSet<Vector2Int>();

        foreach (var pos in activeCells)
        {
            int neighbors = CountAliveNeighbors(pos.x, pos.y);
            bool isAlive = currentGrid[pos.x, pos.y];

            if (isAlive && (neighbors == 2 || neighbors == 3))
            {
                nextGrid[pos.x, pos.y] = true;
                newActiveCells.Add(pos);
            }
            else if (!isAlive && neighbors == 3)
            {
                nextGrid[pos.x, pos.y] = true;
                newActiveCells.Add(pos);
            }
            else
            {
                nextGrid[pos.x, pos.y] = false;
            }

            foreach (var neighbor in GetNeighbors(pos))
            {
                newActiveCells.Add(neighbor);
            }
        }
        // Actualiza visualmente el tilemap y sincroniza las grillas
        foreach (var pos in newActiveCells)
        {
            tilemap.SetTile(new Vector3Int(pos.x, pos.y), nextGrid[pos.x, pos.y] ? lifeTile : baseTile);
        }

        // Copia el estado de nextGrid a currentGrid y limpia nextGrid
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                currentGrid[x, y] = nextGrid[x, y];
                nextGrid[x, y] = false;
            }
        }

        activeCells = newActiveCells;
    }

    public IEnumerable<Vector2Int> GetNeighbors(Vector2Int pos)
    {
        foreach (var offset in neighborOffsets)
        {
            Vector2Int neighbor = pos + offset;

            if (neighbor.x >= 0 && neighbor.x < gridSize &&
                neighbor.y >= 0 && neighbor.y < gridSize)
            {
                yield return neighbor;
            }
        }
    }


    private int CountAliveNeighbors(int x, int y, int range = 1)
    {
        int aliveCount = 0;
        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = -range; dy <= range; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int nx = x + dx, ny = y + dy;
                if (nx >= 0 && ny >= 0 && nx < gridSize && ny < gridSize && currentGrid[nx, ny])
                {
                    aliveCount++;
                }
            }
        }

        return aliveCount;
    }

    public void Randomize()
    {
        HashSet<Vector2Int> newActiveCells = new HashSet<Vector2Int>();
        InitializeGrid();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // Decidimos aleatoriamente si colocar algo en esta celda
                if (UnityEngine.Random.value > 0.5f)
                {
                    currentGrid[x, y] = true;
                    newActiveCells.Add(new Vector2Int(x, y));
                }

            }
        }
        activeCells = newActiveCells;
        UpdateGrid();
    }
}
