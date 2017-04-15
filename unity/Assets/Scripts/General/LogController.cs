using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogController : MonoBehaviour 
{
	private const string LOG_SUCCESS = "<color=green>{0}: </color> {1}";
	private const string LOG_FAILED = "<color=red>{0}: </color> {1}";


	[SerializeField]
	private Text _label;

	[SerializeField]
	private ScrollRect _scrollRect;


	#region MONOBEHAVIOUR

	public void Awake ()
	{
		Application.logMessageReceived += HandleLog;	
	}

	public void OnDestroy()
	{
		Application.logMessageReceived -= HandleLog;
	}

	#endregion	


	private void HandleLog (string logString, string stackTrace, LogType type)
	{
		Log (logString);
	}

	private void Log (string message)
	{
		_label.text += message + "\n\n";

		_scrollRect.verticalNormalizedPosition = 0;
	}


	public void Log(string title, string message, bool success = true)
	{
		Log(string.Format (success ? LOG_SUCCESS : LOG_FAILED, title, message));
	}

	public void Clear()
	{
		_label.text = "";

		_scrollRect.verticalNormalizedPosition = 0;
	}
}