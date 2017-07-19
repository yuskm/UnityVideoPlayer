using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour {

	private int mTotalStep;		// 16 step sequencer
	private int mCounter;		// current step

	private List<bool> mStepState;

	// step 毎にコールされる
	// <delay> sec ずらして、action すること
	public void OnStep(float delay) {
		if (mStepState [mCounter]) {
			AudioSource audioSource = GetComponent<AudioSource> ();
			audioSource.PlayDelayed (delay);
		}
		if (++mCounter >= mTotalStep) {
			mCounter = 0;
		}
	}

	// step sequencer の state を view に 渡す際に利用
	public List<bool> GetStepState() {
		return mStepState;
	}
	// step sequencer の state を view が変更する際に利用する
	public void SetStepState(int idx, bool val) {
		mStepState[ idx ] = val;    
	}
		
	public void SetupClip(AudioClip audioClip) {   
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = audioClip;
	}

	// Use this for initialization
	void Start () {
		mTotalStep = 16;
		mCounter = 0;
		mStepState = new List<bool> (mTotalStep);
		for (int i = 0; i < mTotalStep; i++) {
			mStepState.Add (false);
		}
	}
	
	// Update is called once per frame
	void Update () {
	}
}
