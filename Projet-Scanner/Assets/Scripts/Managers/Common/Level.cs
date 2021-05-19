using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System.Linq;

public class Level : MonoBehaviour, IEventHandler
{
	List<PickupObject> m_ObjectInLevel = new List<PickupObject>();
	public int NumberOfObject { get { return m_ObjectInLevel.Count(); } }

	[Header("Time Before")]
	[SerializeField] float m_GameOver;
	public float GameOver { get { return m_GameOver; } }
	[SerializeField] float m_BlinkingLight;
	bool m_NoMoreBlinking = false;
	bool m_IsEnding = false;

	Light m_PlayerLight;
	float m_InitialPlayerIntensity;

	GameObject m_ChevalPrefab;
	Vector3 m_SecondPosition = new Vector3(0.823f, -0.724f, -0.785f + 20);
	Quaternion m_SecondRotation = Quaternion.Euler(0f, 90f, 0f);

	Vector3 m_ThirdPosition = new Vector3(-2.227f, -1.538f, 0.652f + 20);
	Quaternion m_ThirdRotation = Quaternion.Euler(0f, 0f, 0f);

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
	void OnDestroy()
	{
		UnsubscribeEvents();
	}
	#endregion

	void Awake()
	{
		SubscribeEvents();

		m_ObjectInLevel = FindObjectsOfType<PickupObject>().ToList();
		m_PlayerLight = GameObject.FindGameObjectWithTag("PlayerLight").GetComponent<Light>();
		m_ChevalPrefab = GameObject.FindGameObjectWithTag("Horse");
		m_InitialPlayerIntensity = m_PlayerLight.intensity;
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

		if (m_Timer >= m_GameOver)
			EventManager.Instance.Raise(new TimeIsUpEvent());

		if (!m_IsEnding && m_Timer >= m_GameOver - 1)
		{
			StartCoroutine(GameOverIsComing());
			m_IsEnding = true;
		}

		if (!m_NoMoreBlinking && m_Timer >= m_BlinkingLight)
		{
			StartCoroutine(BlinkingLight());
			m_NoMoreBlinking = true;
		}
	}

	IEnumerator BlinkingLight()
	{
		int n = 0;
		#region Première Lampe bug
		while (GameManager.Instance.IsPlaying)
		{
			m_PlayerLight.intensity = Random.Range(0.05f, 0.3f);
			yield return new WaitForSeconds(Random.Range(.035f, .065f));
			m_PlayerLight.intensity = m_InitialPlayerIntensity;
			yield return new WaitForSeconds(Random.Range(.05f, 3));
			n++;
			if (n > 10)
				break;
		}
		#endregion

		#region Première Coupe Lampe
		m_PlayerLight.intensity = 0;
		m_InitialPlayerIntensity = 0.2f;
		yield return new WaitForSeconds(Random.Range(1.25f, 1.5f));

		m_ChevalPrefab.transform.position = m_SecondPosition;
		m_ChevalPrefab.transform.rotation = m_SecondRotation;

		yield return new WaitForSeconds(Random.Range(1.25f, 1.5f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = Random.Range(0.025f, 0.10f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = Random.Range(0.025f, 0.10f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(8, 14));

		m_PlayerLight.intensity = Random.Range(0.025f, 0.10f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		m_InitialPlayerIntensity = 0.3f;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = Random.Range(0.025f, 0.10f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = Random.Range(0.025f, 0.10f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(15, 25));
		n = 0;
		#endregion

		#region Deuxième Lampe bug
		while (GameManager.Instance.IsPlaying)
		{
			m_PlayerLight.intensity = Random.Range(0.025f, 0.2f);
			yield return new WaitForSeconds(Random.Range(.035f, .065f));
			m_PlayerLight.intensity = m_InitialPlayerIntensity;
			yield return new WaitForSeconds(Random.Range(.05f, 3));
			n++;
			if (n > 10)
				break;
		}
		#endregion

		#region Deuxième Coupe Lampe
		m_PlayerLight.intensity = 0;
		m_InitialPlayerIntensity = 0.10f;
		yield return new WaitForSeconds(Random.Range(1.25f, 2f));

		m_ChevalPrefab.transform.position = m_ThirdPosition;
		m_ChevalPrefab.transform.rotation = m_ThirdRotation;
		EventManager.Instance.Raise(new HorseThirdPositionIsNowEvent());

		yield return new WaitForSeconds(Random.Range(1.25f, 2f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = Random.Range(0.015f, 0.05f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = Random.Range(0.015f, 0.05f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_InitialPlayerIntensity = 0.2f;
		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		#endregion
	}

	IEnumerator GameOverIsComing()
	{
		m_PlayerLight.intensity = Random.Range(0.015f, 0.08f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = Random.Range(0.015f, 0.08f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));

		m_PlayerLight.intensity = 0;
	}
}