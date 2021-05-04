using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System.Linq;

public class Level : MonoBehaviour, IEventHandler
{
	List<PickupObject> m_ObjectInLevel = new List<PickupObject>();

	[SerializeField] float m_TimeBeforeGameOver;
	public float TimeBeforeGameOver { get { return m_TimeBeforeGameOver; } }

	#region Events' subscription
	public void SubscribeEvents()
	{
		//Level
		EventManager.Instance.AddListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);

		//Player
		EventManager.Instance.AddListener<ObjectHasBeenDestroyEvent>(ObjectHasBeenDestroy);
	}
	public void UnsubscribeEvents()
	{
		//Level
		EventManager.Instance.RemoveListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);

		//Player
		EventManager.Instance.RemoveListener<ObjectHasBeenDestroyEvent>(ObjectHasBeenDestroy);
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

	void LevelHasBeenInstantiated(LevelHasBeenInstantiatedEvent e)
	{
		m_ObjectInLevel = FindObjectsOfType<PickupObject>().ToList();
	}

	void ObjectHasBeenDestroy(ObjectHasBeenDestroyEvent e)
	{
		m_ObjectInLevel.Remove(e.ePickupObject.GetComponent<PickupObject>());
		if (m_ObjectInLevel.Count <= 0)
			EventManager.Instance.Raise(new LastObjectHasBeenDestroyEvent());
	}
}