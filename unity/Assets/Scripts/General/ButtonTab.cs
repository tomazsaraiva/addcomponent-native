//
//  ButtonTab.cs
//
//  Author:
//       Tomaz Saraiva <tomaz.saraiva@gmail.com>
//
//  Copyright (c) 2017 Tomaz Saraiva
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonTab : MonoBehaviour
{
	[SerializeField]
	private Text _label;

	[SerializeField]
	private GameObject _goDivider;

	[SerializeField]
	private Color _colorSelect;

	[SerializeField]
	private Color _colorDisable;


	public void Select(bool select)
	{
		_label.color = select ? _colorSelect : _colorDisable;
		_goDivider.SetActive (select);
	}
}