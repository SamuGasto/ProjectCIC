using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager_3 : MonoBehaviour
{
    public int largo = 10;
    public int alto = 10;
    public float predatorPreyDensity = 0.5f;
    [SerializeField]
    public List<Celda> celdas; // Son de 4x3
    public TileBase predador_tile;
    public TileBase predador_hambriento_tile;
    public TileBase presa_tile;
    public TileBase base_tile;

    public int[,] frontGrid = new int[10, 10];
    [SerializeField] Tilemap tilemap_front;
    [SerializeField] Tilemap tilemap_back;

    public void PresentarCelda(Celda celda)
    {
        Debug.Log("---------- Celda -----------");
        Debug.Log("Coordenadas Celda: " + celda.coordenadas_globales_asociadas.x + ", " + celda.coordenadas_globales_asociadas.y);
        if (celda.celda_izquierda != null)
            Debug.Log("Celda izquierda: " + celda.celda_izquierda.coordenadas_globales_asociadas.x + ", " + celda.celda_izquierda.coordenadas_globales_asociadas.y);
        if (celda.celda_derecha != null)
            Debug.Log("Celda derecha: " + celda.celda_derecha.coordenadas_globales_asociadas.x + ", " + celda.celda_derecha.coordenadas_globales_asociadas.y);
        if (celda.celda_superior != null)
            Debug.Log("Celda superior: " + celda.celda_superior.coordenadas_globales_asociadas.x + ", " + celda.celda_superior.coordenadas_globales_asociadas.y);
        if (celda.celda_inferior != null)
            Debug.Log("Celda inferior: " + celda.celda_inferior.coordenadas_globales_asociadas.x + ", " + celda.celda_inferior.coordenadas_globales_asociadas.y);
        Debug.Log("---------- Datos -----------");
        Debug.Log("Hungry predators: " + celda.hungry_predadores.Count);
        Debug.Log("Full predators: " + celda.full_predadores.Count);
        Debug.Log("Presas: " + celda.presas.Count);
        Debug.Log("---------- Local grid -----------");
        for (int i = 2; i >= 0; i--)
        {
            Debug.Log("[" + celda.local_grid[0, i] + ", " + celda.local_grid[1, i] + ", " + celda.local_grid[2, i] + ", " + celda.local_grid[3, i] + "]");
        }
        Debug.Log("---------- Fin -----------");
    }
    public void ImprimirCelda(Celda celda)
    {
        for (int i = 0; i < celda.Largo; i++)
        {
            for (int j = 0; j < celda.Alto; j++)
            {
                if (celda.local_grid[i, j] == 1)
                {
                    frontGrid[celda.coordenadas_globales_asociadas.x + i, celda.coordenadas_globales_asociadas.y + j] = 1;
                    tilemap_front.SetTile(new Vector3Int(celda.coordenadas_globales_asociadas.x + i, celda.coordenadas_globales_asociadas.y + j), predador_tile);
                }
                else if (celda.local_grid[i, j] == 2)
                {
                    frontGrid[celda.coordenadas_globales_asociadas.x + i, celda.coordenadas_globales_asociadas.y + j] = 2;
                    tilemap_front.SetTile(new Vector3Int(celda.coordenadas_globales_asociadas.x + i, celda.coordenadas_globales_asociadas.y + j), predador_hambriento_tile);
                }
                else if (celda.local_grid[i, j] == 3)
                {
                    frontGrid[celda.coordenadas_globales_asociadas.x + i, celda.coordenadas_globales_asociadas.y + j] = 3;
                    tilemap_front.SetTile(new Vector3Int(celda.coordenadas_globales_asociadas.x + i, celda.coordenadas_globales_asociadas.y + j), presa_tile);
                }
                else
                {
                    frontGrid[celda.coordenadas_globales_asociadas.x + i, celda.coordenadas_globales_asociadas.y + j] = 0;
                    tilemap_front.SetTile(new Vector3Int(celda.coordenadas_globales_asociadas.x + i, celda.coordenadas_globales_asociadas.y + j), null);
                }
            }
        }
    }
    public void Initialize()
    {
        tilemap_front.ClearAllTiles();
        tilemap_back.ClearAllTiles();
        frontGrid = new int[largo * 4, alto * 3];

        for (int x = 0; x < largo; x++)
        {
            for (int y = 0; y < alto; y++)
            {
                tilemap_back.SetTile(new Vector3Int(x, y), base_tile);
                Celda newCelda = new Celda(predatorPreyDensity);
                celdas.Add(newCelda);
                int coordenada_x = newCelda.Largo * x;
                int coordenada_y = newCelda.Alto * y;
                newCelda.coordenadas_globales_asociadas = new Vector2Int(coordenada_x, coordenada_y);
                newCelda.Initialize();

                // Añadir la celda izquierda
                if (x > 0)
                {
                    Celda celda_izquierda = celdas.Find(c => Vector2Int.Equals(c.coordenadas_globales_asociadas, new Vector2Int(newCelda.Largo * (x - 1), newCelda.Alto * y)));

                    if (celda_izquierda != null)
                    {
                        newCelda.celda_izquierda = celda_izquierda;
                        celda_izquierda.celda_derecha = newCelda;
                    }
                }
                // Añadir la celda derecha
                if (x < largo - 1)
                {
                    Celda celda_derecha = celdas.Find(c => Vector2Int.Equals(c.coordenadas_globales_asociadas, new Vector2Int(newCelda.Largo * (x + 1), newCelda.Alto * y)));

                    if (celda_derecha != null)
                    {
                        newCelda.celda_derecha = celda_derecha;
                        celda_derecha.celda_izquierda = newCelda;
                    }
                }
                // Añadir la celda arriba
                if (y < alto)
                {
                    Celda celda_superior = celdas.Find(c => Vector2Int.Equals(c.coordenadas_globales_asociadas, new Vector2Int(newCelda.Largo * x, newCelda.Alto * (y + 1))));
                    if (celda_superior != null)
                    {
                        newCelda.celda_superior = celda_superior;
                        celda_superior.celda_inferior = newCelda;
                    }
                }
                // Añadir la celda abajo
                if (y > 0)
                {
                    Celda celda_inferior = celdas.Find(c => Vector2Int.Equals(c.coordenadas_globales_asociadas, new Vector2Int(newCelda.Largo * x, newCelda.Alto * (y - 1))));
                    if (celda_inferior != null)
                    {
                        newCelda.celda_inferior = celda_inferior;
                        celda_inferior.celda_superior = newCelda;
                    }
                }

                ImprimirCelda(newCelda);
            }
        }

        foreach (var celda in celdas)
        {
            PresentarCelda(celda);
        }
    }
    public void UpdateFrontGrid()
    {
        tilemap_front.ClearAllTiles();

        foreach (var celda in celdas)
        {
            Predacion_Reproduccion(celda);
            SeleccionDeDireccion(celda);
        }
        foreach (var celda in celdas)
        {
            ImprimirCelda(celda);
            PresentarCelda(celda);
        }
        Debug.Log("---------- CONCLUYÓ UPDATE -----------");
    }
    public void Predacion_Reproduccion(Celda celda)
    {
        // Un depredador hambriento muere si no hay presas en la misma celda.
        if (celda.hungry_predadores.Count > 0)
        {
            if (celda.presas.Count <= 0)
            {
                Predador predador_hambriento_seleccionado = celda.hungry_predadores.First();
                if (celda.hungry_predadores.Count > 1)
                {
                    predador_hambriento_seleccionado = celda.hungry_predadores[UnityEngine.Random.Range(0, celda.hungry_predadores.Count)];
                }
                celda.RemovePredador(predador_hambriento_seleccionado);
            }
        }


        /*
        Si en la misma celda hay al menos 2 presas,
        1 depredador hambriento y menos de 4
        depredadores llenos, entonces uno de los
        depredadores se come una presa y se
        convierte en depredador lleno.
        */
        if (celda.presas.Count >= 2)
        {
            if (celda.hungry_predadores.Count > 0)
            {
                if (celda.full_predadores.Count <= 3)
                {
                    Presa presa_seleccionada = celda.presas.First();
                    if (celda.presas.Count > 1)
                    {
                        presa_seleccionada = celda.presas[UnityEngine.Random.Range(0, celda.presas.Count)];
                    }
                    Predador predador_hambriendo_seleccionado = celda.hungry_predadores.First();
                    if (celda.hungry_predadores.Count > 1)
                    {
                        predador_hambriendo_seleccionado = celda.hungry_predadores[UnityEngine.Random.Range(0, celda.hungry_predadores.Count)];
                    }

                    celda.RemovePredador(predador_hambriendo_seleccionado);
                    predador_hambriendo_seleccionado.State = PredadorState.Full;
                    celda.AddPredador(predador_hambriendo_seleccionado);
                    celda.RemovePresa(presa_seleccionada);
                }
            }
        }

        /*
        Si no hay presas en la celda, un depredador
        lleno se vuelve hambriento.
        */
        if (celda.presas.Count <= 0)
        {
            if (celda.full_predadores.Count > 0)
            {
                Predador predador_seleccionado = celda.full_predadores.First();
                if (celda.full_predadores.Count > 1)
                {
                    predador_seleccionado = celda.full_predadores[UnityEngine.Random.Range(0, celda.full_predadores.Count)];
                }
                celda.RemovePredador(predador_seleccionado);
                predador_seleccionado.State = PredadorState.Hungry;
                celda.AddPredador(predador_seleccionado);
            }
        }

        /*
        Si hay por lo menos 2 presas en la celda, 1
        depredador lleno y menos de 3 depredadores
        hambrientos, entonces un depredador lleno
        se come una presa, dicho depredador
        reproduce un depredador hambriento, y el
        mismo depredador lleno se vuelve
        hambriento.
        */
        if (celda.presas.Count >= 2)
        {
            if (celda.full_predadores.Count > 0)
            {
                if (celda.hungry_predadores.Count < 3)
                {
                    Predador predador_seleccionado = celda.full_predadores.First();
                    if (celda.full_predadores.Count > 1)
                    {
                        predador_seleccionado = celda.full_predadores[UnityEngine.Random.Range(0, celda.full_predadores.Count)];
                    }
                    celda.RemovePredador(predador_seleccionado);
                    predador_seleccionado.State = PredadorState.Hungry;
                    celda.AddPredador(predador_seleccionado);

                    Predador nuevo_predador_hambriento = new Predador(predador_seleccionado.celda_asociada, PredadorState.Hungry, predador_seleccionado.position);
                    celda.AddPredador(nuevo_predador_hambriento);

                    Presa presa_seleccionada = celda.presas[UnityEngine.Random.Range(0, celda.presas.Count)];

                    celda.RemovePresa(presa_seleccionada);
                }
            }
        }

        /*
        Por último, si 2 o 3 presas se encuentran en
        la misma celda, se reproduce una nueva
        presa.
        */
        if (celda.presas.Count >= 2 && celda.presas.Count <= 3)
        {
            Presa nueva_presa = new Presa(celda, new Vector2Int(0, 0));
            celda.AddPresa(nueva_presa);
        }
    }
    public void SeleccionDeDireccion(Celda celda)
    {
        List<int> initial_directions = new List<int>();
        if (celda.hungry_predadores.Count < 4)
        {
            if (celda.celda_izquierda != null)
                initial_directions.Add(0);
            if (celda.celda_derecha != null)
                initial_directions.Add(1);
            if (celda.celda_superior != null)
                initial_directions.Add(2);
            if (celda.celda_inferior != null)
                initial_directions.Add(3);
        }


        if (initial_directions.Count > 0)
        {
            DistribuirPredadores(celda, celda.hungry_predadores, initial_directions);
        }

        initial_directions = new List<int>();
        if (celda.full_predadores.Count < 4)
        {
            if (celda.celda_izquierda != null)
                initial_directions.Add(0);
            if (celda.celda_derecha != null)
                initial_directions.Add(1);
            if (celda.celda_superior != null)
                initial_directions.Add(2);
            if (celda.celda_inferior != null)
                initial_directions.Add(3);
        }

        if (initial_directions.Count > 0)
        {
            DistribuirPredadores(celda, celda.full_predadores, initial_directions);
        }


        initial_directions = new List<int>();
        if (celda.presas.Count < 4)
        {
            if (celda.celda_izquierda != null)
                initial_directions.Add(0);
            if (celda.celda_derecha != null)
                initial_directions.Add(1);
            if (celda.celda_superior != null)
                initial_directions.Add(2);
            if (celda.celda_inferior != null)
                initial_directions.Add(3);
        }

        if (initial_directions.Count > 0)
        {
            DistribuirPresas(celda, celda.presas, initial_directions);
        }

    }
    public void DistribuirPredadores(Celda celda_origen, List<Predador> predadores, List<int> initial_directions)
    {
        List<int> direcciones_usadas = new List<int>(initial_directions);
        if (predadores.Count > 1)
        {
            List<Predador> predador_list = new List<Predador>(predadores); // Esta lista guardara todos los predador que falta por repartir

            for (int i = 0; i < predadores.Count; i++)
            {
                if (direcciones_usadas.Count <= 0)
                {
                    return;
                }

                int direccion = direcciones_usadas.Count > 1 ? direcciones_usadas[UnityEngine.Random.Range(0, direcciones_usadas.Count)] : direcciones_usadas.First();


                Predador predador_seleccionado = predador_list[UnityEngine.Random.Range(0, predador_list.Count)];
                if (celda_origen.RemovePredador(predador_seleccionado))
                {
                    if (direccion == 0)
                    {
                        celda_origen.celda_izquierda.AddPredador(predador_seleccionado);
                    }
                    else if (direccion == 1)
                    {
                        celda_origen.celda_derecha.AddPredador(predador_seleccionado);
                    }
                    else if (direccion == 2)
                    {
                        celda_origen.celda_superior.AddPredador(predador_seleccionado);
                    }
                    else if (direccion == 3)
                    {
                        celda_origen.celda_inferior.AddPredador(predador_seleccionado);
                    }

                    direcciones_usadas.Remove(direccion);

                    predador_list.Remove(predador_seleccionado);
                }
            }
        }
        else if (predadores.Count == 1)
        {
            if (direcciones_usadas.Count <= 0)
            {
                return;
            }
            int direccion = direcciones_usadas[UnityEngine.Random.Range(0, direcciones_usadas.Count)];
            Predador predador_seleccionado = predadores.First();
            if (celda_origen.RemovePredador(predador_seleccionado))
            {
                if (direccion == 0)
                {
                    celda_origen.celda_izquierda.AddPredador(predador_seleccionado);
                }
                else if (direccion == 1)
                {
                    celda_origen.celda_derecha.AddPredador(predador_seleccionado);
                }
                else if (direccion == 2)
                {
                    celda_origen.celda_superior.AddPredador(predador_seleccionado);
                }
                else if (direccion == 3)
                {
                    celda_origen.celda_inferior.AddPredador(predador_seleccionado);
                }
            }
        }
    }
    public void DistribuirPresas(Celda celda_origen, List<Presa> presas, List<int> initial_directions)
    {
        List<int> direcciones_usadas = new List<int>(initial_directions);
        if (presas.Count > 1)
        {
            List<Presa> presas_list = new List<Presa>(presas);

            for (int i = 0; i < presas.Count; i++)
            {
                if (direcciones_usadas.Count <= 0)
                {
                    return;
                }
                int direccion = direcciones_usadas.Count > 1 ? direcciones_usadas[UnityEngine.Random.Range(0, direcciones_usadas.Count)] : direcciones_usadas.First();


                Presa presa_seleccionada = presas_list[UnityEngine.Random.Range(0, presas_list.Count)];
                if (celda_origen.RemovePresa(presa_seleccionada))
                {
                    if (direccion == 0)
                    {
                        celda_origen.celda_izquierda.AddPresa(presa_seleccionada);
                    }
                    else if (direccion == 1)
                    {
                        celda_origen.celda_derecha.AddPresa(presa_seleccionada);
                    }
                    else if (direccion == 2)
                    {
                        celda_origen.celda_superior.AddPresa(presa_seleccionada);
                    }
                    else if (direccion == 3)
                    {
                        celda_origen.celda_inferior.AddPresa(presa_seleccionada);
                    }

                    direcciones_usadas.Remove(direccion);

                    presas_list.Remove(presa_seleccionada);
                }
            }
        }
        else if (presas.Count == 1)
        {
            if (direcciones_usadas.Count <= 0)
            {
                return;
            }

            int direccion = direcciones_usadas[UnityEngine.Random.Range(0, direcciones_usadas.Count)];
            Presa presa_seleccionada = presas.First();

            if (celda_origen.RemovePresa(presa_seleccionada))
            {
                if (direccion == 0)
                {
                    celda_origen.celda_izquierda.AddPresa(presa_seleccionada);
                }
                else if (direccion == 1)
                {
                    celda_origen.celda_derecha.AddPresa(presa_seleccionada);
                }
                else if (direccion == 2)
                {
                    celda_origen.celda_superior.AddPresa(presa_seleccionada);
                }
                else if (direccion == 3)
                {
                    celda_origen.celda_inferior.AddPresa(presa_seleccionada);
                }
            }
        }
    }

    public void GenerarJSON()
    {

    }

}
