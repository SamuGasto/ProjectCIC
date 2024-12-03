using UnityEngine;

public class GameManager_3 : MonoBehaviour
{
    [Header("Simulación")]
    public bool isPlaying = false;
    public int Tiempo = 0;
    [Header("Configuración")]
    [SerializeField] float maxTimerValue = 100f;
    [SerializeField] float timer = 100f;
    [SerializeField] public float velocidadPasoDelTiempo = 1f;

    [Header("Referencias")]
    [SerializeField] GridManager_3 gridManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gridManager.Initialize();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime * (isPlaying ? velocidadPasoDelTiempo : 0);

        if (timer <= 0)
        {
            timer = maxTimerValue;
            Tiempo++;
            gridManager.UpdateFrontGrid();
        }
    }
}
