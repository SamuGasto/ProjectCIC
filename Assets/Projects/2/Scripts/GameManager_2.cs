using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager_2 : MonoBehaviour
{
    [Header("Simulación")]
    public bool isPlaying = false;
    public int Tiempo = 0;


    [Header("Configuración")]
    [SerializeField] float maxTimerValue = 100f;
    [SerializeField] float timer = 100f;
    [SerializeField] public float velocidadPasoDelTiempo = 1f;

    [Header("Referencias")]
    [SerializeField] GridManager_2 gridManager;
    [SerializeField] PlayerInteraction_2 playerInteraction;


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
