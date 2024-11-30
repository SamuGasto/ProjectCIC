using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public VisualElement ui;

    public Button gameOfLifeButton;
    public Button button_2;
    public Button button_3;
    public Button button_4;
    public Button button_5;
    public Button button_6;
    public Button button_7;

    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }
    private void OnEnable()
    {
        gameOfLifeButton = ui.Q<Button>("GameOfLifeButton");
        gameOfLifeButton.clicked += OnGameOfLifeButtonClicked;

        button_2 = ui.Q<Button>("2Button");
        button_2.clicked += OnButton2Clicked;

        button_3 = ui.Q<Button>("3Button");
        button_3.clicked += OnButton3Clicked;

        button_4 = ui.Q<Button>("4Button");
        button_4.clicked += OnButton4Clicked;

        button_5 = ui.Q<Button>("5Button");
        button_5.clicked += OnButton5Clicked;

        button_6 = ui.Q<Button>("6Button");
        button_6.clicked += OnButton6Clicked;

        button_7 = ui.Q<Button>("7Button");
        button_7.clicked += OnButton7Clicked;
    }

    void OnGameOfLifeButtonClicked()
    {
        SceneManager.LoadScene("GameOfLife", LoadSceneMode.Single);
    }

    void OnButton2Clicked()
    {
        SceneManager.LoadScene("2");
    }

    void OnButton3Clicked()
    {
        SceneManager.LoadScene("3");
    }

    void OnButton4Clicked()
    {
        SceneManager.LoadScene("4");
    }

    void OnButton5Clicked()
    {
        SceneManager.LoadScene("5");
    }

    void OnButton6Clicked()
    {
        SceneManager.LoadScene("6");
    }

    void OnButton7Clicked()
    {
        SceneManager.LoadScene("7");
    }
}
