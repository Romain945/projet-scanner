using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PlayerRotation : SimpleGameStateObserver, IEventHandler
{
    [Tooltip("Player Transform for horizontal rotation")]
    [SerializeField] Transform m_PlayerTransform;

    Transform m_Transform;

    float xRotation = 0f;
    float m_MouseSensitivity = 0f;

    protected override void Awake()
    {
        base.Awake();
        m_Transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play

        float mouseY = Input.GetAxis("Mouse Y") * m_MouseSensitivity * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * m_MouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        m_Transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotation de la tête horizontale et verticale
        m_PlayerTransform.Rotate(Vector3.up * mouseX); // Rotation du joueur a l'horizontale seulement (pour les déplacements)
    }

    #region Callbacks to SettingsManager events
    protected override void GameSettingsChanged(GameSettingsChangedEvent e)
    {
        m_MouseSensitivity = e.eMouseSensitivity;
    }
    #endregion

    void Reset()
    {
        xRotation = 0f;
        m_Transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Rotation de la tête horizontale et verticale
        m_PlayerTransform.rotation = Quaternion.identity; // Rotation du joueur a l'horizontale seulement (pour les déplacements)
    }
    protected override void GameMenu(GameMenuEvent e)
    {
        Reset();
    }
}