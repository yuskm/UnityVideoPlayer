using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonControl : MonoBehaviour {
	private List<GameObject> mStepToggle;
	private GameObject mPlayerControl;

	// Use this for initialization
	void Start () {
		GameObject stepToggleRsc = (GameObject)Resources.Load("StepToggle");
		mPlayerControl = GameObject.Find("PlayerControl");

		mStepToggle = new List<GameObject>(16);

		for (int i = 0; i < 16; i++) {
			GameObject stepToggle = Instantiate (stepToggleRsc, new Vector3(-300+i*30, 200, 0), Quaternion.identity);
			stepToggle.transform.SetParent (transform.Find("StepTogglePanel"),false);
			mStepToggle.Add(stepToggle);

			Toggle stepToggleA = stepToggle.GetComponent<Toggle>();
			stepToggleA.isOn = false;

			this.AddToggleEvent(stepToggleA, i);

			mStepToggle[i].SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}

	// 動的にイベントを追加するために用意した。
	// Start() で 直接 AddListener したら、コールバックの引数が不正となった。(Start() Local の idx 変数がセットされた。)
	void AddToggleEvent(Toggle toggle, int idx) {
		toggle.onValueChanged.AddListener ((value) => {
			this.ToggleEvent(idx);
		});
	}
		
	// button 押した際の イベント
	public void ToggleEvent(int idx) {
		Debug.Log("ToggleEvent ;" + idx);
		Toggle stepToggleA = mStepToggle[ idx ].GetComponent<Toggle>();
		bool val = stepToggleA.isOn;

		PlayerControl playerControl = mPlayerControl.GetComponent<PlayerControl> ();
		playerControl.SetStepState (idx, val);
	}


//ymiya[
// とりあえず、でっち上げ
// button を active にするタイミングで、current track の state に更新する。
// current track を切り替えるたびに、このメンバー関数をコールしなければならない。
	public void SetToggleOn(List<bool> val) {
		if (mStepToggle == null) {
			return;
		}
			
		for (int i = 0; i < mStepToggle.Count && i < val.Count; i++) {
			Toggle stepToggleA = mStepToggle[ i ].GetComponent<Toggle>();
			stepToggleA.isOn = val[ i ];
		}
	}
//ymiya ]

//ymiya[
// button を配置した canvas を active / inactive にすれば良いのではないか？
// これ必要か？
	public void SetToggleActive(bool active) {
		if (mStepToggle == null) {
			return;
		}

		for (int i = 0; i < mStepToggle.Count; i++) {
			mStepToggle[i].SetActive (active);
		}
	}
//ymiya ]

}
