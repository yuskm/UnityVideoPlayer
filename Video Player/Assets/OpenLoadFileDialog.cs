using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class OpenLoadFileDialog : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool Open(ref string path) {
		string selectPath = EditorUtility.OpenFilePanel( "Select a file.", "", "mp4");
		if( selectPath.Length == 0 ) {
			return false;
		}
		path = selectPath;
		return true;
	}
}
