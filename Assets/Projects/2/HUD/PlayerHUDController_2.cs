using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUDController_2 : MonoBehaviour
{
    public GameManager_2 gameManager;
    public GridManager_2 gridManager;
    public VisualElement ui;
    public Slider tiempoEntreFramesSlider;
    public Slider densidadPoblacionalSlider;
    public IntegerField cantDiasPredadorVivirSinComer;
    public Button randomizeButton;
    public Button playPauseButton;
    public VisualElement playPauseIcon;
    public bool isPlaying;
    public Sprite playIcon;
    public Sprite stopIcon;
    public Label cantPredadores;
    public Label cantPresas;
    public Label cantDias;
    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }
    private void OnEnable()
    {
        randomizeButton = ui.Q<Button>("RandomizeButton");
        randomizeButton.clicked += OnRandomizeButtonClicked;

        playPauseButton = ui.Q<Button>("PlayPauseButton");
        playPauseButton.clicked += OnPlayPauseButtonClicked;

        playPauseIcon = ui.Q<VisualElement>("PlayPauseIcon");

        tiempoEntreFramesSlider = ui.Q<Slider>("SliderFPS");
        tiempoEntreFramesSlider.RegisterValueChangedCallback(OnTiempoEntreFramesSliderChanged);
        gameManager.velocidadPasoDelTiempo = tiempoEntreFramesSlider.value;

        densidadPoblacionalSlider = ui.Q<Slider>("SliderDensity");
        densidadPoblacionalSlider.RegisterValueChangedCallback(OnDensidadPoblacionalSliderChanged);
        gridManager.predatorPreyDensity = densidadPoblacionalSlider.value;

        cantDiasPredadorVivirSinComer = ui.Q<IntegerField>("CantDaysWithoutFood");
        cantDiasPredadorVivirSinComer.RegisterValueChangedCallback(OnCantDaysWithoutFoodChanged);
        gridManager.numberOfPredatorsDiesWitoutFood = cantDiasPredadorVivirSinComer.value;

        cantDias = ui.Q<Label>("CantDays");

        cantPresas = ui.Q<Label>("CantPrey");

        cantPredadores = ui.Q<Label>("CantPred");
    }
    private void LateUpdate()
    {
        cantDias.text = gameManager.Tiempo.ToString();
        cantPresas.text = gridManager.preyCells.Count.ToString();
        cantPredadores.text = gridManager.predatorCells.Count.ToString();
    }
    void OnRandomizeButtonClicked()
    {
        gridManager.Randomize();
    }
    void OnPlayPauseButtonClicked()
    {
        StyleBackground backgroundImage = new Background() { sprite = !isPlaying ? stopIcon : playIcon };
        playPauseIcon.style.backgroundImage = backgroundImage;
        isPlaying = !isPlaying;
        gameManager.isPlaying = isPlaying;
    }
    void OnTiempoEntreFramesSliderChanged(ChangeEvent<float> evt)
    {
        gameManager.velocidadPasoDelTiempo = evt.newValue;
    }
    void OnDensidadPoblacionalSliderChanged(ChangeEvent<float> evt)
    {
        gridManager.predatorPreyDensity = evt.newValue;
    }
    void OnCantDaysWithoutFoodChanged(ChangeEvent<int> evt)
    {
        gridManager.numberOfPredatorsDiesWitoutFood = evt.newValue;
    }
}
