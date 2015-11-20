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

	void Awake() {
		MarkerPrefab = Resources.Load ("Marker") as GameObject;
	}

	// Use this for initialization
	void Start (){
		audioMediator = GameObject.Find ("AudioController").GetComponent<AudioMediator> ();
		PopulateSpiral ();
	}
	
	// Update is called once per frame
	void Update (){
		for (int i = 0, endi = ActiveMarkers.Count; i<endi; i++) {
			MarkerMediator m = ActiveMarkers[i];
			float scale = 1 << 13;//16 + 1024 * m.GridPosition.z / GridSize.z;
			m.displayValue = audioMediator.FFTBuffer[0][(numMarkers - 1) - i] * scale;

		}
	}

	public void SetValues(float [][] FFTBuffer){
		int row = FFTBuffer.Length;
		while (--row >= 0) {
			float[] colBuffer = FFTBuffer [row];
			int col = colBuffer.Length;
			while (--col >= 0) {
				ActiveMarkers[(colBuffer.Length * row) + col].gameObject.transform.localScale = new Vector3(1,4*colBuffer[col], 1);
			}
		}
	}
	
	
	private void PopulateSpiral(){




		ActiveMarkers.AddRange (GetMarkers (numMarkers * numLayers));
	}
	
	private MarkerMediator[] GetMarkers(int count){
		MarkerMediator[] NewMarkers = new MarkerMediator[count];

		float finalRadius = Mathf.Sqrt (numMarkers + spiralStartIndex);

		Vector3 origin = new Vector3 (0, 0, 0);

		for (int i = 0; i < count; i++){
			NewMarkers[i] = MakeMarker();
			NewMarkers[i].index = i;

			float angle = (float)(i * 2.399963229728653); //Golden angle relative to TWO_PI

			Vector3 pos = new Vector3(Mathf.Cos(angle) * Mathf.Sqrt(i + spiralStartIndex),0,Mathf.Sin(angle) * Mathf.Sqrt(i + spiralStartIndex));

			//NewMarkers[i].Offset = Vector3.Scale (new Vector3(Random.value, Random.value, Random.value), OffsetRadius);
			NewMarkers[i].Home = pos;

			NewMarkers[i].gameObject.transform.position = NewMarkers[i].Home ;
			NewMarkers[i].gameObject.transform.localScale = new Vector3(1,0,1);
			NewMarkers[i].gameObject.transform.LookAt(origin);
			

			Vector2 PositionRatio = new Vector2(NewMarkers[i].gameObject.transform.position.x / finalRadius, NewMarkers[i].gameObject.transform.position.z / finalRadius);

			Transform prism = NewMarkers[i].gameObject.transform.Find("Prism");

			Renderer r = prism.GetComponent<Renderer>();
			Material m = r.material;
			m.SetVector("_Position", new Vector4(PositionRatio.x, PositionRatio.y));
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

