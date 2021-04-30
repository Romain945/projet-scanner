using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDD.Events;

public class SettingsManager : Manager<SettingsManager>
{
    [Header("Settings Value")]
    [SerializeField] float m_MouseSensitivity;

    #region Manager implementation
    protected override IEnumerator InitCoroutine()
    {
        EventManager.Instance.Raise(new GameSettingsChangedEvent() { eMouseSensitivity = m_MouseSensitivity });
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
}