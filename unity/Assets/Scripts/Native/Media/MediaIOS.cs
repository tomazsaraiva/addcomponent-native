using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MediaIOS : MediaBase 
{
	#region UNITY_IOS

	[DllImport ("__Internal")]
	private static extern bool _HasGalleryPermission (string callback, string gameObject);

	[DllImport ("__Internal")]
	private static extern bool _HasCameraPermission (string callback, string gameObject);


	[DllImport ("__Internal")]
	private static extern void _SelectPicture (string filename, string callback, string gameObject);

	[DllImport ("__Internal")]
	private static extern void _TakePicture (string filename, string callback, string gameObject);


	private const string METHOD_PICTURE_CALLBACK = "PictureCallbackIOS";


	public override void CameraPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		base.CameraPicture (filename, callback, maxSize);

		_TakePicture (filename, METHOD_PICTURE_CALLBACK, gameObject.name);
	}

	public override void GalleryPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		base.GalleryPicture (filename, callback, maxSize);

		_SelectPicture (filename, METHOD_PICTURE_CALLBACK, gameObject.name);
	}


	public void PictureCallbackIOS(string path) 
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log("Picture Callback IOS | " + "path: " + path);
		#endif

		PictureCallback (!string.IsNullOrEmpty (path), path);
	}


	#endregion
}