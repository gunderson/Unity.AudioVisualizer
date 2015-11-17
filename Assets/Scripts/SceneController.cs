using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {

	GridMediator Grid;
	AudioMediator Audio;

	// Use this for initialization
	void Start () {
		Grid = GameObject.Find ("Grid").GetComponent<GridMediator>();
		Audio = GameObject.Find ("AudioController").GetComponent<AudioMediator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
