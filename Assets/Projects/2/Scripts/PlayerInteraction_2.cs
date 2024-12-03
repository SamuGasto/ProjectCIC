using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class PlayerInteraction_2 : MonoBehaviour
{
    [Header("References")]
    public GridManager_2 gridManager;
    [Header("Input Config")]
    public PlayerInputs_2 playerInputs;
    public TileBase preyTile;
    public TileBase predatorTile;
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

        if (gridManager.frontGrid[pos.x, pos.y] == 0)
        {
            gridManager.AddPrey(new Vector2Int(pos.x, pos.y));
            tilemap.SetTile(pos, preyTile);
        }
        else if (gridManager.frontGrid[pos.x, pos.y] == 1)
        {
            gridManager.RemovePrey(new Vector2Int(pos.x, pos.y));
            gridManager.AddPredator(new Vector2Int(pos.x, pos.y));
            tilemap.SetTile(pos, predatorTile);
        }
        else
        {
            gridManager.RemovePredator(new Vector2Int(pos.x, pos.y));
            tilemap.SetTile(pos, null);
        }


    }

    private void RemoveTile(Vector3Int pos)
    {
        if (!IsWithinBounds(pos) || gridManager.frontGrid[pos.x, pos.y] == 0) return;

        if (gridManager.frontGrid[pos.x, pos.y] == 1)
        {
            gridManager.RemovePrey(new Vector2Int(pos.x, pos.y));
        }
        else if (gridManager.frontGrid[pos.x, pos.y] == 2)
        {
            gridManager.RemovePredator(new Vector2Int(pos.x, pos.y));
        }

        tilemap.SetTile(pos, null);
    }

    private bool IsWithinBounds(Vector3Int pos)
    {
        return pos.x >= 0 && pos.x < gridManager.gridSize && pos.y >= 0 && pos.y < gridManager.gridSize;
    }

}
