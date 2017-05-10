// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "custom/water4.0"
{
  Properties 
  {

  }

  SubShader 
  {
    Pass 
    { 	
	  Tags{ "LightMode" = "ShadowCaster" }
	  ZWrite On
	  ZTest LEqual

      	CGPROGRAM
      	#pragma vertex vert
      	#pragma fragment frag
      	#include "UnityCG.cginc"


      	struct appdata {
      		float4 vertex : POSITION;
      		float2 uv : TEXCOORD0;
      	};
   	
      	struct v2f 
      	{
        	float4 pos : SV_POSITION;
        	float2 uv : TEXCOORD0;
      	};
       
      	v2f vert(appdata v)
      	{         
        	v2f o;
        	o.pos = UnityObjectToClipPos(v.vertex);
        	o.uv = v.uv;

        	// calculating viewdirection on vertex

        	return o;
      	}

      	fixed4 frag(v2f i) : COLOR 
      	{
			return half4(1,1,1,1);
      	}
      	ENDCG
    	}
  	}
}