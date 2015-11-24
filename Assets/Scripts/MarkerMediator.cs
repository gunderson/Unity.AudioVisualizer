using UnityEngine;
using System.Collections;

public class MarkerMediator : MonoBehaviour{

	public float friction = 0.95f;
	public float scaleRatio = 0;

	public Vector3 scaleDest;

	public GameObject Prism;
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
	private float _alpha = 0.07f;
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
			PrismMaterial.SetFloat ("_Alpha0", Mathf.Lerp (_prevAlpha, _alpha, _alphaRatio));
		} else if (_alphaRatio == 0) {
			PrismMaterial.SetFloat ("_Alpha0", _prevAlpha);
		} else if (_alphaRatio == 1) {
			PrismMaterial.SetFloat ("_Alpha0", _alpha);
		}
	}
	
	private float _colormapRatio = 0;
	public float colormapRatio{
		get{
			return _colormapRatio;
		}
		set{
			if (value != _colormapRatio){
				_colormapRatio = value;
				PrismMaterial.SetFloat ("_ColorMapBlend", value);
			}
		}
	}

//	private Vector2 _prevColormapPosition = new Vector2();
	private Vector2 _colormapPosition = new Vector2();
	public Vector2 colormapPosition{
		get{
			return _colormapPosition;
		}
		set{
//			_prevColormapPosition = _colormapPosition;
			_colormapPosition = value;

			PrismMaterial.SetVector("_Position", new Vector4(value.x, value.y));
			
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

	public Texture2D Colormap {
		set {
			PrismMaterial.SetTexture("_ColorMap0", PrismMaterial.GetTexture("_ColorMap1"));
			PrismMaterial.SetTexture("_ColorMap1", value);
			colormapRatio = 0;
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
	
//	private Vector3 _positionStart = new Vector3();
	private Vector3 _positionDest = new Vector3();
	public Vector3 positionDest{
		get{
			return _positionDest;
		}
		set{
			
//			_positionStart = _positionDest;
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
			if (_rotationRatio != value){
				_rotationRatio = value;
				_UpdateRotation();
			}
		}
	}

	private float _rotationStart = 0;
	private float _rotationDest = 0;
	public float rotationDest{
		get{
			return _rotationDest;
		}
		set{
			_rotationStart = _rotationDest;
			_rotationDest = value;
			if (_rotationDest != _rotationStart){
				_UpdateRotation();
			}
		}
	}

	private void _UpdateRotation(){
//		Quaternion myQuat = Quaternion.AngleAxis(Mathf.Lerp(_rotationStart, _rotationDest, _rotationRatio),Vector3.up);
		
	}
	
	// End Rotation Transitions
	// ----------------------------------------


	
	// Use this for initialization
	void Awake (){
		Prism = gameObject.transform.Find("Prism").gameObject;
		PrismMaterial = Prism.gameObject.GetComponent<Renderer> ().material;
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

