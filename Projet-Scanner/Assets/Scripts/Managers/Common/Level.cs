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

	Light m_PlayerLight;
	float m_InitialPlayerIntensity;

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
		m_PlayerLight = GameObject.FindGameObjectWithTag("PlayerLight").GetComponent<Light>();
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
        #region Première phase
        while (GameManager.Instance.IsPlaying)
        {
			for (int i = 0; i < m_LevelLight.Length; i++)
				m_LevelLight[i].transform.parent.gameObject.SetActive(false);
			yield return new WaitForSeconds(Random.Range(.035f, .065f));
			for (int i = 0; i < m_LevelLight.Length; i++)
				m_LevelLight[i].transform.parent.gameObject.SetActive(true);
			yield return new WaitForSeconds(Random.Range(.05f, 3));
			n++;
			if(n > 5)
				break;
		}
        #endregion

        #region Coupure et baisse d'intensité
        for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].transform.parent.gameObject.SetActive(false);

		m_InitialIntensity = 0.5f;

		for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].intensity = m_InitialIntensity;
		yield return new WaitForSeconds(Random.Range(4,7));

		for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].transform.parent.gameObject.SetActive(true);
		yield return new WaitForSeconds(Random.Range(.05f, 1));
		n = 0;
        #endregion

        #region Deuxième phase
        while (GameManager.Instance.IsPlaying)
		{
			for (int i = 0; i < m_LevelLight.Length; i++)
				m_LevelLight[i].transform.parent.gameObject.SetActive(false);
			yield return new WaitForSeconds(Random.Range(.035f, .065f));
			for (int i = 0; i < m_LevelLight.Length; i++)
				m_LevelLight[i].transform.parent.gameObject.SetActive(true);
			yield return new WaitForSeconds(Random.Range(.05f, 2.5f));
			n++;
			if (n > 30)
				break;
		}
        #endregion

        #region Deuxième coupure et arrêt lumière
        for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].transform.parent.gameObject.SetActive(false);

		m_InitialIntensity = 0.15f;

		for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].intensity = m_InitialIntensity;
		yield return new WaitForSeconds(Random.Range(4, 7));

		for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].transform.parent.gameObject.SetActive(true);
		yield return new WaitForSeconds(Random.Range(1, 2));
		for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].transform.parent.gameObject.SetActive(false);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));
		for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].transform.parent.gameObject.SetActive(true);
		yield return new WaitForSeconds(Random.Range(.05f, .095f));
		for (int i = 0; i < m_LevelLight.Length; i++)
			m_LevelLight[i].transform.parent.gameObject.SetActive(false);

		yield return new WaitForSeconds(Random.Range(10, 20));
		n = 0;
		#endregion

		#region Première Lampe bug
		while (GameManager.Instance.IsPlaying)
		{
			m_PlayerLight.intensity = Random.Range(0.05f, 0.3f);
			yield return new WaitForSeconds(Random.Range(.035f, .065f));
			m_PlayerLight.intensity = m_InitialPlayerIntensity;
			yield return new WaitForSeconds(Random.Range(.05f, 3));
			n++;
			if (n > 15)
				break;
		}
        #endregion

        #region Coupe Lampe
        m_PlayerLight.intensity = 0;
		m_InitialPlayerIntensity = 0.2f;
		yield return new WaitForSeconds(Random.Range(0.8f, 1.5f));

		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));
		m_PlayerLight.intensity = Random.Range(0.025f, 0.15f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));
		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		yield return new WaitForSeconds(Random.Range(.035f, .065f));
		m_PlayerLight.intensity = Random.Range(0.025f, 0.15f);
		yield return new WaitForSeconds(Random.Range(.035f, .065f));
		m_PlayerLight.intensity = m_InitialPlayerIntensity;
		m_InitialPlayerIntensity = 0.3f;
		yield return new WaitForSeconds(Random.Range(4, 6));
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
	}
}