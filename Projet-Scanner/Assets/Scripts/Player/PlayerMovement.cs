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

    bool m_CanPickupObject;
    GameObject m_PickupObject;

    #region Events subscription
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();

        EventManager.Instance.AddListener<CanPickupAnObjectEvent>(CanPickupAnObject);
        EventManager.Instance.AddListener<CantPickupAnObjectEvent>(CantPickupAnObject);
    }
    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        EventManager.Instance.RemoveListener<CanPickupAnObjectEvent>(CanPickupAnObject);
        EventManager.Instance.RemoveListener<CantPickupAnObjectEvent>(CantPickupAnObject);
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

        Vector3 movement = (m_Transform.right * hAxis + m_Transform.forward * vAxis);
        m_Rigidbody.AddForce(movement * m_MoveSpeed, ForceMode.VelocityChange);
    }

    void Update()
    {
        if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play

        if (m_CanPickupObject && Input.GetKeyDown(KeyCode.E))
        {
            EventManager.Instance.Raise(new ObjectHasBeenDestroyEvent() { ePickupObject = m_PickupObject });
            Destroy(m_PickupObject);
            m_CanPickupObject = false;
            m_PickupObject = null;
        }
    }

    void CanPickupAnObject (CanPickupAnObjectEvent e)
    {
        m_CanPickupObject = true;
        m_PickupObject = e.ePickupObject;
    }

    void CantPickupAnObject (CantPickupAnObjectEvent e)
    {
        m_CanPickupObject = false;
        m_PickupObject = null;
    }

    void Reset()
    {
        m_Rigidbody.position = m_SpawnPoint.position;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        m_CanPickupObject = false;
        m_PickupObject = null;
    }
    protected override void GameMenu(GameMenuEvent e)
    {
        Reset();
    }
}