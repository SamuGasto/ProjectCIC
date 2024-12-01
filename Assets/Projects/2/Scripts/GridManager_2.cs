using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField] float predatorPreyDensity = 0.5f;
    [SerializeField] int numberOfPredatorsDiesWitoutFood = 3;
    public int[,] frontGrid;
    public int[,] nextFrontGrid;
    public bool[,] backGrid;
    private bool[,] backNextGrid;
    [System.Serializable]
    public class Predator
    {
        public Vector2Int position;
        public Vector2Int lastPosition;
        public int timeWithoutFood;
    }
    [System.Serializable]
    public class Prey
    {
        public Vector2Int position;
        public Vector2Int lastPosition;
    }
    [SerializeField]
    public List<Predator> predatorCells; // Position and time life predator
    public List<Prey> preyCells;

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
        predatorCells = new List<Predator>();
        preyCells = new List<Prey>();

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (UnityEngine.Random.value < predatorPreyDensity) // Celda ocupada?
                {
                    if (UnityEngine.Random.value > 0.5f) // Predador o Presa?
                    {
                        tilemap_front.SetTile(new Vector3Int(x, y), predatorTile);
                        frontGrid[x, y] = 2;

                        Predator predator = new Predator()
                        {
                            position = new Vector2Int(x, y),
                            lastPosition = new Vector2Int(x, y),
                            timeWithoutFood = 0
                        };
                        predatorCells.Add(predator);
                    }
                    else
                    {
                        tilemap_front.SetTile(new Vector3Int(x, y), preyTile);
                        frontGrid[x, y] = 1;

                        Prey prey = new Prey()
                        {
                            position = new Vector2Int(x, y),
                            lastPosition = new Vector2Int(x, y)
                        };
                        preyCells.Add(prey);
                    }

                }

            }
        }
    }

    public void UpdateGrid()
    {
        ClearTileMap();
        foreach (var pred in predatorCells)
        {
            pred.timeWithoutFood++;
        }

        Tuple<List<Predator>, List<Prey>> cellsUpdated = ChangeOfState();
        Tuple<List<Predator>, List<Prey>> cellsMoved = Movement(cellsUpdated);

        UpdateFrontGrid(cellsMoved);
        Debug.Log("---------------------");
    }

    public void ClearTileMap()
    {
        foreach (var pred in predatorCells)
        {
            tilemap_front.SetTile(new Vector3Int(pred.position.x, pred.position.y), null);
        }

        foreach (var prey in preyCells)
        {
            tilemap_front.SetTile(new Vector3Int(prey.position.x, prey.position.y), null);
        }
    }

    public void UpdateFrontGrid(Tuple<List<Predator>, List<Prey>> newCells)
    {
        foreach (var predator in newCells.Item1)
        {
            tilemap_front.SetTile(new Vector3Int(predator.position.x, predator.position.y), predatorTile);
        }

        foreach (var prey in newCells.Item2)
        {
            tilemap_front.SetTile(new Vector3Int(prey.position.x, prey.position.y), preyTile);
        }

        predatorCells = newCells.Item1;
        preyCells = newCells.Item2;
    }

    public Tuple<List<Predator>, List<Prey>> ChangeOfState()
    {
        List<Predator> PredatorsAlive = new List<Predator>();
        List<Prey> PreysAlive = new List<Prey>();
        // Si las presas encuentran predadores, mueren
        foreach (var prey in preyCells)
        {
            Tuple<List<Predator>, List<Prey>, HashSet<Vector2Int>> neighbors = NearestNeighbor(prey.position.x, prey.position.y);
            if (neighbors.Item1 != null && neighbors.Item1.Count > 0)
            {
                Predator predatorSelected = neighbors.Item1[0];

                if (neighbors.Item1.Count > 1)
                {
                    predatorSelected = neighbors.Item1[UnityEngine.Random.Range(0, neighbors.Item1.Count - 1)];
                }

                if (predatorSelected != null)
                {
                    Predator predatorInCell = predatorCells.Find(pred => Vector2Int.Equals(pred.position, predatorSelected.position));

                    if (predatorInCell != null)
                    {
                        if (predatorInCell.timeWithoutFood - 1 <= 0)
                        {
                            predatorInCell.timeWithoutFood = 0;
                        }
                        else
                        {
                            predatorInCell.timeWithoutFood--;
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("Predator selected is null");
                }

            }
            else
            {
                PreysAlive.Add(prey);
            }
        }

        // Eliminar predadores que mueren
        foreach (var predator in predatorCells)
        {
            if (predator.timeWithoutFood < numberOfPredatorsDiesWitoutFood)
            {
                PredatorsAlive.Add(predator);
            }
        }

        Tuple<List<Predator>, List<Prey>> newCells = Tuple.Create(PredatorsAlive, PreysAlive);

        return newCells;

    }

    public Tuple<List<Predator>, List<Prey>> Movement(Tuple<List<Predator>, List<Prey>> newCells)
    {
        foreach (var pred in newCells.Item1)
        {
            HashSet<Vector2Int> freeSpaces = NearestNeighbor(pred.position.x, pred.position.y).Item3;

            foreach (var spaces in freeSpaces)
            { // Verificar si hay espacio libre
                if (spaces != pred.lastPosition)
                { // Si hay espacio libre, y no es la celda anterior, se mueve
                    pred.lastPosition = pred.position;
                    pred.position = spaces;
                    break;
                }
            }

        }

        foreach (var prey in newCells.Item2)
        {
            HashSet<Vector2Int> freeSpaces = NearestNeighbor(prey.position.x, prey.position.y).Item3;

            foreach (var spaces in freeSpaces)
            { // Verificar si hay espacio libre
                if (spaces != prey.lastPosition)
                { // Si hay espacio libre, y no es la celda anterior, se mueve
                    prey.lastPosition = prey.position;
                    prey.position = spaces;
                    break;
                }
            }
        }

        return newCells;
    }

    public Tuple<List<Predator>, List<Prey>, HashSet<Vector2Int>> NearestNeighbor(int x, int y)
    /// Devuelve la cantidad de predadores y presas cercanas a la celda x,y de la siguiente forma: (pos_predadores, cantidad_predadores, pos_presas, cantidad_presas)
    {
        List<Predator> predatorNeighbors = new List<Predator>();
        List<Prey> preyNeighbors = new List<Prey>();
        HashSet<Vector2Int> freeSpaces = new HashSet<Vector2Int>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // No revisa su celda actual

                int nx = x + dx, ny = y + dy;
                if (nx >= 0 && ny >= 0 && nx < gridSize && ny < gridSize && frontGrid[nx, ny] == 1)
                {
                    Prey prey = preyCells.Find(p => Vector2Int.Equals(p.position, new Vector2Int(nx, ny)));
                    preyNeighbors.Add(prey);
                }
                else if (nx >= 0 && ny >= 0 && nx < gridSize && ny < gridSize && frontGrid[nx, ny] == 2)
                {

                    Predator predator = predatorCells.Find(p => Vector2Int.Equals(p.position, new Vector2Int(nx, ny)));
                    predatorNeighbors.Add(predator);
                }
                else if (nx >= 0 && ny >= 0 && nx < gridSize && ny < gridSize)
                {
                    freeSpaces.Add(new Vector2Int(nx, ny));
                }
            }
        }

        Tuple<List<Predator>, List<Prey>, HashSet<Vector2Int>> aliveNeighbors = Tuple.Create(predatorNeighbors, preyNeighbors, freeSpaces);
        return aliveNeighbors;
    }
}
