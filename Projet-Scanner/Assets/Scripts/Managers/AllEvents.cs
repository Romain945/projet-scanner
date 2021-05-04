using UnityEngine;
using SDD.Events;
using System.Collections.Generic;

#region GameManager Events
public class GameMenuEvent : SDD.Events.Event
{
}
public class GamePlayEvent : SDD.Events.Event
{
}
public class GamePauseEvent : SDD.Events.Event
{
}
public class GameResumeEvent : SDD.Events.Event
{
}
public class GameVictoryEvent : SDD.Events.Event
{
}
public class GameOverEvent : SDD.Events.Event
{
}
public class GameStatisticsChangedEvent : SDD.Events.Event
{
	public int eScore { get; set; }
}
public class GameSettingsChangedEvent : SDD.Events.Event
{
	public float eMouseSensitivity { get; set; }
}
#endregion

#region MenuManager Events
public class EscapeButtonClickedEvent : SDD.Events.Event
{
}
public class PlayButtonClickedEvent : SDD.Events.Event
{
}
public class ResumeButtonClickedEvent : SDD.Events.Event
{
}
public class SaveSettingsButtonClickedEvent : SDD.Events.Event
{
}
public class SettingsButtonClickedEvent : SDD.Events.Event
{
}
public class CloseSettingsButtonClickedEvent : SDD.Events.Event
{
}
public class MainMenuButtonClickedEvent : SDD.Events.Event
{
}
#endregion

#region LevelsManager Events
public class LevelHasBeenInstantiatedEvent : SDD.Events.Event
{
	public Level eLevel;
	public int eLevelIndex;
	public Transform ePlayerTransform;
}
#endregion

#region Level Events
public class LastObjectHasBeenDestroyEvent : SDD.Events.Event
{

}
#endregion

#region HudManager Events
public class TimeIsUpEvent : SDD.Events.Event
{
}
#endregion

#region GameManager other Events
public class GoToNextLevelEvent : SDD.Events.Event
{
}
public class GoToSpecificLevelEvent : SDD.Events.Event
{
	public int eLevelIndex;
}
public class CallFadeInAnimationPanelEvent : SDD.Events.Event
{
}
public class CallFadeOutAnimationPanelEvent : SDD.Events.Event
{
}
#endregion

#region PanelAnimation Events
public class PanelFadeInIsCompleteEvent : SDD.Events.Event
{
}
public class PanelFadeOutIsCompleteEvent : SDD.Events.Event
{
}
#endregion

#region PickUp Object Events
public class CanPickupAnObjectEvent : SDD.Events.Event
{
	public GameObject ePickupObject;
}
public class CantPickupAnObjectEvent : SDD.Events.Event
{
	public GameObject ePickupObject;
}
#endregion

#region Player Events
public class ObjectHasBeenDestroyEvent : SDD.Events.Event
{
	public GameObject ePickupObject;
}
#endregion