using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour 
{
	void Awake()
	{
		Screen.fullScreen = false;
	}

	void Start()
	{
		SceneManager.LoadScene (1);
	}
}