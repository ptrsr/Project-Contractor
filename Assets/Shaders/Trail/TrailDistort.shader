Shader "custom/TrailDistortion"
{
	Properties {

	}

	SubShader
	{
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
			uniform float4 player;
			uniform float4 smooth;
			uniform float4 lazysmooth;
			
			v2f vert (appdata v)
			{	
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{









				fixed4 scene = tex2D(_scene, i.uv);
				return scene;
			}
			ENDCG
		}
	}
}
