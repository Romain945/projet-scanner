using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PickupObject : MonoBehaviour, IEventHandler
{
    Transform m_PlayerTransform;

    bool m_CanPickUpObject;

	#region Events' subscription
	public void SubscribeEvents()
	{
		//Level
		EventManager.Instance.AddListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
	}
	public void UnsubscribeEvents()
	{
		//Level
		EventManager.Instance.RemoveListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
	}
	void Awake()
	{
		SubscribeEvents();
	}
	void OnDestroy()
	{
		UnsubscribeEvents();
	}
    #endregion

    #region Callbacks to levelManager events
    void LevelHasBeenInstantiated(LevelHasBeenInstantiatedEvent e)
    {
        m_PlayerTransform = e.ePlayerTransform;
    }
    #endregion

    void Update()
    {
        if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play

        if (!m_CanPickUpObject && Vector3.Distance(m_PlayerTransform.position, transform.position) <= 1)
        {
            m_CanPickUpObject = true;
            EventManager.Instance.Raise(new CanPickupAnObjectEvent() { ePickupObject = gameObject});
            return;
        }

        if (m_CanPickUpObject && Vector3.Distance(m_PlayerTransform.position, transform.position) >= 1.3)
        {
            m_CanPickUpObject = false;
            EventManager.Instance.Raise(new CantPickupAnObjectEvent() { ePickupObject = gameObject });
            return;
        }
    }
}
