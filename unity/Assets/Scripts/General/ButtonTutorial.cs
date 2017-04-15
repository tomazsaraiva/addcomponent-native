using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTutorial : MonoBehaviour 
{
	[SerializeField]
	private string _tutorialLink;

	public void OnButtonPressed()
	{
		Application.OpenURL (_tutorialLink);
	}
}