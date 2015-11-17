using UnityEngine;
using System.Collections;

namespace PG {
	public class ColorMap
	{
		private Texture2D _texture;
		private Color[] pix;

		public ColorMap(){}
		
		public ColorMap(Texture2D texture){
			this.texture = texture;
		}

		public Texture2D texture {
			get {
				return _texture;
			}
			set {
				_texture = value;
				pix = _texture.GetPixels();
			}
		}

		// when x,y are ints, treat them as the exact pixel location
		public Color getPixel(int x, int y){
			int index = (y * texture.width) + x;
			return pix[index];
		}

		// when x,y are floats, treat them as ratios
		public Color getPixel(float x, float y){
			return getPixel (Mathf.RoundToInt(x * texture.width), Mathf.RoundToInt(y * texture.height));
		}
	}
}

