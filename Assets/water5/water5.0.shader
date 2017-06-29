Shader "custom/water5.0"
{
	Properties 
  	{
	  	_Color ("Water Color", Color) = (1,1,1,1)
	  	_Refract ("Refraction Normal", 2D) = "" {}
	  	_threshold ("highlight", Range(0,1)) = 1
	  	_intensity ("intensity", Range(0,1)) = 1
	  	_distortion ("Distortion", Range(0,20)) = 1
	  	_depth ("Depth", Range(0,100)) = 1
	  	_waterScale ("Water Scale", Float) = 1
  	}

  	SubShader {
   		Tags { "Queue" = "Geometry+1" }
		GrabPass { "_background" }
    	Pass { 	
    		Cull Off ZWrite On
      		CGPROGRAM
      		#pragma target 3.0
      		#pragma vertex vert
      		#pragma fragment frag
      		#include "UnityCG.cginc"

	      	struct appdata {
	      		float4 vertex : POSITION;
	      		float2 uv : TEXCOORD0;
	      		float3 normal : NORMAL;
	      	};
	   		
	      	struct v2f 
	      	{
	        	float4 pos : SV_POSITION;
	        	float4 screenpos : TEXCOORD1;
	        	float2 uv : TEXCOORD0;
	        	float3 viewdir : FLOAT;
	        	float3 normal : NORMAL;
	        	float3 worldPosition : FLOAT1;

	      	};

	      	v2f vert(appdata v)
	      	{         
	        	v2f o;
	        	o.pos = UnityObjectToClipPos(v.vertex);
	        	o.screenpos = ComputeScreenPos(o.pos);
	        	o.uv = v.uv;
	        	o.normal = v.normal;

	        	// calculating viewdirection on vertex
	        	o.viewdir = normalize(WorldSpaceViewDir(v.vertex));
	        	o.worldPosition = mul(unity_ObjectToWorld, v.vertex);

	        	return o;
	      	}

	      	uniform sampler2D _CameraDepthTexture;
	      	uniform sampler2D _background;

	      	uniform sampler2D _Refract;
	      	uniform float _distortion;
	      	uniform float _depth;
	      	uniform float _intensity;
	      	uniform float _threshold;
	      	uniform half4 _Color;

	      	uniform float _waterScale;

	      	fixed4 frag(v2f i) : COLOR 
	      	{	
	      		float time = _Time;
	      		float4 shift = float4(0, time, time, 0.5);
	
	      		// calculating normals and refraction
				float3 n1 = UnpackNormal(tex2D(_Refract, i.uv*_waterScale + shift.xy));
				float3 n2 = UnpackNormal(tex2D(_Refract, i.uv*_waterScale + shift.zw));
				float3 normals = (n1 + n2)*0.5;
				normals -= 1;
				float3 refraction = normals * _distortion;

				// calculate refracted screen position and depth
				float4 screen = float4(i.screenpos.x + refraction.x, i.screenpos.yzw);
				float sceneZ = LinearEyeDepth(DecodeFloatRG(tex2Dproj(_CameraDepthTexture, screen)));
				float fragZ = screen.z;

				// mask out refraction for objects above water
				float refmask = step(fragZ, sceneZ);
				refraction *= refmask;
				screen = float4(i.screenpos.x + refraction.x, i.screenpos.yzw);

				// scene depth with masked refraction
            	sceneZ = LinearEyeDepth(DecodeFloatRG(tex2Dproj(_CameraDepthTexture, screen)));
            	float sceneZ01 = Linear01Depth(DecodeFloatRG(tex2Dproj(_CameraDepthTexture, screen)));

            	// scene projection with masked refraction
	      		half4 background = tex2Dproj(_background, screen);

	      		// world position of scene fragment
	      		float3 view = normalize(i.worldPosition - _WorldSpaceCameraPos);
	      		float3 dir = sceneZ * view;
				float3 worldPos = _WorldSpaceCameraPos + dir;

				// depth from fragz to scenez
	            float waterDepth = saturate((sceneZ - fragZ)/_depth) * _intensity;

	            // create lighting
				float light = dot(normals, float3(0,-1,0)) * 0.5 + 0.5;
				light *= pow(light, 1.5);
				light = step(light, _threshold);
//				if(light > _threshold)
//					return float(1);


				// create output
				half4 o = background;

				// draw output
//				o = lerp(o, _Color, waterDepth);
//				o += light;

				// return output
	            return o;
	     		
	      	}
      	ENDCG
    	}
	}
}