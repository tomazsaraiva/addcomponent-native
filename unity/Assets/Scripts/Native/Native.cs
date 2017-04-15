//
//  NativeManager.cs
//
//  Author:
//       Tomaz Saraiva <tomaz.saraiva@gmail.com>
//
//  Copyright (c) 2017 Tomaz Saraiva
using UnityEngine;
using System.Collections;
using System;

public class Native : MonoBehaviour
{
	#region Singleton

	private static Native _instance;

	public static Native Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance =  new GameObject ("Native").AddComponent <Native>();
			}

			return _instance;
		}
	}

	#endregion

	#region MEDIA

	private MediaBase _media;

	void Awake()
	{
		DontDestroyOnLoad (gameObject);

		#if UNITY_EDITOR
		_media = gameObject.AddComponent <MediaStandalone>();
		#elif UNITY_ANDROID
		_media = gameObject.AddComponent <MediaAndroid>();
		#elif UNITY_IOS
		_media = gameObject.AddComponent <MediaIOS>();
		#elif UNITY_WEBGL
		_media = gameObject.AddComponent <MediaWebGL>();
		#else
		Debug.LogError("NOT IMPLEMENTED");
		#endif
	}

	public void CameraPicture (string filename, MediaBase.OnNewPictureHandler callback, int maxSize = -1)
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log("Camera Picture | " + "filename: " + filename + ", " + "maxSize: " + maxSize);
		#endif

		_media.CameraPicture (filename, callback, maxSize);
	}

	public void GalleryPicture (string filename, MediaBase.OnNewPictureHandler callback, int maxSize = -1)
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log("Gallery Picture | " + "filename: " + filename + ", " + "maxSize: " + maxSize);
		#endif

		_media.GalleryPicture (filename, callback, maxSize);
	}

	#endregion
}