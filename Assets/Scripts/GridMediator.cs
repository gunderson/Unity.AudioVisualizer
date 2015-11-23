using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMediator : MonoBehaviour{
	private List<MarkerMediator> ActiveMarkers = new List<MarkerMediator> ();
	public GameObject MarkerPrefab;
	private AudioMediator audioMediator;
	private float GoldenAngle = 135.508f;


	public int numMarkers = 2048;
	public int numLayers = 1;
	public float spiralScale = 1.0f;
	public int spiralStartIndex = 30;
	public float spiralRadius = 64f;

	public Vector3 spinRate = new Vector3();
	public Vector3 positionDuration = new Vector3();
	public float positionRatio = 0;
	public float colormapRatio = 0;

	public float BeatScale = 1 << 14;

	private float _transitionStartTime = 0;
	public float transitionDuration = 0;

	private float _prevAlpha = 0;
	public float Alpha = 0f;


	public Vector3 markerLookTarget = new Vector3();


	public enum LayoutPattern {
		Spiral,
		Cone,
		Grid,
		Sphere
	};

	private LayoutPattern _mode = LayoutPattern.Spiral;
	public LayoutPattern Mode{
		get{
			return _mode;
		}
		set{
			_mode = value;

			switch(value){
			case LayoutPattern.Spiral:
				MoveMarkersToSpiral();
				break;
				
			case LayoutPattern.Cone:
				MoveMarkersToCone();
				break;
				
			case LayoutPattern.Grid:
				MoveMarkersToGrid();
				break;
				
			case LayoutPattern.Sphere:
				MoveMarkersToSphere();
				break;
			}
		}
	}



	void Awake() {
		MarkerPrefab = Resources.Load ("Marker") as GameObject;
	}

	// Use this for initialization
	void Start (){
		audioMediator = GameObject.Find ("AudioController").GetComponent<AudioMediator> ();
		PopulateMarkers ();
		setupSpiral (ActiveMarkers);
	}
	
	// Update is called once per frame
	void Update (){
		switch(_mode){
		case LayoutPattern.Spiral:
			UpdateSpiral(audioMediator.FFTBuffer);
			break;
			
		case LayoutPattern.Cone:
			UpdateGrid(audioMediator.FFTBuffer);
			break;
			
		case LayoutPattern.Grid:
			UpdateCone(audioMediator.FFTBuffer);
			break;
			
		case LayoutPattern.Sphere:
			UpdateSphere(audioMediator.FFTBuffer);
			break;
		}

		for (int i = 0, endi = ActiveMarkers.Count; i<endi; i++) {
			MarkerMediator m = ActiveMarkers[i];
			m.positionRatio = positionRatio;
			m.colormapRatio = colormapRatio;
			if (_prevAlpha != Alpha){
				m.alpha = Alpha;
			}
		}
		
		_prevAlpha = Alpha;
	}
	
	public void MoveMarkersToSpiral(){
		for (int i = 0, endi = ActiveMarkers.Count; i<endi; i++) {
			MarkerMediator m = ActiveMarkers[i];

		}
	}

	public void MoveMarkersToCone(){
		for (int i = 0, endi = ActiveMarkers.Count; i<endi; i++) {
			MarkerMediator m = ActiveMarkers[i];
		}
	}

	public void MoveMarkersToGrid(){
		
	}

	public void MoveMarkersToSphere(){
		
	}


	public void UpdateSpiral(float[][] FFTBuffer){
		for (int i = 0, endi = ActiveMarkers.Count; i<endi; i++) {
			MarkerMediator m = ActiveMarkers[i];
			m.displayValue = audioMediator.FFTBuffer[0][(numMarkers - 1) - i] * BeatScale;
		}
	}

	public void UpdateGrid(float[][] FFTBuffer){
		
	}

	public void UpdateCone(float[][] FFTBuffer){
		
	}

	public void UpdateSphere(float[][] FFTBuffer){
		
	}

	public List<MarkerMediator> changeColormap(List<MarkerMediator> Markers, Texture2D colormap){
		for (int i = 0, endi = Markers.Count; i<endi; i++) {
			MarkerMediator m = Markers[i];
			// set colormap1 to colormap0
			// set colormap1 to colormap
			// set colormapRatio = 0;
		}
		return Markers;
	}

	public List<MarkerMediator> setColormapRatio(List<MarkerMediator> Markers, float ratio){
		for (int i = 0, endi = Markers.Count; i<endi; i++) {
			MarkerMediator m = Markers[i];
			// set colormapRatio = ratio;
		}
		return Markers;
	}
	
	private List<MarkerMediator> setupSpiral(List<MarkerMediator> Markers){

		float finalRadius = Mathf.Sqrt (numMarkers + spiralStartIndex);
		
		Vector3 origin = new Vector3 (0, 0, 0);

		for (int i = 0; i < Markers.Count; i++){
			MarkerMediator m = Markers[i];
			
			float angle = (float)(i * 2.399963229728653); //Golden angle relative to TWO_PI
			
			Vector3 pos = new Vector3(Mathf.Cos(angle) * Mathf.Sqrt(i + spiralStartIndex),0,Mathf.Sin(angle) * Mathf.Sqrt(i + spiralStartIndex));
			
			//m.Offset = Vector3.Scale (new Vector3(Random.value, Random.value, Random.value), OffsetRadius);
			m.Home = pos;
			
			m.gameObject.transform.position = m.Home ;
			m.gameObject.transform.localScale = new Vector3(1,0,1);
			m.gameObject.transform.LookAt(origin);
			
			
			Vector2 PositionRatio = new Vector2(m.gameObject.transform.position.x / finalRadius, m.gameObject.transform.position.z / finalRadius);
			
			m.colormapPosition = new Vector2(PositionRatio.x, PositionRatio.y);
		}

		return Markers;
	}
	
	private void PopulateMarkers(){
		ActiveMarkers.AddRange (GetMarkers (numMarkers * numLayers));
	}
	
	private MarkerMediator[] GetMarkers(int count){
		MarkerMediator[] NewMarkers = new MarkerMediator[count];

		for (int i = 0; i < count; i++){
			NewMarkers[i] = MakeMarker();
			NewMarkers[i].index = i;
		}
		return NewMarkers;
	}
	
	private MarkerMediator MakeMarker(){
		GameObject MarkerGameObject = Instantiate (MarkerPrefab);
		//MarkerGameObject.gameObject.transform.parent = gameObject.transform;
		MarkerMediator Marker = MarkerGameObject.GetComponent<MarkerMediator>();
		return Marker;
	}
}

