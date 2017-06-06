Shader "custom/vignette"
{
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			uniform sampler2D _scene;

			uniform float _range;
			uniform float _width;
			uniform float _intensity;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float dist = distance(i.uv, float2(0.5,0.5));
				float start = _range - _width;

				float diff = saturate((dist - start) / (_range - start));
				diff = min(diff, _intensity);

				half4 scene = tex2D(_scene, i.uv);
				half4 vignette = half4(1,1,1,1);

				return scene + vignette * diff;
			}
			ENDCG
		}
	}
}
