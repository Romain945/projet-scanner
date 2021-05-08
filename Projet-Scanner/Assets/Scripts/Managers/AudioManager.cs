﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class AudioManager : Manager<AudioManager>
{
	#region Audio source & settings
	[Header("Audio Source")]
	[SerializeField] AudioSource m_MenuMusic;
	[SerializeField] AudioSource m_LevelMusic;
    [SerializeField] AudioSource m_ClickSound;
    [SerializeField] AudioSource m_PickupSound;
    #endregion

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

        //MainMenuManager
        EventManager.Instance.AddListener<SettingsButtonClickedEvent>(SettingsButtonClicked);
        EventManager.Instance.AddListener<SaveSettingsButtonClickedEvent>(SaveSettingsButtonClicked);
        EventManager.Instance.AddListener<CloseSettingsButtonClickedEvent>(CloseSettingsButtonClicked);

        //Animation
        EventManager.Instance.AddListener<CallFadeInAnimationPanelEvent>(CallFadeInAnimationPanel);

        //Player
        EventManager.Instance.AddListener<ObjectHasBeenDestroyEvent>(ObjectHasBeenDestroy);
    }
	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

        //MainMenuManager
        EventManager.Instance.RemoveListener<SettingsButtonClickedEvent>(SettingsButtonClicked);
        EventManager.Instance.RemoveListener<SaveSettingsButtonClickedEvent>(SaveSettingsButtonClicked);
        EventManager.Instance.RemoveListener<CloseSettingsButtonClickedEvent>(CloseSettingsButtonClicked);

        //Animation
        EventManager.Instance.RemoveListener<CallFadeInAnimationPanelEvent>(CallFadeInAnimationPanel);

        //Player
        EventManager.Instance.RemoveListener<ObjectHasBeenDestroyEvent>(ObjectHasBeenDestroy);
    }
    #endregion

    #region Callbacks to GameManager events
    protected override void GameMenu(GameMenuEvent e)
    {
        m_MenuMusic.Play();
        m_LevelMusic.Stop();
    }

    protected override void GamePlay(GamePlayEvent e)
    {
        StartCoroutine(StartFadeOutIn(m_MenuMusic, m_LevelMusic));
    }

    protected override void GamePause(GamePauseEvent e)
    {
        m_LevelMusic.Pause();
    }

    protected override void GameResume(GameResumeEvent e)
    {
        m_LevelMusic.Play();
    }

    protected override void GameVictory(GameVictoryEvent e)
    {
        m_LevelMusic.Play();
    }

    protected override void GameOver(GameOverEvent e)
    {
        m_LevelMusic.Play();
    }

    void CallFadeInAnimationPanel(CallFadeInAnimationPanelEvent e)
    {
        StartCoroutine(StartFadeOut(m_LevelMusic));
    }
    #endregion

    #region Callbacks to MenuManager events
    void SettingsButtonClicked(SettingsButtonClickedEvent e)
    {
        m_ClickSound.Play();
    }

    void SaveSettingsButtonClicked(SaveSettingsButtonClickedEvent e)
    {
        m_ClickSound.Play();
    }

    void CloseSettingsButtonClicked(CloseSettingsButtonClickedEvent e)
    {
        m_ClickSound.Play();
    }
    #endregion

    #region Callbacks to Player events
    void ObjectHasBeenDestroy(ObjectHasBeenDestroyEvent e)
    {
        m_PickupSound.Play();
    }
    #endregion

    IEnumerator StartFadeOutIn(AudioSource audioSourceOut, AudioSource audioSourceIn)
    {
        float currentTime1 = 0;
        float currentTime2 = 0;
        float start1 = audioSourceOut.volume;
        float start2 = 0;

        while (currentTime1 < .25f)
        {
            currentTime1 += Time.deltaTime;
            audioSourceOut.volume = Mathf.Lerp(start1, 0f, currentTime1 / .25f);
            yield return null;
        }
        audioSourceOut.Stop();
        audioSourceOut.volume = start1;
        audioSourceIn.Play();
        while (currentTime2 < .75f)
        {
            currentTime2 += Time.deltaTime;
            audioSourceIn.volume = Mathf.Lerp(start2, 0.5f, currentTime2 / .75f);
            yield return null;
        }
        yield break;
    }

    IEnumerator StartFadeOut(AudioSource audioSourceOut)
    {
        float currentTime1 = 0;
        float start1 = audioSourceOut.volume;

        while (currentTime1 < .75f)
        {
            currentTime1 += Time.deltaTime;
            audioSourceOut.volume = Mathf.Lerp(start1, 0f, currentTime1 / .75f);
            yield return null;
        }
        audioSourceOut.Stop();
        audioSourceOut.volume = start1;
        yield break;
    }
}