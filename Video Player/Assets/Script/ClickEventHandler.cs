using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//長押し制御用 script
public class ClickEventHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	public float  mValidSec = 3.0f;  		//長押しとして認識する時間（これより長い時間で長押しとして認識する）

	private float mRequiredTime;			// 長押し認識時刻（この時刻を超えたら長押しとして認識する）
	private bool  mIsPressing; 	            // 押下中フラグ（単一指のみの取得としても利用）

//	private GameObject 	mStepTogglePanel;	// step button 用 panel
//	private GameObject	mPlayerControl;

	////////////////////////////////////////////////////////////////////////
	// 一定時間経過後に step button 消去
//	IEnumerator StepButtonErase(float time) {
//		yield return new WaitForSeconds (time);
//		mStepTogglePanel.SetActive (false);
//	}

	////////////////////////////////////////////////////////////////////////
	void Start()
	{
		mIsPressing = false;
//ymiya[
//初期化処理
//		mStepTogglePanel = GameObject.Find("Canvas").gameObject.transform.Find("StepButtonPanel").gameObject;
//		mStepTogglePanel.SetActive (false);
//		mPlayerControl = GameObject.Find("PlayerControl").gameObject;
//ymiya]
	}

	////////////////////////////////////////////////////////////////////////
	void Update()
	{
		if (mIsPressing)  //はじめに押した指のみとなる
		{
			if (mRequiredTime < Time.time)   //一定時間過ぎたら認識
			{
				//コールバックイベント
				mIsPressing = false;           //長押し完了したら無効にする
			}
		}
	}
		
	////////////////////////////////////////////////////////////////////////
	public void OnPointerDown(PointerEventData data)
	{
		if (!mIsPressing) {
			mIsPressing = true;
			mRequiredTime = Time.time + mValidSec;
		} else {
			mIsPressing = false; 
		}
	}

	////////////////////////////////////////////////////////////////////////
	public void OnPointerUp(PointerEventData data)
	{
		if (mIsPressing) {          //はじめに押した指のみとなる
			mIsPressing = false;
		}
	}

	//UI領域から外れたら無効にする
	public void OnPointerExit(PointerEventData data)
	{
		if (mIsPressing) {          //はじめに押した指のみとなる
			mIsPressing = false;   //領域から外れたら無効にする
		}
	}
}
