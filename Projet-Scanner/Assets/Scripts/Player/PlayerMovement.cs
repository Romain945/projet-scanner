using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PlayerMovement : SimpleGameStateObserver, IEventHandler
{
    [SerializeField] Camera playerCamera;
    CharacterController m_CharacterController;
    [SerializeField] float walkingSpeed = 7.5f;
    [SerializeField] float jumpSpeed = 8f;
    [SerializeField] float gravity = 20f;

    Vector3 moveDirection;

    [Tooltip("Player spawn")]
    [SerializeField] Transform m_SpawnPoint;

    Transform m_Transform;

    #region Events subscription
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();
    }
    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        m_Transform = GetComponent<Transform>();
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Update()
    {

        if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float speedZ = Input.GetAxis("Vertical");
        float speedX = Input.GetAxis("Horizontal");

        float speedY = moveDirection.y;

        speedX = speedX * walkingSpeed;
        speedZ = speedZ * walkingSpeed;

        moveDirection = forward * speedZ + right * speedX;

        if (Input.GetKeyDown(KeyCode.Space) && m_CharacterController.isGrounded)
            moveDirection.y = jumpSpeed;
        else
            moveDirection.y = speedY;

        if (!m_CharacterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        m_CharacterController.Move(moveDirection * Time.deltaTime);
    }

    void Reset()
    {
        m_Transform.position = m_SpawnPoint.position;
    }
    protected override void GameMenu(GameMenuEvent e)
    {
        Reset();
    }
}