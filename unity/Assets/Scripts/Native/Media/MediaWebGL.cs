//
//  MediaWebGL.cs
//
//  Author:
//       Tomaz Saraiva <tomaz.saraiva@gmail.com>
//
//  Copyright (c) 2017 Tomaz Saraiva
using UnityEngine;
using System.Collections;

public class MediaWebGL : MediaBase
{
	#if UNITY_WEBGL

	// params: audioEnabled, maxSize, gameObject, callback
	private const string WEB_ENABLE_CAMERA = "enableCamera"; 

	// params: maxSize, gameObject, callback
	private const string WEB_SELECT_IMAGE = "selectImage"; 


	private const string METHOD_PICTURE_CALLBACK = "PictureCallbackWebGL";


	public override void CameraPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		base.CameraPicture (filename, callback, maxSize);

		Application.ExternalCall (WEB_ENABLE_CAMERA, false, maxSize, gameObject.name, METHOD_PICTURE_CALLBACK);
	}

	public override void GalleryPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		base.GalleryPicture (filename, callback, maxSize);

		Application.ExternalCall (WEB_SELECT_IMAGE, maxSize, gameObject.name, METHOD_PICTURE_CALLBACK);
	}

	public void PictureCallbackWebGL(string result) 
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log("Picture Callback WebGL | " + "result: " + result);
		#endif

		JSONObject json = new JSONObject (result);
		bool success = json.GetField ("success").b;
		string path = null;

		if (success)
		{
			path = json.GetField ("image").str;
		}

		PictureCallback (success, path);
	}

	#endif
}