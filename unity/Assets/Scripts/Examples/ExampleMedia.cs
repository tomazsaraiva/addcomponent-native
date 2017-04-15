using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;

public class ExampleMedia : MonoBehaviour
{
	private const string DEFAULT_IMAGE_NAME = "picture.png";


	[SerializeField]
	private GameObject _goImageLoading;

	[SerializeField]
	private Image _imageResult;

	[SerializeField]
	private GameObject _goButtonCamera;

	[SerializeField]
	private GameObject _goButtonGallery;

	[SerializeField]
	private GameObject _goMessage;

	[SerializeField]
	private Text _labelMessage;


	private AspectRatioFitter _aspectRatioFitter;


	public void CameraPicture ()
	{
		LoadingPicture (true);

		Native.Instance.CameraPicture (DEFAULT_IMAGE_NAME, PictureLoaded, 512);
	}

	public void GalleryPicture()
	{
		LoadingPicture (true);

		Native.Instance.GalleryPicture (DEFAULT_IMAGE_NAME, PictureLoaded, 512);
	}


	private void PictureLoaded(bool success, string path, Sprite sprite)
	{
		if(success)
		{
			if(_aspectRatioFitter == null)
			{
				_aspectRatioFitter = _imageResult.GetComponent <AspectRatioFitter>();
			}

			_aspectRatioFitter.aspectRatio = (float)sprite.texture.width/(float)sprite.texture.height;

			_imageResult.sprite = sprite;

			ShowMessage("SUCCESS: " + sprite.texture.width + "x" + sprite.texture.height + "px");
		}

		LoadingPicture (false);
	}


	private void LoadingPicture(bool loading)
	{
		_imageResult.gameObject.SetActive (!loading);

		_goImageLoading.SetActive (loading);

		_goButtonCamera.SetActive (!loading);
		_goButtonGallery.SetActive (!loading);
	}
		
	private void ShowMessage(string message)
	{
		_labelMessage.text = message;
		_goMessage.SetActive (true);
	}
}