//
//  MediaStandalone.cs
//
//  Author:
//       Tomaz Saraiva <tomaz.saraiva@gmail.com>
//
//  Copyright (c) 2017 Tomaz Saraiva
using UnityEngine;
using System.Collections;

public class MediaStandalone : MediaBase
{
	public override void CameraPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		base.CameraPicture (filename, callback, maxSize);

		// TODO: implement webcam texture
	}

	public override void GalleryPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		base.GalleryPicture (filename, callback, maxSize);

		string path = null;

		#if UNITY_EDITOR
		path = UnityEditor.EditorUtility.OpenFilePanel("Open image","","jpg,png,bmp");
		#else 
		// TODO: implement load file on standalone
		#endif

		PictureCallback (!string.IsNullOrEmpty (path), "file:///" + path);
	}
}