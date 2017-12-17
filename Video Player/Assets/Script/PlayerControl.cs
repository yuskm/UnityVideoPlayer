// #define USE_RAYCAST_FOR_GET_CLICK

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour {

	private const int mStepCount = 16;		// 16 step sequencer
	private const int mDisplayCount = 9;   

	private List< List<bool> > mStepState;	// 16 step x 9 display 分の step data を保存
//ymiya[2017/12/06
// - 削除予定
	private List<GameObject> mTrack;		// 9 Screen 分 の 音声 track
//ymiya]
	private List<GameObject> mScreen;		// 9 screen
	private int mCurrentDisplay;			// 操作中のdisplay番号 
	                                        // -1 は 操作中ではない。 
	private int mCurrentStep;
	private GameObject mCanvas;	// 必要 
	private GameObject mStepTogglePanel;


////////////////////////////////////////////////////////////////////////
	// 左クリックされた場所のオブジェクトを取得
	GameObject getClickObject(int key) {
		GameObject result = null;
		if ( Input.GetMouseButtonDown(key) ) {

#if USE_RAYCAST_FOR_GET_CLICK
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast(ray, out hit)){
				result = hit.collider.gameObject;
			}
#else
			PointerEventData pointer = new PointerEventData (EventSystem.current);
			pointer.position = Input.mousePosition;
			List<RaycastResult> raycastResult = new List<RaycastResult> ();
			EventSystem.current.RaycastAll (pointer, raycastResult);
			if (raycastResult.Count > 0) {
				result = raycastResult[0].gameObject;
			}
#endif
		}
		return result;
	}
	
////////////////////////////////////////////////////////////////////////
	// step sequencer の state を view に 渡す際に利用
	public List<bool> GetStepState(int idx) {
		return mTrack[mCurrentDisplay].GetComponent<Track> ().GetStepState ();
	}

////////////////////////////////////////////////////////////////////////
	// step sequencer の state を view が通知する際に利用する
	public void SetStepState(int idx, bool val) {
		mTrack[mCurrentDisplay].GetComponent<Track> ().SetStepState(idx,val);
		mScreen[mCurrentDisplay].GetComponent<StepSeq> ().SetStepState(idx,val);
//ymiya[ 2017/12/07
// - これいらないのでは？
//		mStepState [mCurrentDisplay] [idx] = val;
//ymiya]
	}
		
////////////////////////////////////////////////////////////////////////
	// 一定時間経過後に step button 消去
	// この処理はここでやるべきではない。ButtonControl(mStepTogglePanel） が無操作を判別して自分で SetActive すべき。
	// Awake で Activate を hook し、start coroutine で 消せばいいのでは？
	// ButtonControl には、ToggleEvent があるのでここで延長できるか？
	IEnumerator StepButtonErase(float time) {
		yield return new WaitForSeconds (time);
		mCurrentDisplay = -1;			// 画面の step button を非表示

//ymiya[ 2017.10.10
//canvas そのものではなく、 panel のみで制御するように変更した。
		// step 制御用のボタンが配置された canvas を非表示に。
//		Canvas canvas = mCanvas.GetComponent<Canvas> ();
//		canvas.enabled = false;
		mStepTogglePanel.SetActive (false);
//ymiya]
	}

////////////////////////////////////////////////////////////////////////
	// clock generator が step timing を通知 
	public void OnStep(float delay) {
		for (int i = 0; i < mDisplayCount; i++) {
			mTrack[i].GetComponent<Track>().OnStep(delay);
//ymiya[ 2017/12/06
// - 本来なら delay の機構をつけたい。まだない。
			mScreen[i].GetComponent<StepSeq>().OnStep();
//ymiya]
		}

		mCurrentStep++;
		if ( mCurrentStep >= 16 ) {
			mCurrentStep = 0;
		}
	}

