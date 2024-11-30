using UnityEngine;
using UnityEngine.UIElements;

public class PlayerHUDController : MonoBehaviour
{
    public GameManager gameManager;
    public GridManager gridManager;
    public VisualElement ui;
    public Slider tiempoEntreFramesSlider;
    public Button randomizeButton;
    public Button playPauseButton;
    public VisualElement playPauseIcon;
    public bool isPlaying;
    public Sprite playIcon;
    public Sprite stopIcon;

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

        tiempoEntreFramesSlider = ui.Q<Slider>("TiempoEntreFramesSlider");
        tiempoEntreFramesSlider.RegisterValueChangedCallback(OnTiempoEntreFramesSliderChanged);
        gameManager.velocidadPasoDelTiempo = tiempoEntreFramesSlider.value;

        playPauseIcon = ui.Q<VisualElement>("PlayPauseIcon");
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
}
