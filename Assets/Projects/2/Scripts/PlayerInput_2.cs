using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs_2 : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerInteraction_2 playerInteraction;
    [Header("Flags")]
    public bool isClicking;
    public bool isSecondaryClicking;
    public Vector2 mousePosition;
    public Vector2 movement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (isSecondaryClicking)
        {
            playerInteraction.HandleInput();
        }
    }

    void OnMove(InputValue value)
    {
        movement = value.Get<Vector2>();
    }
    void OnAttack(InputValue value)
    {
        isClicking = value.Get<float>() > 0 ? true : false;

        if (isClicking)
        {
            playerInteraction.HandleInput();
        }
    }
    void OnSecondaryClick(InputValue value)
    {
        isSecondaryClicking = value.Get<float>() > 0 ? true : false;
    }
    void OnPoint(InputValue value)
    {
        mousePosition = value.Get<Vector2>();
    }
}

