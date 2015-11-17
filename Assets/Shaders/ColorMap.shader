Shader "Custom/Colormap" {
	Properties {
		_ColorMap ("Color Map", 2D) = "" {}
		_Alpha ("Opacity", Float) = 1.0
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
	            
	            float _Alpha;
	            float4 _Position;
				sampler2D _ColorMap;
	            
	            float4 vert(float4 v:POSITION) : SV_POSITION {
	                return mul (UNITY_MATRIX_MVP, v);
	            }

	            fixed4 frag() : SV_Target {
	            	fixed4 c = tex2D(_ColorMap, _Position);
	            	c = c * _Alpha;
	                return c;
	            }
	            
            ENDCG
        }
    }
}