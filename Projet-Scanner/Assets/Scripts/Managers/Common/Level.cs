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
	bool m_NoMoreBlinking = true;

	[SerializeField] Light[] m_LevelLight;
	float m_InitialIntensity;

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
		m_InitialIntensity = m_LevelLight[0].intensity;
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

		if(m_Timer >= m_GameOver)
			EventManager.Instance.Raise(new TimeIsUpEvent());

		if (m_NoMoreBlinking && m_Timer >= m_BlinkingLight)
		{
			m_IsBlinking = true;
			m_NoMoreBlinking = false;
		}

		if (m_IsBlinking)
        {
			StartCoroutine(BlinkingLight());
			m_IsBlinking = false;
        }

	}

	IEnumerator BlinkingLight()
    {
		int n = 0;
        while (GameManager.Instance.IsPlaying)
        {
			for (int i = 0; i < m_LevelLight.Length; i++)
				m_LevelLight[i].intensity = 0;
			yield return new WaitForSeconds(Random.Range(.035f, .065f));
			for (int i = 0; i < m_LevelLight.Length; i++)
				m_LevelLight[i].intensity = m_InitialIntensity;
			yield return new WaitForSeconds(Random.Range(.05f, 3));
			n++;
			if(n > 20)
				break;
		}

		for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].intensity = 0;
		yield return new WaitForSeconds(Random.Range(3,6));
		for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].intensity = m_InitialIntensity;
		yield return new WaitForSeconds(Random.Range(.05f, 1));

		while (GameManager.Instance.IsPlaying)
		{
			for (int i = 0; i < m_LevelLight.Length; i++)
				m_LevelLight[i].intensity = 0;
			yield return new WaitForSeconds(Random.Range(.035f, .065f));
			for (int i = 0; i < m_LevelLight.Length; i++)
				m_LevelLight[i].intensity = m_InitialIntensity;
			yield return new WaitForSeconds(Random.Range(.05f, 3));
		}
	}
}