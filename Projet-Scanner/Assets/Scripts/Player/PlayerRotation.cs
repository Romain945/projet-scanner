using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [Tooltip("Player Transform for horizontal rotation")]
    [SerializeField] Transform m_PlayerTransform;

    Transform m_Transform;

    float xRotation = 0f;
    [SerializeField] float m_MouseSensitivity;

    void Awake()
    {
        m_Transform = GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void FixedUpdate()
    {

        float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        m_Transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotation de la tête horizontale et verticale
        m_PlayerTransform.Rotate(Vector3.up * mouseX); // Rotation du joueur a l'horizontale seulement (pour les déplacements)
    }

    void Reset()
    {
        xRotation = 0f;
        m_Transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotation de la tête horizontale et verticale
        m_PlayerTransform.rotation = Quaternion.identity; // Rotation du joueur a l'horizontale seulement (pour les déplacements)
    }
}