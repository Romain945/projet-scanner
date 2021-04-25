using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class SettingsManager : Manager<MenuManager>
{
    [Header("Sensitivity Slider")]
    [SerializeField] Slider m_SensitivitySlider;
    [SerializeField] Text m_SensitivityValueText;

    [Header("Settings Value")]
    [SerializeField] float m_MouseSensitivity;

    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        m_SensitivitySlider.value = m_MouseSensitivity;
        m_SensitivityValueText.text = m_MouseSensitivity.ToString();
        EventManager.Instance.Raise(new GameSettingsChangedEvent() { eSensitivity = m_MouseSensitivity });
        yield break;
    }
    #endregion

    #region Events' subscription
    public override void SubscribeEvents()
    {
        base.SubscribeEvents();

        //MainMenuManager
        EventManager.Instance.AddListener<SaveSettingsButtonClickedEvent>(SaveSettingsButtonClicked);
        EventManager.Instance.AddListener<CloseSettingsButtonClickedEvent>(CloseSettingsButtonClicked);

    }
    public override void UnsubscribeEvents()
    {
        base.UnsubscribeEvents();

        //MainMenuManager
        EventManager.Instance.RemoveListener<SaveSettingsButtonClickedEvent>(SaveSettingsButtonClicked);
        EventManager.Instance.RemoveListener<CloseSettingsButtonClickedEvent>(CloseSettingsButtonClicked);
    }
    #endregion

    #region Callbacks to Events issued by MenuManager
    void SaveSettingsButtonClicked(SaveSettingsButtonClickedEvent e)
    {
        m_MouseSensitivity = m_SensitivitySlider.value;
        EventManager.Instance.Raise(new GameSettingsChangedEvent() { eSensitivity = m_MouseSensitivity });
    }
    void CloseSettingsButtonClicked(CloseSettingsButtonClickedEvent e)
    {
        m_SensitivitySlider.value = m_MouseSensitivity;
        m_SensitivityValueText.text = m_MouseSensitivity.ToString();
    }
    #endregion

    #region UI OnClick Events
    public void SensitivitySliderValueHasBeenChanged()
    {
        m_SensitivityValueText.text = m_SensitivitySlider.value.ToString();
    }
    #endregion
}
