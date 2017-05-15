Shader "custom/DepthPass"
{
	Properties
	{
		_MainTex   ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex  : SV_POSITION;
			};
			
			v2f vert (appdata v)
			{
				v2f o;

				o.vertex  = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_DEPTH(o.depth);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				return fixed4(1,0,0,1);
			}

			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _CameraDepthTexture;

			float4 frag(v2f_img i) : COLOR
			{
				return Linear01Depth(UNITY_SAMPLE_DEPTH(tex2D(_CameraDepthTexture, i.uv)));
			}
			ENDCG
		}
	}
}