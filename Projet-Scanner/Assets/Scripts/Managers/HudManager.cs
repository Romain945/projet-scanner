using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class HudManager : Manager<HudManager>
{
	[Header("Texts")]
	[SerializeField] Text m_ScoreValueText;

	[Header("Timer")]
	[SerializeField] Text m_TimerValue;

	float m_Timer = 0f;

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
	}
	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
	}
	#endregion

	#region Callbacks to GameManager events
	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		m_ScoreValueText.text = e.eScore.ToString();
	}
	#endregion

	void Update()
	{
		if (GameManager.Instance && !GameManager.Instance.IsPlaying) return; // GameState.play

		m_TimerValue.text = string.Format("{0:0.00}", m_Timer);
		m_Timer += Time.deltaTime;
	}

    void Reset()
	{
		m_Timer = 0;
		m_TimerValue.text = string.Format("{0:0.00}", m_Timer);

	}
    protected override void GameMenu(GameMenuEvent e)
	{
		Reset();
	}

}