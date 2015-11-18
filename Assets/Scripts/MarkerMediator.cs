using UnityEngine;
using System.Collections;

public class MarkerMediator : MonoBehaviour{

	private float friction = 0.8f;

	// Use this for initialization
	void Start (){
	}
	
	// Update is called once per frame
	void Update (){
		Vector3 newScale = new Vector3 (1, gameObject.transform.localScale.y * friction, 1);
		gameObject.transform.localScale = newScale;
	}


	public Vector3 GridPosition = new Vector3(0,0,0);
	public Vector3 Home = new Vector3(0,0,0);
	public Vector3 Offset = new Vector3(0,0,0);
	public int index = 0;

	public float displayValue {
		set {
			if (value > gameObject.transform.localScale.y){
				gameObject.transform.localScale = new Vector3(1,value,1);
			}
		}
	}

	// change color

}

