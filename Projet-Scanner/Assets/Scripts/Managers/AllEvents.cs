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
	public float eSensitivity { get; set; }
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