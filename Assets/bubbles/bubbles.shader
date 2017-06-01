Shader "custom/bubbles"
{
	Properties
	{
		_Refract ("Refraction Tex", 2D) = "white" {}
		_Color ("bubble color", Color) = (1,1,1,1)
		_refract ("Refraction Intensity", Range(0,5)) = 0
		_refPower ("refraction power", Range(0,5)) = 1
		_colPower ("color power", Range(0,5)) = 1
		_edge ("edge width", Range(0,1)) = 0

		_warpIntensity ("warp intensity", Range(0,0.1)) = 0
		_warpSpeed ("warp speed", Range(0,200)) = 0
		_warpSize ("warp size", Range(0,20)) = 0
	}

	SubShader
	{
		Tags { "Queue" = "Transparent" }
		Grabpass {"_background"}

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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float4 screen : TEXCOORD1;
				float3 viewDir : FLOAT;
			};

			uniform sampler2D _background;
			uniform sampler2D _Refract;
			uniform float _refract;
			uniform float _refPower;
			uniform float _colPower;
			uniform float _edge;
			uniform half4 _Color;

			uniform float _warpIntensity;
			uniform float _warpSpeed;
			uniform float _warpSize;

			v2f vert (appdata v)
			{
				v2f o;

				// random warp on world position
				float3 baseWorldPos = mul(unity_ObjectToWorld, float4(0,0,0,1));
				float worldSeed = baseWorldPos.x + baseWorldPos.y + baseWorldPos.z;

				// warp shifting
				float shift = sin(_Time * _warpSpeed + v.vertex.y * _warpSize + worldSeed) * _warpIntensity;

				o.vertex = UnityObjectToClipPos(v.vertex + float4(shift, 0,0,0));
				o.normal = v.normal;
				o.uv = v.uv;
				o.screen = ComputeScreenPos(o.vertex);

				// edge detection
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex));

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{

				// view and normal diff
				float diff = 1 - (dot(i.viewDir, i.normal) * 0.5 + 0.5);

				float high = pow( dot(_WorldSpaceLightPos0, i.normal) * 0.5 + 0.5 , 5);

				// refraction and color strengths
				float refDiff = pow(diff, 5 - _refPower);
				float colDiff = pow(diff, 5 - _colPower);
				float edge = step(1-_edge, diff);

				// refraction shifting
				float time = _Time;
				float shift = time;

				// calculating refraction
				half4 normalColor = tex2D(_Refract, i.uv);
				float3 r1 = UnpackNormal( tex2D(_Refract, i.uv * float2(1,0.5) + float2(shift, 0)) );
				float3 r2 = UnpackNormal( tex2D(_Refract, i.uv * float2(1,0.5) + float2(0, 2 * shift)) );
				float3 refraction = (r1 + r2) * 0.5;

				// drawing refracted background scene
				i.screen += float4(refraction.y, -refraction.y ,0 , 0) * _refract * refDiff;

				half4 scene = tex2Dproj(_background, i.screen);
				scene += _Color * colDiff;
				scene += float(high);
				scene += edge;

				return scene;
			}
			ENDCG
		}
	}
}