////////////////////////////////////////////////////////////////////////
	// Use this for initialization
	void Start () {
		mCurrentDisplay = -1;	// どの display も操作中ではない
		mCurrentStep = 0;
		mStepState =  new List< List<bool> >( mDisplayCount );
		for (int i = 0; i < mDisplayCount; i++) {
			List<bool> listA = new List<bool> ( mStepCount );
			for (int j = 0; j < mStepCount; j++) {
				listA.Add (false);
			}
			mStepState.Add(listA); 
		}
//ymiya[ 2017/12/06
// - audio sequencer 入れる場合は active に
		GameObject trackRsc = (GameObject)Resources.Load("Prefab/Track");
		List<AudioClip> audioClip = new List<AudioClip>(mDisplayCount);
		audioClip.Add( (AudioClip) Resources.Load("HT",typeof(AudioClip) ) );
		mTrack = new List<GameObject>(mDisplayCount);
		for (int i = 0; i < mDisplayCount; i++) {
			GameObject track = Instantiate (trackRsc);
			track.GetComponent<Track> ().SetupClip (audioClip [0]);
			mTrack.Add (track);
		}
//ymiya]
		
		// 9 screen object on canvas
		mScreen = new List<GameObject>(mDisplayCount);
		mScreen.Add(GameObject.Find ("SplittedScreen11"));
		mScreen.Add(GameObject.Find ("SplittedScreen12"));
		mScreen.Add(GameObject.Find ("SplittedScreen13"));
		mScreen.Add(GameObject.Find ("SplittedScreen21"));
		mScreen.Add(GameObject.Find ("SplittedScreen22"));
		mScreen.Add(GameObject.Find ("SplittedScreen23"));
		mScreen.Add(GameObject.Find ("SplittedScreen31"));
		mScreen.Add(GameObject.Find ("SplittedScreen32"));
		mScreen.Add(GameObject.Find ("SplittedScreen33"));

		mCanvas = GameObject.Find("Canvas").gameObject;
		Canvas canvas = mCanvas.GetComponent<Canvas> ();
		canvas.enabled = true;

//ymiya[ 2017/12/07
// - SetActive したいので tranform 
//		mStepTogglePanel = GameObject.Find("StepButtonPanel").gameObject;
		mStepTogglePanel = mCanvas.transform.Find("StepButtonPanel").gameObject;
		mStepTogglePanel.SetActive (false);
//ymiya]

//ymiya[ 2017/12/06 
// 必要ないのでは？
//		ButtonControl buttonControl = mCanvas.GetComponent<ButtonControl> (); 
//		buttonControl.SetToggleActive (false);
//ymiya]	
	}

////////////////////////////////////////////////////////////////////////
	// Update is called once per frame
	void Update ()
	{
		// 左クリックされた Game Object 取得
		GameObject rClickObj = getClickObject (1);
		GameObject lClickObj = getClickObject (0);

		if (rClickObj != null) {
			StepSeq stepSeq = rClickObj.GetComponent<StepSeq> ();
			if (stepSeq != null) {
				int trackId = stepSeq.GetTrackId ();
				if (trackId >= 0 && trackId <= 8) { 
					OpenLoadFileDialog loadFileDlg = rClickObj.GetComponent<OpenLoadFileDialog> ();
					string filePath = "";
					if (loadFileDlg.Open (ref filePath)) {
						VideoPlayer videoPlayer = rClickObj.GetComponent<VideoPlayer> ();
						videoPlayer.url = "file://" + filePath;
//						videoPlayer.targetCameraAlpha = 0.5F;
//						videoPlayer.Play ();
					}
				}
			}
		}

		if (lClickObj != null) {
			StepSeq stepSeq = lClickObj.GetComponent<StepSeq> ();

			if (stepSeq != null) {
				int trackId = stepSeq.GetTrackId ();
				if (trackId >= 0 && trackId <= 8) { 
					mCurrentDisplay = trackId;	// 当該画面の step button を表示
					Canvas canvas = mCanvas.GetComponent<Canvas> ();
					canvas.enabled = true;
					mStepTogglePanel.SetActive (true);
					StartCoroutine (StepButtonErase (5.0F)); // 3 sec 後に step button 削除
				}
			}
		}
	}

////////////////////////////////////////////////////////////////////////
	public void SetupTrack(int index) {
//ymiya[
// とりあえず、でっち上げ
// button を active にするタイミングで、current track の state に更新する。
// current track を切り替えるたびに、このメンバー関数をコールしなければならない。
		ButtonControl buttonControl = mCanvas.GetComponent<ButtonControl> (); 
		buttonControl.SetToggleOn (GetStepState(index));
//ymiya]	
	}
}
