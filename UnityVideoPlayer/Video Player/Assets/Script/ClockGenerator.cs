using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockGenerator : MonoBehaviour {

	public float mBpm;

	private int 		mLowestFPS;
	private float 		mNextClock;
	private GameObject	mPlayerControl;

	// Use this for initialization
	void Start () {
		mBpm = 120.0f;
		mLowestFPS = 20;
		mNextClock = 0.0f;
		mPlayerControl = GameObject.Find("PlayerControl");
	}
	
	// Update is called once per frame
	void Update () {
		if ( Time.time + 1.0 / mLowestFPS > mNextClock) {
			long delay = GetComponent<AudioSource>().clip.samples - GetComponent<AudioSource>().timeSamples;
			mPlayerControl.GetComponent<PlayerControl> ().OnStep ((float)delay/44100.0f);
			mNextClock += (60.0f / mBpm / 4 ); 	// 16 beat =  60 sec / bpm(4beat) / 4
		}
	}
}
