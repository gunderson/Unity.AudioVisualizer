using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SceneController : MonoBehaviour {

	public GridMediator MGrid;
	public AudioMediator MAudio;
	public CameraMediator MCamera;

	public GameObject Grid;
	public GameObject Audio;
	public GameObject CameraHolder;

	public struct CuePoint{
		public float CueTime;
		public string Name;
		public bool Triggered;
		
		public CuePoint(float CueTime, string Name)
		{
			this.Triggered = false;
			this.CueTime = CueTime;
			this.Name = Name;
		}
	}
		
	public List<CuePoint> CuePoints;
	
	public float StartTime;
	
	// Use this for initialization
	void Start () {
		StartTime = 0;
		CuePoints = new List<CuePoint> ();
		MGrid = Grid.GetComponent<GridMediator> ();
		MAudio = Grid.GetComponent<AudioMediator> ();
		MCamera = GameObject.Find("CameraHolder").GetComponent<CameraMediator> ();

		contstructCuePoints ();
	}
	
	// Update is called once per frame
	void Update () {
		TriggerCuePoints ();
	}

	void TriggerCuePoints(){
		float CurrentTime = Time.time - StartTime;
		float PrevTime = CurrentTime - Time.deltaTime;

		for (int i = 0; i < CuePoints.Count; i++) {
			CuePoint c = CuePoints[i];
			if (c.CueTime > PrevTime && c.CueTime <= CurrentTime && c.Triggered == false){
				Debug.Log ("TRIGGER");
				TriggerCuePoint(c);
				CuePoints.Remove(c);
				continue;
			}
		}
	}

	void TriggerCuePoint(CuePoint c){
		c.Triggered = true;
		switch (c.Name) {
		case "Start":
			break;
		case "First Move":
			Debug.Log (Time.time);
			MCamera.TweenPosition (new Vector3 (0, 20, 0), 40);
			MCamera.TweenRotation (new Vector3 (90, 0, 0), 40);
			break;
		}
	}

	void contstructCuePoints(){
		CuePoints.Add(new CuePoint(0, "Start"));
		CuePoints.Add(new CuePoint(5, "First Move"));
	}
}
