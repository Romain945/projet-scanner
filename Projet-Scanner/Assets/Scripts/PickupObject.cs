using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PickupObject : MonoBehaviour, IEventHandler
{
    GameObject m_PlayerGO;

	bool m_HorseIsOnThridPosition = false;
	bool m_HorseIsHit = false;

	Vector3 m_HorseLastPosition = new Vector3(-4.731f, -1.545f, 4.619f + 20);
	Quaternion m_HorseLastRotation = Quaternion.Euler(0, -10.414f, 0);

	#region Events' subscription
	public void SubscribeEvents()
	{
		//Level
		EventManager.Instance.AddListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
		EventManager.Instance.AddListener<HorseThirdPositionIsNowEvent>(HorseThirdPositionIsNow);
	}
	public void UnsubscribeEvents()
	{
		//Level
		EventManager.Instance.RemoveListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
		EventManager.Instance.RemoveListener<HorseThirdPositionIsNowEvent>(HorseThirdPositionIsNow);
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
	void HorseThirdPositionIsNow (HorseThirdPositionIsNowEvent e)
    {
		m_HorseIsOnThridPosition = true;
	}
	#endregion

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject == m_PlayerGO)     //player has collided with object
		{
			if (gameObject.CompareTag("Horse") && m_HorseIsHit)
			{
				EventManager.Instance.Raise(new ObjectHasBeenDestroyEvent() { ePickupObject = gameObject });
				EventManager.Instance.Raise(new HorseHasBeenDestroyEvent());
				Destroy(gameObject);
				return;
			}
            else
			{
				EventManager.Instance.Raise(new ObjectHasBeenDestroyEvent() { ePickupObject = gameObject });
				Destroy(gameObject);
			}
		}
	}

	void Update()
    {
		if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play
		if (!m_HorseIsOnThridPosition) return;
		if (!m_HorseIsHit && gameObject.CompareTag("Horse") && Vector3.Distance(gameObject.transform.position, m_PlayerGO.transform.position) <= 1)
        {
			m_HorseIsHit = true;
			gameObject.transform.position = m_HorseLastPosition;
			gameObject.transform.rotation = m_HorseLastRotation;
			EventManager.Instance.Raise(new HorseHasBeenHitEvent());
		}
	}
}
