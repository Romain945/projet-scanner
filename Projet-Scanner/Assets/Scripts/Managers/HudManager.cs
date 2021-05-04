using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class HudManager : Manager<HudManager>
{
	[Header("Texts")]
	[SerializeField] Text m_ScoreValueText;
	[SerializeField] Text m_PickupText;

	[Header("Timer")]
	[SerializeField] Text m_TimerValue;
	float m_Timer = 0f;
	float m_TimeBeforeGameOver;

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion

	#region Events subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();

		//LevelsManager
		EventManager.Instance.AddListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);

		EventManager.Instance.AddListener<CanPickupAnObjectEvent>(CanPickupAnObject);
		EventManager.Instance.AddListener<CantPickupAnObjectEvent>(CantPickupAnObject);

		EventManager.Instance.AddListener<ObjectHasBeenDestroyEvent>(ObjectHasBeenDestroy);
	}
	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

		//LevelsManager
		EventManager.Instance.RemoveListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);

		EventManager.Instance.RemoveListener<CanPickupAnObjectEvent>(CanPickupAnObject);
		EventManager.Instance.RemoveListener<CantPickupAnObjectEvent>(CantPickupAnObject);

		EventManager.Instance.RemoveListener<ObjectHasBeenDestroyEvent>(ObjectHasBeenDestroy);
	}
	#endregion

	#region Callbacks to GameManager events
	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		m_ScoreValueText.text = e.eScore.ToString();
	}
	void LevelHasBeenInstantiated(LevelHasBeenInstantiatedEvent e)
    {
		m_TimeBeforeGameOver = e.eLevel.TimeBeforeGameOver;
    }
	#endregion

	#region Callbacks to Pickup events
	void CanPickupAnObject(CanPickupAnObjectEvent e)
	{
		m_PickupText.enabled = true;
	}
	void CantPickupAnObject(CantPickupAnObjectEvent e)
	{
		m_PickupText.enabled = false;
	}
	void ObjectHasBeenDestroy(ObjectHasBeenDestroyEvent e)
	{
		m_PickupText.enabled = false;
	}
	#endregion

	void Update()
	{
		if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play

		m_TimerValue.text = string.Format("{0:0.00}", m_Timer);
		m_Timer += Time.deltaTime;

		if (m_Timer >= m_TimeBeforeGameOver)
			EventManager.Instance.Raise(new TimeIsUpEvent());
	}


    void Reset()
	{
		m_PickupText.enabled = false;
		m_Timer = 0;
		m_TimerValue.text = string.Format("{0:0.00}", m_Timer);

	}
    protected override void GameMenu(GameMenuEvent e)
	{
		Reset();
	}

}