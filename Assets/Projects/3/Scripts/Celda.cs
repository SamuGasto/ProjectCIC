using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Celda
{
    private float predatorPreyDensity;
    public Celda(float predatorPreyDensity)
    {
        this.predatorPreyDensity = predatorPreyDensity;
    }
    public Vector2Int coordenadas_globales_asociadas;
    public Celda celda_superior;
    public Celda celda_izquierda;
    public Celda celda_derecha;
    public Celda celda_inferior;
    private int largo = 4;
    public int Largo { get { return largo; } }
    private int alto = 3;
    public int Alto { get { return alto; } }
    [SerializeField]
    public int[,] local_grid = new int[4, 3]; // 0 = Vacio, 1 = Predador, 2 = Predador Hambriento, 3 = Presa
    public List<Predador> hungry_predadores;
    public List<Predador> full_predadores;
    public List<Presa> presas;

    public void Initialize()
    {
        hungry_predadores = new List<Predador>();
        full_predadores = new List<Predador>();
        presas = new List<Presa>();

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (UnityEngine.Random.value < predatorPreyDensity)
                { // Celda ocupada?
                    if (UnityEngine.Random.value > 0.5f)
                    {   // Predador o Presa?
                        if (UnityEngine.Random.value > 0.5f)
                        { // Predador Hambriento
                            if (hungry_predadores.Count <= 3)
                            {
                                AddPredador(new Predador(this, PredadorState.Hungry, new Vector2Int(i, j)));
                            }

                        }
                        else
                        {
                            if (full_predadores.Count <= 3)
                            {
                                AddPredador(new Predador(this, PredadorState.Full, new Vector2Int(i, j)));
                            }

                        }
                    }
                    else
                    { // Presa
                        if (presas.Count <= 3)
                        {
                            AddPresa(new Presa(this, new Vector2Int(i, j)));
                        }
                    }
                }
            }
        }
    }

    public bool AddPredador(Predador predador)
    {
        if (predador.State == PredadorState.Hungry)
        {
            if (hungry_predadores.Count <= 3)
            {
                predador.celda_asociada = this;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (local_grid[i, j] == 0)
                        {
                            local_grid[i, j] = 2;
                            predador.position = new Vector2Int(i, j);
                            hungry_predadores.Add(predador);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        else if (predador.State == PredadorState.Full)
        {
            if (full_predadores.Count <= 3)
            {
                predador.celda_asociada = this;

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (local_grid[i, j] == 0)
                        {
                            local_grid[i, j] = 1;
                            predador.position = new Vector2Int(i, j);
                            full_predadores.Add(predador);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        return false;
    }
    public bool AddPresa(Presa presa)
    {
        if (presas.Count <= 3)
        {
            presa.celda_asociada = this;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (local_grid[i, j] == 0)
                    {
                        local_grid[i, j] = 3;
                        presa.position = new Vector2Int(i, j);
                        presas.Add(presa);
                        return true;
                    }
                }
            }
        }
        return false;
    }
    public bool RemovePredador(Predador predador)
    {
        if (hungry_predadores.Contains(predador))
        {
            local_grid[predador.position.x, predador.position.y] = 0;

            hungry_predadores.Remove(predador);
            return true;
        }
        else if (full_predadores.Contains(predador))
        {
            local_grid[predador.position.x, predador.position.y] = 0;

            full_predadores.Remove(predador);
            return true;
        }
        return false;
    }
    public bool RemovePresa(Presa presa)
    {
        if (presas.Contains(presa))
        {
            local_grid[presa.position.x, presa.position.y] = 0;

            presas.Remove(presa);
            return true;
        }
        return false;
    }
}
