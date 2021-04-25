using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class MenuManager : Manager<MenuManager>
{
    #region Panels & Labels
    [Header("Panels")]
    [SerializeField] GameObject m_MainMenuPanel;
    [SerializeField] GameObject m_PausePanel;
    [SerializeField] GameObject m_SettingsPanel;
    [SerializeField] GameObject m_HudPanel;
    [SerializeField] GameObject m_VictoryPanel;
    [SerializeField] GameObject m_GameOverPanel;

    List<GameObject> m_AllPanels;

    [Header("Labels")]
    [SerializeField] Text[] m_ScoreValueText;
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
    }
    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();
    }
    #endregion

    #region Monobehaviour lifecycle
    protected override void Awake()
    {
        base.Awake();
        RegisterPanels();
    }
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            EscapeButtonHasBeenClicked();
        }
    }
    #endregion

    #region Panel Methods
    void RegisterPanels()
    {
        m_AllPanels = new List<GameObject>();
        m_AllPanels.Add(m_MainMenuPanel);
        m_AllPanels.Add(m_PausePanel); 
        m_AllPanels.Add(m_HudPanel);
        m_AllPanels.Add(m_VictoryPanel);
        m_AllPanels.Add(m_GameOverPanel);
    }
    void OpenPanel(GameObject panel)
    {
        foreach (var item in m_AllPanels)
            if (item) item.SetActive(item == panel);
    }
    #endregion

    #region Callbacks to GameManager events
    protected override void GameMenu(GameMenuEvent e)
    {
        OpenPanel(m_MainMenuPanel);
    }

    protected override void GamePlay(GamePlayEvent e)
    {
        OpenPanel(m_HudPanel);
    }

    protected override void GamePause(GamePauseEvent e)
    {
        OpenPanel(m_PausePanel);
    }

    protected override void GameResume(GameResumeEvent e)
    {
        OpenPanel(m_HudPanel);
    }

    protected override void GameVictory(GameVictoryEvent e)
    {
        OpenPanel(m_VictoryPanel);
        for (int i = 0; i < m_ScoreValueText.Length; i++)
            m_ScoreValueText[i].text = GameManager.Instance.Score.ToString();
    }

    protected override void GameOver(GameOverEvent e)
    {
        OpenPanel(m_GameOverPanel);
        for (int i = 0; i < m_ScoreValueText.Length; i++)
            m_ScoreValueText[i].text = GameManager.Instance.Score.ToString();
    }
    #endregion

    #region UI OnClick Events
    public void EscapeButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new EscapeButtonClickedEvent());
    }

    public void PlayButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new PlayButtonClickedEvent());
    }

    public void ResumeButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new ResumeButtonClickedEvent());
    }

    public void SettingsButtonHasBeenClicked()
    {
        m_SettingsPanel.SetActive(true);
    }

    public void SaveSettingsButtonHasBeenClicked()
    {
        m_SettingsPanel.SetActive(false);
        EventManager.Instance.Raise(new SaveSettingsButtonClickedEvent());
    }

    public void CloseSettingsButtonHasBeenClicked()
    {
        m_SettingsPanel.SetActive(false);
        EventManager.Instance.Raise(new CloseSettingsButtonClickedEvent());
    }

    public void MainMenuButtonHasBeenClicked()
    {
        EventManager.Instance.Raise(new MainMenuButtonClickedEvent());
    }

    public void CloseTheGameButtonHasBeenClicked()
    {
        Application.Quit();
    }
    #endregion
}