Shader "custom/seaweed" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_shift ("Shift", float) = 0
		_speed ("Speed", float) = 0
		_intensity ("Intensity", float) = 0
	}
	SubShader {
		Cull Off
//		Tags { "Rendertype" = "Opaque"}
		Tags { "Rendertype" = "Grass"}
		CGPROGRAM
		#pragma surface surf Standard fullForwardShadows vertex:vert addshadow
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
		};

		uniform sampler2D _MainTex;

		float _shift;
		float _speed;
		float _intensity;

		uniform half _Glossiness;
		uniform half _Metallic;
		uniform fixed4 _Color;

		void vert( inout appdata_base v) {	
			float time = _Time;
			float mask = min(v.vertex.y / 2, 1);
			float4 objectOrigin = mul(unity_ObjectToWorld, float4(0,0,0,0));
			// push vertex positions into world space to get proper offset
			float4 worldVert = mul(unity_ObjectToWorld, v.vertex);
			worldVert.x += (sin(time * _speed + worldVert.y * _shift) * _intensity) *mask;
			// revert offset back into objectspace position
			v.vertex = mul(unity_WorldToObject, worldVert);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			clip(c.a-0.01);
			o.Albedo = c.rgb;

			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}