using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PickupObject : MonoBehaviour, IEventHandler
{
    GameObject m_PlayerGO;

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
		m_PlayerGO = GameObject.FindGameObjectWithTag("Player");
	}
	#endregion

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == m_PlayerGO)     //player has collided with object
		{
			EventManager.Instance.Raise(new ObjectHasBeenDestroyEvent() { ePickupObject = gameObject });
			Destroy(gameObject);
		}
	}
}
