using UnityEngine;
using System.Collections;

public class MarkerMediator : MonoBehaviour{

	public float friction = 0.95f;
	public float scaleRatio = 0;

	public Vector3 scaleDest;

	private GameObject Prism;

	public enum Mode {
		Stream,
		Grid,
		Wash,
	};

	private float _colormapRatio = 0;
	public float colormapRatio{
		get{
			return _colormapRatio;
		}
		set{
			_colormapRatio = value;
		}
	}

	private float _positionRatio = 0;
	public float positionRatio{
		get{
			return _positionRatio;
		}
		set{
			_positionRatio = value;
		}
	}
	
	private Vector3 _positionStart = new Vector3();
	private Vector3 _positionDest = new Vector3();
	public Vector3 positionDest{
		get{
			return _positionDest;
		}
		set{
			_positionDest = value;
		}
	}

	private Vector3 _rotationStart = new Vector3();
	private Vector3 _rotationDest = new Vector3();
	public Vector3 rotationDest{
		get{
			return _rotationDest;
		}
		set{
			_rotationDest = value;
		}
	}
	
	// Use this for initialization
	void Start (){
		Prism = gameObject.transform.Find("Prism").gameObject;
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

