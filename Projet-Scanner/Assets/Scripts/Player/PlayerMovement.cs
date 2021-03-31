using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("Player move speed")]
    [SerializeField] float m_MoveSpeed;

    [Tooltip("Player spawn")]
    [SerializeField] Transform m_SpawnPoint;

    Transform m_Transform;
    Rigidbody m_Rigidbody;

    void Awake()
    {
        m_Transform = GetComponent<Transform>();
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        Vector3 movement = (m_Transform.right * hAxis + m_Transform.forward * vAxis);
        m_Rigidbody.AddForce(movement * m_MoveSpeed, ForceMode.VelocityChange);
    }

    void Reset()
    {
        m_Rigidbody.position = m_SpawnPoint.position;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
    }
}