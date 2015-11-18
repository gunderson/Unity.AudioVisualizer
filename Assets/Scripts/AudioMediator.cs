using UnityEngine;
using System.Collections;

public class AudioMediator : MonoBehaviour {

	private int FFTSampleDepth = 32;
	private int FFTBufferDepth = 32;

	public float[][] FFTBufferBuffer;
	public int CurrentBufferPosition = 0;
	public AudioSource MainAudioSource;

	// Use this for initialization
	void Start () {
		// load audio file
		FFTBufferBuffer = AudioMediator.MakeFlatBufferBuffer(FFTBufferDepth, FFTSampleDepth);
		MainAudioSource = gameObject.GetComponent<AudioSource> ();
//		MainAudioSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		float[] b = new float[32];
//		MainAudioSource.GetSpectrumData (b, 1, FFTWindow.Rectangular);
//		CurrentBufferPosition = OffsetIndex (CurrentBufferPosition, 1, FFTBufferBuffer.Length);
	}
	
	// loops an incrementer so that 
	public int OffsetIndex(int current, int offset, int length){
		int newBufferPosition = (current + offset) % length;
		newBufferPosition = newBufferPosition >= 0 ? newBufferPosition : newBufferPosition + length;
		return newBufferPosition;
	}

	public static float[][] MakeFlatBufferBuffer(int w, int h){
		float[][] output = new float[w][];
		while(w > 0){
			output[--w] = AudioMediator.MakeArray(h);
		}
		return output;
	}

	public static float[] MakeArray(int i){
		float[] output = new float[i];
		while(i > 0){
			output[--i] = 0;
		}
		return output;
	}

}

