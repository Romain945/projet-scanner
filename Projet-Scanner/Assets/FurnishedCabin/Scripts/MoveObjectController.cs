using UnityEngine;
using System.Collections;
using SDD.Events;

public class MoveObjectController : SimpleGameStateObserver, IEventHandler
{
	float m_reachRange = 1.1f;			

	Animator anim;
	Camera fpsCam;
	GameObject player;

	const string animBoolName = "isOpen_Obj_";

	bool playerEntered;
	bool showInteractMsg;
	GUIStyle guiStyle;
	string msg;

	int rayLayerMask;
	bool m_NoMoreUI = false;

	#region Events subscription
	public override void SubscribeEvents()
	{
		base.SubscribeEvents();

		EventManager.Instance.AddListener<TimeIsUpEvent>(TimeIsUp);
	}
	public override void UnsubscribeEvents()
	{
		base.UnsubscribeEvents();

		EventManager.Instance.RemoveListener<TimeIsUpEvent>(TimeIsUp);
	}
	#endregion

	void Start()
	{
		m_NoMoreUI = false;

		//Initialize moveDrawController if script is enabled.
		player = GameObject.FindGameObjectWithTag("Player");
		fpsCam = Camera.main;

		if (fpsCam == null)	//a reference to Camera is required for rayasts
		{
			Debug.LogError("A camera tagged 'MainCamera' is missing.");
		}

		//create AnimatorOverrideController to re-use animationController for sliding draws.
		anim = GetComponent<Animator>(); 
		anim.enabled = false;  //disable animation states by default.  

		//the layer used to mask raycast for interactable objects only
		LayerMask iRayLM = LayerMask.NameToLayer("InteractRaycast");
		rayLayerMask = 1 << iRayLM.value;  

		//setup GUI style settings for user prompts
		setupGui();

	}
		
	void OnTriggerEnter(Collider other)
	{		
		if (other.gameObject == player)		//player has collided with trigger
		{			
			playerEntered = true;

		}
	}

	void OnTriggerExit(Collider other)
	{		
		if (other.gameObject == player)		//player has exited trigger
		{			
			playerEntered = false;
			//hide interact message as player may not have been looking at object when they left
			showInteractMsg = false;		
		}
	}



	void Update()
	{		
		if (playerEntered)
		{
			//center point of viewport in World space.
			Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(0.5f,0.5f,0f));
			RaycastHit hit;

			//if raycast hits a collider on the rayLayerMask
			if (Physics.Raycast(rayOrigin,fpsCam.transform.forward, out hit, m_reachRange, rayLayerMask))
			{
				MoveableObject moveableObject = null;
				//is the object of the collider player is looking at the same as me?
				if (!isEqualToParent(hit.collider, out moveableObject))
				{	//it's not so return;
					return;
				}
					
				if (moveableObject != null)		//hit object must have MoveableDraw script attached
				{
					showInteractMsg = true;
					string animBoolNameNum = animBoolName + moveableObject.objectNumber.ToString();

					bool isOpen = anim.GetBool(animBoolNameNum);	//need current state for message.
					msg = getGuiMsg(isOpen);

					if (Input.GetKeyUp(KeyCode.E))
					{
						if(!isOpen)
							EventManager.Instance.Raise(new DoorHasBeenOpenEvent());
						else
							EventManager.Instance.Raise(new DoorHasBeenCloseEvent());

						anim.enabled = true;
						anim.SetBool(animBoolNameNum,!isOpen);
						msg = getGuiMsg(!isOpen);
					}

				}
			}
			else
			{
				showInteractMsg = false;
			}
		}

	}

	//is current gameObject equal to the gameObject of other.  check its parents
	private bool isEqualToParent(Collider other, out MoveableObject draw)
	{
		draw = null;
		bool rtnVal = false;
		try
		{
			int maxWalk = 6;
			draw = other.GetComponent<MoveableObject>();

			GameObject currentGO = other.gameObject;
			for(int i=0;i<maxWalk;i++)
			{
				if (currentGO.Equals(this.gameObject))
				{
					rtnVal = true;	
					if (draw== null) draw = currentGO.GetComponentInParent<MoveableObject>();
					break;			//exit loop early.
				}

				//not equal to if reached this far in loop. move to parent if exists.
				if (currentGO.transform.parent != null)		//is there a parent
				{
					currentGO = currentGO.transform.parent.gameObject;
				}
			}
		} 
		catch (System.Exception e)
		{
			Debug.Log(e.Message);
		}
			
		return rtnVal;

	}
		

	#region GUI Config
	//configure the style of the GUI
	private void setupGui()
	{
		guiStyle = new GUIStyle();
		guiStyle.fontSize = 16;
		guiStyle.fontStyle = FontStyle.Bold;
		guiStyle.normal.textColor = Color.white;
		msg = "Press E to Open";
	}

	private string getGuiMsg(bool isOpen)
	{
		string rtnVal;
		if (isOpen)
		{
			rtnVal = "Press E to Close";
		}else
		{
			rtnVal = "Press E to Open";
		}

		return rtnVal;
	}

	void OnGUI()
	{
		if (showInteractMsg && !m_NoMoreUI)  //show on-screen prompts to user for guide.
		{
			GUI.Label(new Rect (50,Screen.height - 50,200,50), msg,guiStyle);
		}
	}
    //End of GUI Config --------------
    #endregion
	void TimeIsUp(TimeIsUpEvent e)
    {
		m_NoMoreUI = true;
	}
}
