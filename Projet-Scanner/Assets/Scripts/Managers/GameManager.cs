using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SDD.Events;

public enum GameState { mainMenu, levelSelection, nextLevel, play, pause, levelComplete, victory, gameOver }

public class GameManager : Manager<GameManager>
{
    GameState m_GameState;
    public bool IsPlaying { get { return m_GameState == GameState.play; } }

    #region Time
    void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }
    #endregion

    #region Score & disc
    int m_Score = 0;
    public int Score { get { return m_Score; } }

    void IncrementScore(int increment)
    {
        SetScore(m_Score + increment);
    }
    void SetScore(int score)
    {
        m_Score = score;
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eScore = m_Score});
    }
    #endregion

    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        Menu();
        EventManager.Instance.Raise(new GameStatisticsChangedEvent() { eScore = 0});
        yield break;
    }
    #endregion

    #region Game flow & Gameplay
    void InitNewLevel()
    {
        SetScore(0);

        m_GameState = GameState.nextLevel;
        EventManager.Instance.Raise(new GoToNextLevelEvent() {});
    }
    #endregion

    #region Events' subscription
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();

        //MainMenuManager
        EventManager.Instance.AddListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.AddListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.AddListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.AddListener<EscapeButtonClickedEvent>(EscapeButtonClicked);

        //LevelsManager
        EventManager.Instance.AddListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);

        //Levels
        EventManager.Instance.AddListener<LastObjectHasBeenDestroyEvent>(LastObjectHasBeenDestroy);

        //HudManager
        EventManager.Instance.AddListener<TimeIsUpEvent>(TimeIsUp);

        //Animation
        EventManager.Instance.AddListener<PanelFadeInIsCompleteEvent>(PanelFadeInIsComplete);
        EventManager.Instance.AddListener<PanelFadeOutIsCompleteEvent>(PanelFadeOutIsComplete);

        //Player
        EventManager.Instance.AddListener<ObjectHasBeenDestroyEvent>(ObjectHasBeenDestroy);
    }
    public override void UnsubscribeEvents()
    {
        base.SubscribeEvents();

        //MainMenuManager
        EventManager.Instance.RemoveListener<MainMenuButtonClickedEvent>(MainMenuButtonClicked);
        EventManager.Instance.RemoveListener<PlayButtonClickedEvent>(PlayButtonClicked);
        EventManager.Instance.RemoveListener<ResumeButtonClickedEvent>(ResumeButtonClicked);
        EventManager.Instance.RemoveListener<EscapeButtonClickedEvent>(EscapeButtonClicked);

        //LevelsManager
        EventManager.Instance.RemoveListener<LevelHasBeenInstantiatedEvent>(LevelHasBeenInstantiated);

        //Levels
        EventManager.Instance.RemoveListener<LastObjectHasBeenDestroyEvent>(LastObjectHasBeenDestroy);

        //HudManager
        EventManager.Instance.RemoveListener<TimeIsUpEvent>(TimeIsUp);

        //Animation
        EventManager.Instance.RemoveListener<PanelFadeInIsCompleteEvent>(PanelFadeInIsComplete);
        EventManager.Instance.RemoveListener<PanelFadeOutIsCompleteEvent>(PanelFadeOutIsComplete);

        //Player
        EventManager.Instance.RemoveListener<ObjectHasBeenDestroyEvent>(ObjectHasBeenDestroy);
    }
    #endregion

    #region Callbacks to events issued by PanelAnimation
    void PanelFadeInIsComplete(PanelFadeInIsCompleteEvent e)
    {
        if (m_GameState == GameState.levelComplete)
        {
            if (LevelsManager.Instance && LevelsManager.Instance.IsLastLevel)
                Victory();
        }
        else if (m_GameState == GameState.gameOver)
            GameOver();
    }
    void PanelFadeOutIsComplete(PanelFadeOutIsCompleteEvent e)
    {
        m_GameState = GameState.play;
    }
    #endregion

    #region Callbacks to events issued by LevelsManager
    void LevelHasBeenInstantiated(LevelHasBeenInstantiatedEvent e)
    {
        SetTimeScale(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventManager.Instance.Raise(new GamePlayEvent());
        EventManager.Instance.Raise(new CallFadeOutAnimationPanelEvent());
    }
    #endregion

    #region Callbacks to events issued by Level
    void LastObjectHasBeenDestroy (LastObjectHasBeenDestroyEvent e)
    {
        m_GameState = GameState.levelComplete;
        EventManager.Instance.Raise(new CallFadeInAnimationPanelEvent());
    }
    #endregion

    #region Callbacks to events issued by HudManager
    void TimeIsUp (TimeIsUpEvent e)
    {
        m_GameState = GameState.gameOver;
        EventManager.Instance.Raise(new CallFadeInAnimationPanelEvent());
    }
    #endregion

    #region Callbacks to Events issued by MenuManager
    void MainMenuButtonClicked(MainMenuButtonClickedEvent e)
    {
        Menu();
    }

    void PlayButtonClicked(PlayButtonClickedEvent e)
    {
        Play();
    }

    void ResumeButtonClicked(ResumeButtonClickedEvent e)
    {
        Resume();
    }
    void EscapeButtonClicked(EscapeButtonClickedEvent e)
    {
        if (IsPlaying)
            Pause();
    }
    #endregion

    #region Callbacks to Events issued by PlayerManager
    void ObjectHasBeenDestroy (ObjectHasBeenDestroyEvent e)
    {
        IncrementScore(1);
    }
    #endregion

    #region GameState methods
    void Menu()
    {
        SetTimeScale(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_GameState = GameState.mainMenu;
        EventManager.Instance.Raise(new GameMenuEvent());
    }

    void Play()
    {
        InitNewLevel();
    }

    void Pause()
    {
        SetTimeScale(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_GameState = GameState.pause;
        EventManager.Instance.Raise(new GamePauseEvent());
    }

    void Resume()
    {
        SetTimeScale(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_GameState = GameState.play;
        EventManager.Instance.Raise(new GameResumeEvent());
    }

    void Victory()
    {
        SetTimeScale(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_GameState = GameState.victory;
        EventManager.Instance.Raise(new GameVictoryEvent());
    }

    void GameOver()
    {
        SetTimeScale(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        m_GameState = GameState.gameOver;
        EventManager.Instance.Raise(new GameOverEvent());
    }
    #endregion
}