using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class LevelsManager : Manager<LevelsManager>
{
	#region levels & current level management
	[Header("LevelsManager")]
	[SerializeField] GameObject[] m_LevelsPrefabs;
	int m_CurrentLevelIndex;
	GameObject m_CurrentLevelGO;
	Level m_CurrentLevel;
	public Level CurrentLevel { get { return m_CurrentLevel; } }
	public bool IsLastLevel { get { return m_CurrentLevelIndex >= m_LevelsPrefabs.Length - 1; } }
	#endregion

	#region Manager implementation
	protected override IEnumerator InitCoroutine()
	{
		yield break;
	}
	#endregion

	#region Events' subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();
		EventManager.Instance.AddListener<GoToNextLevelEvent>(GoToNextLevel);
		EventManager.Instance.AddListener<GoToSpecificLevelEvent>(GoToSpecificLevel);
	}
	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();
		EventManager.Instance.RemoveListener<GoToNextLevelEvent>(GoToNextLevel);
		EventManager.Instance.RemoveListener<GoToSpecificLevelEvent>(GoToSpecificLevel);
	}
	#endregion

	#region Level flow
	void Reset()
	{
		Destroy(m_CurrentLevelGO);
		m_CurrentLevelGO = null;
		m_CurrentLevelIndex = -1;
	}

	void InstantiateLevel(int levelIndex)
	{
		levelIndex = Mathf.Max(levelIndex, 0) % m_LevelsPrefabs.Length;
		m_CurrentLevelGO = Instantiate(m_LevelsPrefabs[levelIndex]);

		m_CurrentLevel = m_CurrentLevelGO.GetComponent<Level>();
	}

	IEnumerator GoToNextLevelCoroutine()
	{
		Destroy(m_CurrentLevelGO);
		while (m_CurrentLevelGO) yield return null;

		InstantiateLevel(m_CurrentLevelIndex);
		EventManager.Instance.Raise(new LevelHasBeenInstantiatedEvent() { eLevel = m_CurrentLevel, eLevelIndex = m_CurrentLevelIndex });

	}
	#endregion

	#region Callbacks to GameManager events
	protected override void GameMenu(GameMenuEvent e)
	{
		Reset();
	}

	public void GoToNextLevel(GoToNextLevelEvent e)
	{
		m_CurrentLevelIndex++;
		StartCoroutine(GoToNextLevelCoroutine());
	}

	public void GoToSpecificLevel(GoToSpecificLevelEvent e)
	{
		m_CurrentLevelIndex = e.eLevelIndex;
		StartCoroutine(GoToNextLevelCoroutine());
	}
	#endregion
}