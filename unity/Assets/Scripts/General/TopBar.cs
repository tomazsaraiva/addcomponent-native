using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBar : MonoBehaviour 
{
	[SerializeField]
	private ButtonTab[] _buttonTabs;

	[SerializeField]
	private GameObject[] _tabs;


	private int _index;


	public void EnableTab(int index)
	{
		_buttonTabs [_index].Select (false);
		_tabs [_index].SetActive (false);

		_index = index;

		_buttonTabs [_index].Select (true);
		_tabs [_index].SetActive (true);
	}
}