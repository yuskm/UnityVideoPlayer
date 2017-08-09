using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CameraCapture : MonoBehaviour {
//ymiya[
	public int Width  = 1280;
	public int Height = 720;
	public int FPS    = 15;
	public int FrameRate = 30;
//ymiya]
	private bool  mIsRec;

//ymiya[ 
	public Material material;
//ymiya]

	private GameObject mCountDown;

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

		mCountDown = GameObject.Find("CountDown");
		Image countdown = mCountDown.GetComponent<Image> ();
		countdown.fillAmount = 0;
	}

	// 一定時間経過後に rec 終了
	IEnumerator StopRec(float time) {
		yield return new WaitForSeconds (time);
		mIsRec = false;	

		Microphone.End (Microphone.devices [0]);
		// microphone に録音したデータを wav file に書き出し。SavWav class 使用 (public static).
		SavWav.Save(Application.dataPath + "/../testcapture/a.wav" , GetComponent<AudioSource>().clip);
	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.Space) && !mIsRec) {
			mIsRec = true;
			StartCoroutine (StopRec (1));	// 最大 １秒間 キャプチャ可能

			Image countdown = mCountDown.GetComponent<Image> ();
			countdown.fillAmount = 0;

			GetComponent<AudioSource>().clip = Microphone.Start(Microphone.devices [0], false, 3/*temp*/, 44100);	// 音声録音開始
		}
		if (Input.GetKeyUp (KeyCode.Space) && mIsRec) {
			mIsRec = false;
		}

		if (mIsRec) {
			Image countdown = mCountDown.GetComponent<Image> ();
			countdown.fillAmount += Time.deltaTime;
		}
	}

	void LateUpdate() {
		if (mIsRec) {
			if (webcamTexture != null) {
				StartCoroutine(SaveToPNGFile (webcamTexture.GetPixels (), Application.dataPath + "/../testcapture/" + Time.frameCount + "SavedScreen.png" ));
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