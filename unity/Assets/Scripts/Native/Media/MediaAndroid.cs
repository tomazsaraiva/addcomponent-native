//
//  NativeGallery.cs
//
//  Author:
//       Tomaz Saraiva <tomaz.saraiva@gmail.com>
//
//  Copyright (c) 2017 Tomaz Saraiva
using UnityEngine;
using System.Collections;

public class MediaAndroid : MediaBase
{
	#if UNITY_ANDROID

	private const string GALLERY_PACKAGE = "com.addcomponent.gallery";
	private const string GALLERY_CLASS = ".GalleryFragment";
	private const string GALLERY_METHOD_GALLERY_SELECT = "selectImage";

	private const string CAMERA_PACKAGE = "com.addcomponent.camera";
	private const string CAMERA_CLASS = ".CameraFragment";
	private const string CAMERA_METHOD_TAKE_PICTURE = "takePicture";

	private const string METHOD_PICTURE_CALLBACK = "PictureCallbackAndroid";


	public override void CameraPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		base.CameraPicture (filename, callback, maxSize);

		using (AndroidJavaObject unityPlayer = new AndroidJavaObject (CAMERA_PACKAGE + CAMERA_CLASS))
		{ 
			unityPlayer.Call (CAMERA_METHOD_TAKE_PICTURE, gameObject.name, filename, METHOD_PICTURE_CALLBACK);
		} 
	}

	public override void GalleryPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		base.GalleryPicture (filename, callback, maxSize);

		using (AndroidJavaObject unityPlayer = new AndroidJavaObject (GALLERY_PACKAGE + GALLERY_CLASS))
		{
			unityPlayer.Call (GALLERY_METHOD_GALLERY_SELECT, gameObject.name, METHOD_PICTURE_CALLBACK, filename);
		} 
	}


	public void PictureCallbackAndroid (string result)
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log("Picture Callback Android | " + "result: " + result);
		#endif

		PictureCallback (!string.IsNullOrEmpty (result), result);
	}

	#endif
}