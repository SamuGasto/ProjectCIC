using System.Collections.Generic;
using Cinemachine;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    [Header("Simulación")]
    public bool isPlaying = false;
    public int Tiempo = 0;


    [Header("Configuración")]
    [SerializeField] float maxTimerValue = 100f;
    [SerializeField] float timer = 100f;
    [SerializeField] public float velocidadPasoDelTiempo = 1f;

    [Header("Referencias")]
    [SerializeField] GridManager gridManager;
    [SerializeField] PlayerInteraction playerInteraction;

    void Start()
    {
        gridManager.InitializeGrid();
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
    }

    void Update()
    {
        timer -= Time.deltaTime * (isPlaying ? velocidadPasoDelTiempo : 0);

        if (timer <= 0)
        {
            timer = maxTimerValue;
            Tiempo++;
            gridManager.UpdateGrid();
        }
    }
}
