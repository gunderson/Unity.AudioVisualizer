using UnityEngine;
using System.Collections;

public class MarkerMediator : MonoBehaviour{

	public float friction = 0.95f;
	public float scaleRatio = 0;

	public Vector3 scaleDest;

	private GameObject Prism;
	private Material PrismMaterial;

	public bool DisplayvalueAffectsScaleX = false;
	public bool DisplayvalueAffectsScaleY = true;
	public bool DisplayvalueAffectsScaleZ = false;


	// ----------------------------------------
	// Color Transitions

	private float _alphaRatio = 1;
	public float alphaRatio{
		get{
			return _alphaRatio;
		}
		set{
			_alphaRatio = value;

			_SetAlpha();
		}
	}

	private float _prevAlpha = 0;
	private float _alpha = 0.04f;
	public float alpha{
		get{
			return _alpha;
		}
		set{
			_prevAlpha = _alpha;
			_alpha = value;

			_SetAlpha();
		}
	}

	private void _SetAlpha(){
		// update alpha mix
		if (_alpha != _prevAlpha && _alphaRatio != 0 && _alphaRatio != 1) {
			PrismMaterial.SetFloat ("_Alpha", Mathf.Lerp (_prevAlpha, _alpha, _alphaRatio));
		} else if (_alphaRatio == 0) {
			PrismMaterial.SetFloat ("_Alpha", _prevAlpha);
		} else if (_alphaRatio == 1) {
			PrismMaterial.SetFloat ("_Alpha", _alpha);
		}
	}
	
	private float _colormapRatio = 0;
	public float colormapRatio{
		get{
			return _colormapRatio;
		}
		set{
			_colormapRatio = value;
		}
	}

	private Vector2 _prevColormapPosition = new Vector2();
	private Vector2 _colormapPosition = new Vector2();
	public Vector2 colormapPosition{
		get{
			return _colormapPosition;
		}
		set{
			_prevColormapPosition = _colormapPosition;
			_colormapPosition = value;
		}
	}

	private float _colormapPositionRatio = 0;
	public float colormapPositionRatio{
		get{
			return _colormapPositionRatio;
		}
		set{
			_colormapPositionRatio = value;
		}
	}

	// End Color Transitions
	// ----------------------------------------
	// Position Transitions

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
			
			_positionStart = _positionDest;
			_positionDest = value;
		}
	}
	
	// End Position Transitions
	// ----------------------------------------
	// Rotation Transitions

	
	private float _rotationRatio = 0;
	public float rotationRatio{
		get{
			return _rotationRatio;
		}
		set{
			_rotationRatio = value;
		}
	}

	private Vector3 _rotationStart = new Vector3();
	private Vector3 _rotationDest = new Vector3();
	public Vector3 rotationDest{
		get{
			return _rotationDest;
		}
		set{
			_rotationStart = _rotationDest;
			_rotationDest = value;
		}
	}
	
	// End Rotation Transitions
	// ----------------------------------------


	
	// Use this for initialization
	void Start (){
		Prism = gameObject.transform.Find("Prism").gameObject;
		PrismMaterial = Prism.GetComponent<Renderer> ().material;
	}
	
	// Update is called once per frame
	void Update (){
		// update position
		// update rotation
		// update colormap position
		// update colormap mix


		Vector3 newScale = new Vector3 ();
		newScale.x = DisplayvalueAffectsScaleX ? gameObject.transform.localScale.x * friction : 1;
		newScale.y = DisplayvalueAffectsScaleY ? gameObject.transform.localScale.y * friction : 1;
		newScale.z = DisplayvalueAffectsScaleZ ? gameObject.transform.localScale.z * friction : 1;
		gameObject.transform.localScale = newScale;
	}


	public Vector3 GridPosition = new Vector3(0,0,0);
	public Vector3 Home = new Vector3(0,0,0);
	public Vector3 Offset = new Vector3(0,0,0);
	public int index = 0;

	public float displayValue {
		set {
			Vector3 newScale = new Vector3 ();
			newScale.x = DisplayvalueAffectsScaleX && value > gameObject.transform.localScale.x ? value : gameObject.transform.localScale.x;
			newScale.y = DisplayvalueAffectsScaleY && value > gameObject.transform.localScale.y ? value : gameObject.transform.localScale.y;
			newScale.z = DisplayvalueAffectsScaleZ && value > gameObject.transform.localScale.z ? value : gameObject.transform.localScale.z;
			gameObject.transform.localScale = newScale;

		}
	}

	// change color

}

