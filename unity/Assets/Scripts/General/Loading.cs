using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour 
{
	[SerializeField]
	private float _speed;

	void Update ()
	{
		transform.Rotate (Vector3.forward * -_speed * Time.deltaTime);
	}
}