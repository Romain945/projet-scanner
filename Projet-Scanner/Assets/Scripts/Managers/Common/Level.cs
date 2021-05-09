using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System.Linq;

public class Level : MonoBehaviour, IEventHandler
{
	List<PickupObject> m_ObjectInLevel = new List<PickupObject>();

	[Header ("Time Before")]
	[SerializeField] float m_GameOver;
	public float GameOver { get { return m_GameOver; } }
	[SerializeField] float m_BlinkingLight;
	bool m_IsBlinking;

	float m_Timer = 0;

	#region Events' subscription
	public void SubscribeEvents()
	{
		//Player
		EventManager.Instance.AddListener<ObjectHasBeenDestroyEvent>(ObjectHasBeenDestroy);
	}
	public void UnsubscribeEvents()
	{
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

	void Start()
    {
		m_ObjectInLevel = FindObjectsOfType<PickupObject>().ToList();
	}

	void ObjectHasBeenDestroy(ObjectHasBeenDestroyEvent e)
	{
		m_ObjectInLevel.Remove(e.ePickupObject.GetComponent<PickupObject>());
		if (m_ObjectInLevel.Count <= 0)
			EventManager.Instance.Raise(new LastObjectHasBeenDestroyEvent());
	}

	void Update()
    {
		if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play

		m_Timer += Time.deltaTime;

		if (m_Timer >= m_BlinkingLight)
			m_IsBlinking = true;

		//if(m_IsBlinking)

	}
}