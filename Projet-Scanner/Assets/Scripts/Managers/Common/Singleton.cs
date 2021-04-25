using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
	static T m_Instance;
	public static T Instance { get { return m_Instance; } }

	protected virtual void Awake()
	{
		if (m_Instance != null)
			Destroy(gameObject);
		else m_Instance = this as T;
	}
}