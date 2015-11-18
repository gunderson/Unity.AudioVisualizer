using UnityEngine;
using System.Collections;

public class AudioMediator : MonoBehaviour {

	public int FFTSampleDepth = 2048;
	public int FFTBufferDepth = 1;

	public float[][] FFTBuffer;
	public int CurrentBufferPosition = 0;
	public AudioSource MainAudioSource;

	// Use this for initialization
	void Start () {
		// load audio file
		FFTBuffer = AudioMediator.MakeFlatBuffer(FFTBufferDepth, FFTSampleDepth);
		MainAudioSource = gameObject.GetComponent<AudioSource> ();
		MainAudioSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		MainAudioSource.GetSpectrumData (FFTBuffer[CurrentBufferPosition], 0, FFTWindow.Rectangular);
		CurrentBufferPosition = OffsetIndex (CurrentBufferPosition, 1, FFTBuffer.Length);
	}
	
	// loops an incrementer so that 
	public int OffsetIndex(int current, int offset, int length){
		int newBufferPosition = (current + offset) % length;
		newBufferPosition = newBufferPosition >= 0 ? newBufferPosition : newBufferPosition + length;
		return newBufferPosition;
	}

	public static float[][] MakeFlatBuffer(int w, int h){
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

