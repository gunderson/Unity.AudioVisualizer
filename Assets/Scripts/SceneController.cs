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
		public float Delay;
		public string Name;
		public bool Triggered;
		public float Duration;
		public float CueTime;
		
		public CuePoint(string Name, float Delay, float Duration)
		{
			this.Triggered = false;
			this.Delay = Delay;
			this.Name = Name;
			this.Duration = Duration;
			this.CueTime = 0;
		}
	}
		
	public List<CuePoint> CuePoints;
	
	public float StartTime;
	
	// Use this for initialization
	void Start () {
		StartTime = 0;
		CuePoints = new List<CuePoint> ();
		MGrid = Grid.GetComponent<GridMediator> ();
		MAudio = Audio.GetComponent<AudioMediator> ();
		MCamera = GameObject.Find("CameraHolder").GetComponent<CameraMediator> ();

		MakeCuePoints ();
	}
	
	// Update is called once per frame
	void Update () {
		TriggerCuePoints ();
		
		if (Input.GetKey("escape")){
			Application.Quit();

		}
	}

	void TriggerCuePoints(){
		float CurrentTime = Time.time - StartTime;
		float PrevTime = CurrentTime - Time.deltaTime;
		foreach (CuePoint c in CuePoints) {
			if (c.CueTime > PrevTime && c.CueTime <= CurrentTime && c.Triggered == false){
				TriggerCuePoint(c);
				CuePoints.Remove(c);
				break;
			}
		}
		
	}

	void TriggerCuePoint(CuePoint c){
		c.Triggered = true;
		switch (c.Name) {
		case "Start":
			MAudio.MainAudioSource.Play ();
			
			break;
		case "Move 0":
			// sweep up to top
			MCamera.TweenPosition (new Vector3 (0, 4, -32), c.Duration);
			MCamera.TweenRotation (new Vector3 (5, 0, 0), c.Duration);
			break;

		case "Move 1":
			// outside upper angle
			MCamera.TweenPosition (new Vector3 (0, 10, -45), c.Duration);
			MCamera.TweenRotation (new Vector3 (5, 0, 0), c.Duration);
			MGrid.TweenBeatScale(new Vector3 (1 << 11,0,0), c.Duration);
			break;
			
		case "Move 2":
			// back nome
			MCamera.TweenPosition (new Vector3 (0, 0, 0), c.Duration);
			MCamera.TweenRotation (new Vector3 (0, 0, 0), c.Duration);
			MGrid.TweenBeatScale(new Vector3 (1 << 14,0,0), c.Duration);
			break;
			
		case "Move 3":
			// down the cone
			MCamera.TweenPosition (new Vector3 (0, 2, 0), c.Duration);
			MCamera.TweenRotation (new Vector3 (90, 0, 0), c.Duration);
			break;

		case "Move 4":
			// pull out wide
			MCamera.TweenPosition (new Vector3 (0, 200, 0), c.Duration);
			MCamera.TweenRotation (new Vector3 (90, 0, 0), c.Duration);
			break;
			
		case "Move 5":
			// pull out wider
			MCamera.TweenPosition (new Vector3 (0, 85, 0), c.Duration);
			MCamera.TweenRotation (new Vector3 (90, 0, 0), c.Duration);
			break;

		case "Move 6":
			// come down to the side
			MCamera.TweenPosition (new Vector3 (0, 30, -40), c.Duration);
			MCamera.TweenRotation (new Vector3 (90 - 36.86f, 0, 0), c.Duration);
			break;

		case "Move 7":
			// rotate prisms
//			MGrid.PrismRotation = new Vector3(0, 0, 45);
			// move 1/8 around column
			MCamera.TweenPosition (new Vector3 (-10, 30, -30), c.Duration);
			MCamera.TweenRotation (new Vector3 (90 - 36.86f, 45, 0), c.Duration);
			break;
		case "Move 8":
			// finish move
			MCamera.TweenPosition (new Vector3 (-40, 30, 0), c.Duration);
			MCamera.TweenRotation (new Vector3 (90 - 36.86f, 90, 0), c.Duration);
			break;

		case "Move 9":
			// back home
			MCamera.TweenPosition (new Vector3 (0, 0, 0), c.Duration);
			MCamera.TweenRotation (new Vector3 (0, 0, 0), c.Duration);
			break;
		}
	}

	void MakeCuePoints(){
		setupCuePoint (new CuePoint("Start" , 0,  0 ));
		setupCuePoint (new CuePoint("Move 0", 17, 50)); 
		setupCuePoint (new CuePoint("Move 1", 0, 44)); 
		setupCuePoint (new CuePoint("Move 2", 12, 32));
		setupCuePoint (new CuePoint("Move 3", 0, 20)); // start time + (duration + delay) + (duration + delay)
		setupCuePoint (new CuePoint("Move 4", 12, 70));
		setupCuePoint (new CuePoint("Move 5", 3, 30));
		setupCuePoint (new CuePoint("Move 6", 12, 30));
		setupCuePoint (new CuePoint("Move 7", 3,  10));
		setupCuePoint (new CuePoint("Move 8", 0,  10));
		setupCuePoint (new CuePoint("Move 9", 12, 30));
	}

	private CuePoint setupCuePoint(CuePoint c){
		if (CuePoints.Count > 0) {
			c.CueTime = CuePoints [CuePoints.Count - 1].CueTime + CuePoints [CuePoints.Count - 1].Duration + c.Delay;
		} else {
			c.CueTime = c.Delay;
		}
		CuePoints.Add(c);
		return c;
	}
}
