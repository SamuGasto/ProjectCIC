using UnityEngine;

public class Presa
{
    public Celda celda_asociada;
    public Vector2Int position; // Posición en la celda
    public Presa(Celda celda_asociada, Vector2Int position)
    {
        this.celda_asociada = celda_asociada;
        this.position = position;
    }
}
