using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraCapture : MonoBehaviour {
	public int Width  = 1280;
	public int Height = 720;
	public int FPS    = 15;
	public int FrameRate = 30;

	private bool mIsRec;

	public Material material;

	WebCamTexture webcamTexture;

	void Start () {
		mIsRec = false;

		Time.captureFramerate = FrameRate;

		WebCamDevice[] devices = WebCamTexture.devices;

		// display all cameras
		for (var i = 0; i < devices.Length; i++) {
			print (i + ":" + devices[i].name);
		}

		webcamTexture = new WebCamTexture(devices[0].name, Width, Height, FPS);
		GetComponent<Renderer> ().material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}

	// 一定時間経過後に rec 終了
	IEnumerator StopRec(float time) {
		yield return new WaitForSeconds (time);
		mIsRec = false;	
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space)) {
			mIsRec = true;
			StartCoroutine (StopRec (1));
		}
	}

	void LateUpdate() {
		if (mIsRec) {
			if (webcamTexture != null) {
				StartCoroutine(SaveToPNGFile (webcamTexture.GetPixels (), Application.dataPath + "/../" + Time.frameCount + "SavedScreen.png" ));
			}
		}
	}

	IEnumerator SaveToPNGFile( Color[] texData , string filename ) {
		yield return new WaitForEndOfFrame();

		Texture2D takenPhoto = new Texture2D(Width, Height, TextureFormat.ARGB32, false);
		takenPhoto.SetPixels(texData);
		takenPhoto.Apply();

		byte[] png = takenPhoto.EncodeToPNG();
		Destroy(takenPhoto);

		// For testing purposes, also write to a file in the project folder
		File.WriteAllBytes(filename, png);
	}
}