Shader "custom/caustics"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_caustics ("caustics", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

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
				float4 ray : TEXCOORD1;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float2 uvdepth : TEXCOORD1;
				float4 interpolatedRay : TEXCOORD2;
			};

			float4 _MainTex_TexelSize;
			sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
			sampler2D _caustics;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.uvdepth = v.uv.xy;

				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1 - o.uv.y;
				#endif

				o.interpolatedRay = v.ray;
				return o;
			}

			float4 _origin;

			fixed4 frag (v2f i) : SV_Target
			{	

				fixed4 col = tex2D(_MainTex, i.uv);

				float time = _Time;
				float2 shift1 = float2(time, 0);
				float2 shift2 = float2(0.5, time);

				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uvdepth));
				float linearDepth = Linear01Depth(rawDepth);
				float4 dir = linearDepth * i.interpolatedRay;
				float3 pos = _WorldSpaceCameraPos + dir;
				fixed4 cRing = fixed4(0,0,0,0);

				float dist = distance( pos, _origin);


				if (dist < 3)
					cRing = fixed4(1,0,0,1);
					

				float2 fragUV = float2(pos.x, pos.z) / 3;


				half4 c1 = tex2D(_caustics, fragUV + shift1);
				half4 c2 = tex2D(_caustics, fragUV + shift2);

				fixed4 c = (c1*5) * (c2*5);

				return col + c;
			}
			ENDCG
		}
	}
}
