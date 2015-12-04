using UnityEngine;
using System.Collections;

public class MotionTween : MonoBehaviour {

	public struct Tween {
		public float StartTime;
		public float Duration;
		public Vector3 Start;
		public Vector3 Dest;
		public bool isComplete; 

		public Tween(Vector3 Start, Vector3 Dest, float Duration){
			StartTime = Time.time;
			this.Start = Start;
			this.Dest = Dest;
			this.Duration = Duration;
			this.isComplete = false;
			
		}
	}

	public float BeatScale = 1 >> 11;

	private Tween _PositionTween;
	public void TweenPosition(Vector3 Dest, float Duration){
		_PositionTween = new Tween(gameObject.transform.position, Dest, Duration);
	}
	
	private Tween _RotationTween;
	public void TweenRotation(Vector3 Dest, float Duration){
		_RotationTween = new Tween(gameObject.transform.rotation.eulerAngles, Dest, Duration);
	}
	
	private Tween _BeatScaleTween;
	public void TweenBeatScale(Vector3 Dest, float Duration){
		_BeatScaleTween = new Tween(new Vector3(BeatScale,0,0), Dest, Duration);
	}

	// Use this for initialization
	public virtual void Start () {
		_PositionTween = new Tween ();
		_PositionTween.isComplete = true;
		
		_RotationTween = new Tween ();
		_RotationTween.isComplete = true;

		_BeatScaleTween = new Tween ();
		_BeatScaleTween.isComplete = true;
	}
	
	// Update is called once per frame
	public virtual void Update () {
		float cur = 0;
		if (!_PositionTween.isComplete) {
			cur = (Time.time - _PositionTween.StartTime) / _PositionTween.Duration;
			if (cur >= 1){
				cur = 1;
				_PositionTween.isComplete = true;
			}
			gameObject.transform.position = Vector3.Lerp(_PositionTween.Start, _PositionTween.Dest, cur);
		}
		
		if (!_RotationTween.isComplete) {
			cur = (Time.time - _RotationTween.StartTime) / _RotationTween.Duration;
			if (cur >= 1){
				cur = 1;
				_RotationTween.isComplete = true;
			}
			gameObject.transform.rotation =  Quaternion.Euler(Vector3.Lerp(_RotationTween.Start, _RotationTween.Dest, cur));
		}

		if (!_BeatScaleTween.isComplete) {
			cur = (Time.time - _BeatScaleTween.StartTime) / _BeatScaleTween.Duration;
			if (cur >= 1){
				cur = 1;
				_BeatScaleTween.isComplete = true;
			}
			BeatScale =  Vector3.Lerp(_BeatScaleTween.Start, _BeatScaleTween.Dest, cur)[0];
		}
	}
}
