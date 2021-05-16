using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PlayerMovement : SimpleGameStateObserver, IEventHandler
{
    [Tooltip("Player move speed")]
    [SerializeField] float m_MoveSpeed;

    [Tooltip("Player spawn")]
    [SerializeField] Transform m_SpawnPoint;

    Transform m_Transform;
    Rigidbody m_Rigidbody;

    bool m_IsGrounded;

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
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play

        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = m_Transform.right * hAxis + m_Transform.forward * vAxis;
        m_Rigidbody.velocity = movement;
    }

    void OnCollisionStay()
    {
        m_IsGrounded = true;
    }

    void Update()
    {
        if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play

        if (Input.GetKeyDown(KeyCode.Space) && m_IsGrounded)
        {
            m_Rigidbody.AddForce(0, 3.5f, 0, ForceMode.Impulse);
            m_IsGrounded = false;
        }
    }

    void Reset()
    {
        m_Rigidbody.position = m_SpawnPoint.position;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
    }
    protected override void GameMenu(GameMenuEvent e)
    {
        Reset();
    }
}