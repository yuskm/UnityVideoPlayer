using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImagePlayer : MonoBehaviour {

	[SerializeField]
	// 連番画像が保存されているディレクトリ
	public string mDirPath;     
	public int FrameRate = 30;

	// コマ表示キュー
	Queue<System.IO.FileInfo> mFileQueue;

	// 表示用テクスチャ
	Texture2D 	mTexture2D;
	RawImage 	mRawImage;

	void Start () {
		mRawImage = GetComponent<RawImage>();

		// ディレクトリ内のpngファイル一覧を取得
		//   -> キューに入れる
		mDirPath = Application.dataPath + "/../testcapture/";
		System.IO.DirectoryInfo info = new System.IO.DirectoryInfo(mDirPath);
		mFileQueue = new Queue<System.IO.FileInfo>(info.GetFiles("*.png"));

		// テクスチャオブジェクトを作成しておく
		int Width  = 1280;
		int Height = 720;
		mTexture2D = new Texture2D(Width, Height, TextureFormat.ARGB32, false);
		mRawImage.texture = Texture2D.blackTexture;

		Time.captureFramerate = FrameRate;
	}

	IEnumerator PlayImage() {
		if (mFileQueue.Count <= 0) {
			// 全部表示したので何もしない
			yield break;
		}

		// ファイルからテクスチャデータ読み込み
		System.IO.FileInfo targetImage = mFileQueue.Dequeue();
		System.IO.FileStream stream = targetImage.OpenRead();
		var data = new byte[stream.Length];
		stream.Read(data, 0, (int)stream.Length);
		mTexture2D.LoadImage(data);

		// RawImageにテクスチャとして設定
		mRawImage.texture = mTexture2D;
	}

	void Update () {
		StartCoroutine(PlayImage());
	}
}