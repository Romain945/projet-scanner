using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;

public class PanelAnimation : MonoBehaviour, IEventHandler
{
	#region Events' subscription
	public virtual void SubscribeEvents()
	{
		EventManager.Instance.AddListener<CallFadeInAnimationPanelEvent>(CallFadeInAnimationPanel);
		EventManager.Instance.AddListener<CallFadeOutAnimationPanelEvent>(CallFadeOutAnimationPanel);
	}
	public virtual void UnsubscribeEvents()
	{
		EventManager.Instance.RemoveListener<CallFadeInAnimationPanelEvent>(CallFadeInAnimationPanel);
		EventManager.Instance.RemoveListener<CallFadeOutAnimationPanelEvent>(CallFadeOutAnimationPanel);
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

	[SerializeField] Animator m_FadeAnimation;

	#region Callbacks to GameManager events
	void CallFadeInAnimationPanel(CallFadeInAnimationPanelEvent e)
	{
		m_FadeAnimation.Play("FadeIn", -1, 0f);
	}
	void CallFadeOutAnimationPanel(CallFadeOutAnimationPanelEvent e)
	{
		m_FadeAnimation.Play("FadeOut", -1, 0f);
	}
	#endregion

	public void PanelFadeInIsComplete()
	{
		EventManager.Instance.Raise(new PanelFadeInIsCompleteEvent());
	}
	public void PanelFadeOutIsComplete()
	{
		EventManager.Instance.Raise(new PanelFadeOutIsCompleteEvent());
	}
}