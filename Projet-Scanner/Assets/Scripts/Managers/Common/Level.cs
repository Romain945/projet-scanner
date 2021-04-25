using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDD.Events;
using System.Linq;

public class Level : MonoBehaviour, IEventHandler
{
	#region Events' subscription
	public void SubscribeEvents()
	{
	}
	public void UnsubscribeEvents()
	{
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
}