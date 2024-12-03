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
    public bool InHUD;
    Vector3Int mousePos;

    private void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        mousePos = tilemap.WorldToCell(Camera.main.ScreenToWorldPoint(playerInputs.mousePosition));
        InHUD = !EventSystem.current.IsPointerOverGameObject();
    }


    public void PlaceTile()
    {
        Vector3Int pos = mousePos;
        if (!IsWithinBounds(pos) || InHUD) return;

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

    public void RemoveTile()
    {
        Vector3Int pos = mousePos;
        if (!IsWithinBounds(pos) || gridManager.frontGrid[pos.x, pos.y] == 0 || InHUD) return;

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
