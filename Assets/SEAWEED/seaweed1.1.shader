Shader "custom/seaweed1.1" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MetalRough ("Metallic Roughness", 2D) = "" {}
		_Normal ("Normal", 2D) = "" {}
		_shift ("Shift", float) = 0
		_speed ("Speed", float) = 0
		_intensity ("Intensity", float) = 0
	}
	SubShader {
		Cull Off
		Tags { "Rendertype" = "Opaque"}
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

		uniform sampler2D _MetalRough;
		uniform sampler2D _Normal;
		uniform fixed4 _Color;

		void vert( inout appdata_full v) {	
			float time = _Time;
			float mask = min(v.vertex.y / 2, 1);

			// push vertex positions into world space to get proper offset
			float4 worldVert = mul(unity_ObjectToWorld, v.vertex);
			worldVert.x += (sin(time * _speed + worldVert.y * _shift + worldVert.x / 5) * _intensity) * mask;
			// revert offset back into objectspace position
			v.vertex = mul(unity_WorldToObject, worldVert);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			clip(c.a - 0.01);
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal (tex2D(_Normal, IN.uv_MainTex));

			half4 MetalRough = tex2D(_MetalRough, IN.uv_MainTex);
			o.Metallic = MetalRough.rgb;
			o.Smoothness = 1 - MetalRough.a;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}