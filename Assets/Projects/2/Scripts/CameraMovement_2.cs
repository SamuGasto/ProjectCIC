using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement_2 : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerInputs_2 playerInput;
    [Header("Configuración")]
    public float cameraSpeed = 10f;
    [SerializeField] float minX = 1;
    [SerializeField] float maxX = 66;
    [SerializeField] float minY = 1;
    [SerializeField] float maxY = 66;
    [SerializeField] float smoothVel = 1f;
    Vector3 refVelocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float finalPos_x = Mathf.Clamp(transform.position.x + playerInput.movement.x * cameraSpeed, minX, maxX);
        float finalPos_y = Mathf.Clamp(transform.position.y + playerInput.movement.y * cameraSpeed, minY, maxY);
        Vector3 newVector = Vector3.SmoothDamp(transform.position, new Vector3(finalPos_x, finalPos_y, transform.position.z), ref refVelocity, smoothVel, cameraSpeed * 10);
        transform.position = newVector;
    }

}
