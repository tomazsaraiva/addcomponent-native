using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBack : MonoBehaviour 
{
	#region MONOBEHAVIOUR

	void Update()
	{
		#if UNITY_ANDROID
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Back();
		}
		#endif	
	}

	#endregion

	public void OnButtonPressed()
	{
		Back ();
	}

	private void Back()
	{
		SceneManager.LoadScene (1);
	}
}