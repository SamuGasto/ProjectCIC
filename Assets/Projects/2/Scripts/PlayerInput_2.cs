using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputs_2 : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerInteraction_2 playerInteraction;
    [Header("Flags")]
    public bool isClicking;
    public bool startClicking;
    public bool isSecondaryClicking;
    public Vector2 mousePosition;
    public Vector2 movement;

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }
    void OnAttack(InputValue value)
    {
        isClicking = value.Get<float>() > 0 ? true : false;

        if (isClicking)
        {
            playerInteraction.PlaceTile();
        }

    }
    void OnSecondaryClick(InputValue value)
    {
        isSecondaryClicking = value.Get<float>() > 0 ? true : false;

        if (isSecondaryClicking)
        {
            playerInteraction.RemoveTile();
        }
    }
    void OnPoint(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }
    void OnExit(InputValue value)
    {
        if (value.Get<float>() > 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

