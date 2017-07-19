using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DivLinePanelScript : MonoBehaviour {

	private GameObject mPanelObject;
	private Color 	   mPanelOriginalColor;

	// Use this for initialization
	void Start () {
		this.mPanelObject = GameObject.Find("DivLinePanel");
		this.mPanelOriginalColor = mPanelObject.GetComponent<Image>().color;

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			this.mPanelObject.GetComponent<Image>().color = new Color(mPanelOriginalColor.r, mPanelOriginalColor.g, mPanelOriginalColor.b, mPanelOriginalColor.a);
		} else {
			this.mPanelObject.GetComponent<Image>().color = new Color(mPanelOriginalColor.r, mPanelOriginalColor.g, mPanelOriginalColor.b, 0.0f);		
		}
	}
}
