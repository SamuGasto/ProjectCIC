using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerInteraction : MonoBehaviour
{
    [Header("References")]
    public GridManager gridManager;
    [Header("Input Config")]
    public PlayerInputs playerInputs;
    public TileBase baseTile;
    public TileBase lifeTile;
    public Tilemap tilemap;

    public void HandleInput()
    {
        Vector3Int pos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(playerInputs.mousePosition));
        if (playerInputs.isClicking && !EventSystem.current.IsPointerOverGameObject())
        {

            PlaceTile(pos);
        }
        else if (playerInputs.isSecondaryClicking && !EventSystem.current.IsPointerOverGameObject())
        {
            RemoveTile(pos);
        }
    }

    private void PlaceTile(Vector3Int pos)
    {
        if (!IsWithinBounds(pos)) return;
        Debug.Log($"Placing tile at {pos}");

        tilemap.SetTile(pos, lifeTile);
        gridManager.currentGrid[pos.x, pos.y] = true;
        gridManager.activeCells.Add(new Vector2Int(pos.x, pos.y));

        // Añade vecinos como activos para mantener la consistencia
        foreach (var neighbor in gridManager.GetNeighbors(new Vector2Int(pos.x, pos.y)))
        {
            gridManager.activeCells.Add(neighbor);
        }
    }

    private void RemoveTile(Vector3Int pos)
    {
        if (!IsWithinBounds(pos) || !gridManager.currentGrid[pos.x, pos.y]) return;

        tilemap.SetTile(pos, baseTile);
        gridManager.currentGrid[pos.x, pos.y] = false;
        gridManager.activeCells.Remove(new Vector2Int(pos.x, pos.y));
    }

    private bool IsWithinBounds(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < gridManager.gridSize && pos.y >= 0 && pos.y < gridManager.gridSize;
    }

}
