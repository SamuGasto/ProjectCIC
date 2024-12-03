using System;
using UnityEngine;

public enum PredadorState
{
    Hungry,
    Full
}
public class Predador
{
    public PredadorState State = PredadorState.Hungry;
    public Celda celda_asociada;
    public Vector2Int position; // Posición en la celda
    public Predador(Celda celda_asociada, PredadorState state, Vector2Int position)
    {
        this.celda_asociada = celda_asociada;
        this.State = state;
        this.position = position;
    }
}
