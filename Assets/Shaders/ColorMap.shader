Shader "Custom/Colormap" {
	Properties {
		_ColorMap0 ("Color Map 0", 2D) = "" {}
		_ColorMap1 ("Color Map 1", 2D) = "" {}
		_ColorMapBlend ("Color Map Blend", Float) = 0.0
		_SpeedX ("Color Map Speed X", Float) = 0.0
		_SpeedY ("Color Map Speed Y", Float) = 0.0
		_Alpha0 ("Alpha", Float) = 1.0
		_Position ("Color Map Position Ratio", Vector) = (0,0,0,0)
	}

    SubShader {
    	Tags { "Queue" = "Transparent" } 
        Pass {
				
			// Cull Off // Draw front and back faces
			ZWrite Off // Don't write to depth buffer 
			// in order not to occlude other objects

			Blend One One // Additive blending
			
            CGPROGRAM
            
	            #pragma vertex vert
	            #pragma fragment frag
	            
	            float _Alpha0;
	            float _ColorMapBlend;
	            float _SpeedX;
	            float _SpeedY;
	            float4 _Position;
				sampler2D _ColorMap0;
				sampler2D _ColorMap1;
	            
	            // vertex input: position, second UV
		        struct appdata {
		            float4 vertex : POSITION;
		            float4 texcoord1 : TEXCOORD1;
		        };

		        struct v2f {
		            float4 pos : SV_POSITION;
		        };
		        
		        v2f vert (appdata v) {
		            v2f o;
		            o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
		            return o;
		        }

	            fixed4 frag() : SV_Target {
	            	fixed2 pixelPos = _Position + fixed2(_SpeedX * _Time[0], _SpeedY * _Time[0]);
	            	fixed4 c0 = tex2D(_ColorMap0, pixelPos);
	            	fixed4 c1 = tex2D(_ColorMap1, pixelPos);
	            	fixed4 c = lerp(c0,c1, _ColorMapBlend) * _Alpha0;
	                return c;
	            }
	            
            ENDCG
        }
    }
}