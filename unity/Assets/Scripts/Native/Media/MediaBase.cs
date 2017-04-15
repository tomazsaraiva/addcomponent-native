using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaBase : MonoBehaviour 
{
	public delegate void OnNewPictureHandler (bool success, string path, Sprite sprite);

	private OnNewPictureHandler _callback;

	private const int MAX_COUNT = 3;
	private int _count;

	private int _maxSize = -1;


	public virtual void CameraPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log("Camera Picture | " + "filename: " + filename + ", " + "maxSize: " + maxSize);
		#endif

		_callback = callback;
		_maxSize = maxSize;
		_count = MAX_COUNT;
	}

	public virtual void GalleryPicture (string filename, OnNewPictureHandler callback, int maxSize = -1)
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log("Gallery Picture | " + "filename: " + filename + ", " + "maxSize: " + maxSize);
		#endif

		_callback = callback;
		_maxSize = maxSize;
		_count = MAX_COUNT;
	}


	protected void PictureCallback(bool success, string path)
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log("Picture Callback | " + "success: " + success + ", " + "path: " + path);
		#endif

		if (success && _count > 0)
		{
			LoadPicture (path);
		}
		else
		{
			PictureLoaded (false, null, null);
		}
	}


	private void LoadPicture (string path)
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log ("Load Picture | " + "path: " + path + ", " + "_count: " + _count);
		#endif

		StartCoroutine (LoadPictureCoroutine (path));
	}

	private IEnumerator LoadPictureCoroutine (string path)
	{
		_count--;

		#if !UNITY_EDITOR && !UNITY_WEBGL
		path = "file://" + path;
		#endif

		WWW www = new WWW (path);

		yield return www;

		if (www.size == 0)
		{
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.Log("Load Picture Coroutine FAILED | " + "www.size: " + www.size);
			#endif

			yield return new WaitForSeconds (1.0f);

			LoadPicture (path);
		}
		else if (!string.IsNullOrEmpty (www.error))
		{
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.Log("Load Picture Coroutine FAILED | " + "www.error: " + www.error);
			#endif

			PictureLoaded (false, null, null);
		}
		else
		{
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.Log("Load Picture Coroutine SUCCESS");
			#endif

			Texture2D texture = new Texture2D(www.texture.width, www.texture.height, TextureFormat.RGB24, false);

			texture.anisoLevel = 1; 
			texture.filterMode = FilterMode.Bilinear; 
			texture.wrapMode = TextureWrapMode.Clamp; 

			www.LoadImageIntoTexture (texture);

			Sprite picture = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), Vector2.zero, 1);

			PictureLoaded (true, path, picture);
		}	
	}

	private void PictureLoaded(bool success, string path, Sprite sprite)
	{
		#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log("Picture Loaded | " + "success: " + success + ", " + "path: " + path + ", " + " sprite: " +  sprite);
		#endif

		if(_callback != null)
		{
			_callback.Invoke (success, path, sprite);
			_callback = null;
		}
		else 
		{
			#if UNITY_EDITOR || DEVELOPMENT_BUILD
			Debug.Log("Picture Callback is NULL");
			#endif
		}
	}
}