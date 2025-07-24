using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public float movementSpeed = 1.0f; // уменьшили скорость движения
    public float rotationSpeed = 0.5f; // уменьшили скорость вращения

    private float mouseX = 0f;
    private float mouseY = 0f;

    private Camera mainCamera;
    private float fixedY;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        fixedY = mainCamera.transform.position.y;
    }

    void Update()
    {
        // Получаем ввод от мыши
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY += Input.GetAxis("Mouse Y") * rotationSpeed;

        // Ограничиваем угол обзора
        mouseY = Mathf.Clamp(mouseY, -90f, 90f);

        // Вращаем камеру
        mainCamera.transform.eulerAngles = new Vector3(-mouseY, mouseX, 0f);

        // Получаем ввод от клавиатуры
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        // Двигаем камеру без изменения высоты
        Vector3 movement = transform.forward * verticalInput * movementSpeed + transform.right * horizontalInput * movementSpeed;
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x + movement.x, fixedY, mainCamera.transform.position.z + movement.z);
    }
}

