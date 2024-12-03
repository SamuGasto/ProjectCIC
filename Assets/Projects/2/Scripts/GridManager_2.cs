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
    public bool[,] backGrid;
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

    public void AddPrey(Vector2Int prey_position)
    {
        Prey prey = new Prey() { position = prey_position, lastPosition = prey_position };
        frontGrid[prey.position.x, prey.position.y] = 1;
        preyCells.Add(prey);
    }

    public void RemovePrey(Vector2Int prey_position)
    {
        Prey prey = preyCells.Find(p => Vector2Int.Equals(p.position, prey_position));
        frontGrid[prey.position.x, prey.position.y] = 0;
        preyCells.Remove(prey);
    }

    public void AddPredator(Vector2Int predator_position)
    {
        Predator predator = new Predator() { position = predator_position, lastPosition = predator_position, timeWithoutFood = 0 };
        frontGrid[predator.position.x, predator.position.y] = 2;
        predatorCells.Add(predator);
    }
    public void RemovePredator(Vector2Int predator_position)
    {
        Predator predator = predatorCells.Find(p => Vector2Int.Equals(p.position, predator_position));
        frontGrid[predator.position.x, predator.position.y] = 0;
        predatorCells.Remove(predator);
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
        Debug.Log("---------------------");
        List<Predator> predatorsAlive = new List<Predator>();
        List<Prey> preysAlive = new List<Prey>();

        ClearTileMap();
        Debug.Log("[UG] Limpieza Tilemap");

        foreach (var pred in predatorCells)
        {
            pred.timeWithoutFood++;
        }
        Debug.Log("[UG] Se alimentaron los predadores");

        ChangeOfState(predatorsAlive, preysAlive);
        Debug.Log("[UG] Se actualizaron los estados de los predadores y presas");
        Movement(predatorsAlive, preysAlive);
        Debug.Log("[UG] Se movieron los predadores y presas");

        UpdateFrontGrid(predatorsAlive, preysAlive);
        Debug.Log("[UG] Se actualizaron los grids");
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

    public void UpdateFrontGrid(List<Predator> predatorsAlive, List<Prey> preysAlive)
    {
        int[,] newGrid = new int[gridSize, gridSize];
        foreach (var predator in predatorsAlive)
        {
            tilemap_front.SetTile(new Vector3Int(predator.position.x, predator.position.y), predatorTile);
            newGrid[predator.position.x, predator.position.y] = 2;
        }

        foreach (var prey in preysAlive)
        {
            tilemap_front.SetTile(new Vector3Int(prey.position.x, prey.position.y), preyTile);
            newGrid[prey.position.x, prey.position.y] = 1;
        }

        predatorCells = predatorsAlive;
        preyCells = preysAlive;
        frontGrid = newGrid;
    }

    public void ChangeOfState(List<Predator> predatorsAlive, List<Prey> preysAlive)
    {
        // Si las presas encuentran predadores, mueren
        foreach (var prey in preyCells)
        {
            Tuple<List<Predator>, List<Prey>> neighbors = NearestNeighbor(prey.position.x, prey.position.y);
            if (neighbors.Item1.Count > 0)
            {
                Predator predatorSelected = neighbors.Item1[0];

                if (neighbors.Item1.Count > 1)
                {
                    predatorSelected = neighbors.Item1[UnityEngine.Random.Range(0, neighbors.Item1.Count - 1)];
                }

                frontGrid[prey.position.x, prey.position.y] = 0;

                if (predatorSelected.timeWithoutFood - 1 <= 0)
                {
                    predatorSelected.timeWithoutFood = 0;
                }
                else
                {
                    predatorSelected.timeWithoutFood--;
                }
            }
            else
            {
                preysAlive.Add(prey);
            }
        }

        // Eliminar predadores que mueren
        foreach (var predator in predatorCells)
        {
            if (predator.timeWithoutFood < numberOfPredatorsDiesWitoutFood)
            {
                predatorsAlive.Add(predator);
            }
            else
            {
                frontGrid[predator.position.x, predator.position.y] = 0;
            }
        }
    }

    public void Movement(List<Predator> predatorsAlive, List<Prey> preysAlive)
    {
        List<Vector2Int> occupedPositions = new List<Vector2Int>();

        foreach (var pred in predatorsAlive)
        {
            pred.lastPosition = pred.position;

            List<Vector2Int> freeSpaces = GetFreeSpaces(occupedPositions, pred.position, pred.position.x, pred.position.y);

            if (freeSpaces.Count > 0)
            {
                Vector2Int newPosition = freeSpaces[0];

                if (freeSpaces.Count >= 1)
                {
                    newPosition = freeSpaces[UnityEngine.Random.Range(0, freeSpaces.Count - 1)];
                }

                pred.lastPosition = pred.position;
                pred.position = newPosition;
            }


        }

        foreach (var prey in preysAlive)
        {
            List<Vector2Int> freeSpaces = GetFreeSpaces(occupedPositions, prey.position, prey.position.x, prey.position.y);
            prey.lastPosition = prey.position;

            if (freeSpaces.Count > 0)
            {
                Vector2Int newPosition = freeSpaces[0];

                if (freeSpaces.Count >= 1)
                {
                    newPosition = freeSpaces[UnityEngine.Random.Range(0, freeSpaces.Count - 1)];
                }

                prey.lastPosition = prey.position;
                prey.position = newPosition;
            }
        }
    }

    public Tuple<List<Predator>, List<Prey>> NearestNeighbor(int x, int y)
    /// Devuelve la cantidad de predadores y presas cercanas a la celda x,y de la siguiente forma: (pos_predadores, cantidad_predadores, pos_presas, cantidad_presas)
    {
        List<Predator> predatorNeighbors = new List<Predator>();
        List<Prey> preyNeighbors = new List<Prey>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // No revisa su celda actual

                int nx = x + dx, ny = y + dy;
                if (nx >= 0 && ny >= 0 && nx < gridSize && ny < gridSize)
                {
                    if (frontGrid[nx, ny] == 1)
                    {
                        Prey prey = preyCells.Find(p => Vector2Int.Equals(p.position, new Vector2Int(nx, ny)));
                        preyNeighbors.Add(prey);
                    }
                    else if (frontGrid[nx, ny] == 2)
                    {
                        Predator predator = predatorCells.Find(p => Vector2Int.Equals(p.position, new Vector2Int(nx, ny)));
                        predatorNeighbors.Add(predator);
                    }

                }
            }
        }

        Tuple<List<Predator>, List<Prey>> aliveNeighbors = Tuple.Create(predatorNeighbors, preyNeighbors);
        return aliveNeighbors;
    }

    public List<Vector2Int> GetFreeSpaces(List<Vector2Int> occupedPositions, Vector2Int lastPos, int x, int y)
    {
        List<Vector2Int> freeSpaces = new List<Vector2Int>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // No revisa su celda actual

                int nx = x + dx, ny = y + dy;
                if (nx >= 0 && ny >= 0 && nx < gridSize && ny < gridSize)
                {
                    if (frontGrid[nx, ny] == 0)
                    {
                        if (!occupedPositions.Contains(new Vector2Int(nx, ny)) && lastPos != new Vector2Int(nx, ny))
                        {
                            occupedPositions.Add(new Vector2Int(nx, ny));
                            freeSpaces.Add(new Vector2Int(nx, ny));
                        }
                    }
                }
            }
        }

        return freeSpaces;
    }
}
