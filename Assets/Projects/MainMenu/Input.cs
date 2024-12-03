using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Input : MonoBehaviour
{
    public void OnExit(InputValue value)
    {
        if (value.Get<float>() > 0)
        {
            SceneManager.LoadScene("MainMenu");
        }

    }
}
