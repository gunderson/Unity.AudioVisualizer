using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMediator : MonoBehaviour{
	public Vector3 CellCount = new Vector3 (32, 1, 32);
	public Vector3 GridSize = new Vector3 (32, 32, 32);
	public Vector3 OffsetRadius = new Vector3 (2f, 0.0f, 2f);
	private Vector3 CellSize;
	private List<MarkerMediator> ActiveMarkers = new List<MarkerMediator> ();
	public GameObject MarkerPrefab;
	private AudioMediator audioMediator;
	
	void Awake() {
		MarkerPrefab = Resources.Load ("Marker") as GameObject;
		CellSize = new Vector3 (GridSize.x / CellCount.x, GridSize.y / CellCount.y, GridSize.z / CellCount.z);
	}

	// Use this for initialization
	void Start (){
		audioMediator = GameObject.Find ("AudioController").GetComponent<AudioMediator> ();
		PopulateGrid ();
	}
	
	// Update is called once per frame
	void Update (){
		for (int i = 0, endi = ActiveMarkers.Count; i<endi; i++) {
			MarkerMediator m = ActiveMarkers[i];
//			m.gameObject.transform.localRotation = new Vector3(0, m.GridPosition.x, 0);
			int col = (Mathf.RoundToInt(m.GridPosition.x) + audioMediator.CurrentBufferPosition) % (audioMediator.FFTBufferDepth - 1);
			int row = Mathf.RoundToInt(m.GridPosition.z * 8);
			float scale = 512;//16 + 1024 * m.GridPosition.z / GridSize.z;
			m.gameObject.transform.localScale = new Vector3(1, audioMediator.FFTBuffer[col][row] * scale, 1);
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
	
	
	private void PopulateGrid(){
		ActiveMarkers.AddRange (GetMarkers (Mathf.RoundToInt(CellCount.x * CellCount.y * CellCount.z)));
	}
	
	private MarkerMediator[] GetMarkers(int count){
		MarkerMediator[] NewMarkers = new MarkerMediator[count];
		for (int i = 0; i < count; i++){
			NewMarkers[i] = MakeMarker();
			NewMarkers[i].index = i;

			NewMarkers[i].GridPosition.x = NewMarkers[i].index % CellCount.x;
			NewMarkers[i].GridPosition.y = Mathf.Floor(NewMarkers[i].index / (CellCount.x * CellCount.z));
			NewMarkers[i].GridPosition.z = Mathf.Floor(NewMarkers[i].index / CellCount.x);

			NewMarkers[i].Offset = Vector3.Scale (new Vector3(Random.value, Random.value, Random.value), OffsetRadius);
			NewMarkers[i].Home = Vector3.Scale(NewMarkers[i].GridPosition, CellSize);

			NewMarkers[i].gameObject.transform.position = NewMarkers[i].Home + NewMarkers[i].Offset;
			NewMarkers[i].gameObject.transform.localScale = new Vector3(1,10,1);
			Vector2 PositionRatio = new Vector2(NewMarkers[i].gameObject.transform.position.x / GridSize.x, NewMarkers[i].gameObject.transform.position.z / GridSize.z);

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

