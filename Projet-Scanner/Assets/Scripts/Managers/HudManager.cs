using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class HudManager : Manager<HudManager>
{
	[Header("Texts")]
	[SerializeField] Text m_ScoreValueText;
	[SerializeField] Text m_ScoreMaxValueText;
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
	}
	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

		//LevelsManager
		EventManager.Instance.RemoveListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);
	}
	#endregion

	#region Callbacks to GameManager events
	protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
	{
		m_ScoreValueText.text = e.eScore.ToString();
	}
    #endregion

    #region Callbacks to LevelManager events
	void LevelHasBeenInstantiated(LevelHasBeenInstantiatedEvent e)
    {
		m_ScoreMaxValueText.text = "/ "+e.eLevel.NumberOfObject.ToString();
	}
    #endregion
}