Shader "custom/volumetric_fx"
{
	SubShader
	{
		//------------------PASS----------------------
		// Used for making the light rays (orthogonal)
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform sampler2D _LastCameraDepthTexture;
			uniform float _height;
			
			uniform int _layers = 1;

			uniform float _time;

			float rand(float x)
			{
				return frac(sin(x));
			}

			float noiseX(float x, float multi)
			{
				float rI = floor(x * (30.0 * multi) + sin(_time));  // integer
				float rF = frac(x * (10.0 * multi) + _time / 3.0);  // fraction
				return pow(lerp(rand(rI), rand(rI + (2 * multi)), smoothstep(0., 1., rF)), 3);
			}

			float noiseY(float y)
			{
				float rI = floor(y * 2 + sin(_time / 3.0));  // integer
				float rF = frac(y * 3  + _time / 3.0);  // fraction
				return pow(lerp(rand(rI), rand(rI + 4.5), smoothstep(0., 1., rF)), 2);
			}

			fixed4 frag (v2f_img i) : SV_Target
			{
				float final = 0;
				
				for (int j = 0; j < _layers; j++)
				{
					float depth = 1 - tex2D(_LastCameraDepthTexture , float2(j, i.uv.x));

					if (depth > i.uv.y)
						depth = 1;
					else
						depth = 0;
					
					final += depth;
				}

				final /= _layers;

				float horizontal = 1 - pow(abs(i.uv.x - 0.5f) * 2, 2);
				float vertical = 1 - pow(i.uv.y, 2);

				float cutOff = noiseY(i.uv.x);

				if (i.uv.y > cutOff)
					cutOff = cutOff / (i.uv.y);
				else
					cutOff = 1;

				float noise = clamp(noiseX(i.uv.x, 0.3f) + noiseX(i.uv.x, 1.0f), 0, 1);

				return fixed4(1, 1, 1, final * horizontal * vertical * cutOff * noise);
			}
			ENDCG
		}
		//-----------------PASS--------------------
		// Used for blurring (with multiple passes)
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			static float offset[3] = {0.0, 1.3846153846, 3.2307692308};
			static float weight[3] = {0.2270270270, 0.3162162162, 0.0702702703};


			uniform sampler2D _blur;
			uniform bool _horizontal;
			uniform float2 _size;

			fixed4 frag (v2f_img i) : SV_Target
			{
				float result = tex2D( _blur, i.uv).a * weight[0];

				if (_horizontal) 
				{   
					for (int j = 1; j < 3; j++) 
					{
						result += tex2D( _blur, i.uv + float2(0, offset[j] * _size.x)).a * weight[j];
						result += tex2D( _blur, i.uv - float2(0, offset[j] * _size.x)).a * weight[j];
					}
				}
				else 
				{
					for (int k = 1; k < 3; k++) 
					{
						result += tex2D( _blur, (i.uv + float2(offset[k] * _size.y, 0))).a * weight[k];
						result += tex2D( _blur, (i.uv - float2(offset[k] * _size.y, 0))).a * weight[k];
					}
				}

				return fixed4(1, 1, 1, result);
			}
			ENDCG
		}
		//----------------------PASS------------------------
		// Used for putting the blurred texture in the world
		Pass
			{
				Cull Off ZTest On
				Blend SrcAlpha OneMinusSrcAlpha
				Tags{ "RenderType" = "Transparent" }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv	  : TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float2 uv	  : TEXCOORD0;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = v.uv;
					return o;
				}

				uniform sampler2D _texture;

				fixed4 frag(v2f i) : SV_Target
				{
					return tex2D(_texture, i.uv);	
				}
			ENDCG
		}

	//	Pass
	//		{
	//			Cull Off ZTest On
	//			Blend SrcAlpha OneMinusSrcAlpha
	//			Tags{ "RenderType" = "Transparent" }

	//			CGPROGRAM
	//			#pragma vertex vert
	//			#pragma fragment frag

	//			static const float PI = 3.14159265f;

	//			#include "UnityCG.cginc"

	//			struct appdata
	//		{
	//			float4 vertex : POSITION;
	//			float2 uv	  : TEXCOORD0;
	//		};

	//		struct v2f
	//		{
	//			float4 vertex : SV_POSITION;
	//			float2 uv	  : TEXCOORD0;
	//		};

	//		v2f vert(appdata v)
	//		{
	//			v2f o;
	//			o.vertex = UnityObjectToClipPos(v.vertex);
	//			o.uv = v.uv;
	//			return o;
	//		}

	//		uniform sampler2D _LastCameraDepthTexture;
	//		uniform float _height;

	//		fixed4 frag(v2f i) : SV_Target
	//		{
	//			float uvCoord = ((1 / i.uv.x) * i.uv.y + 1) / 2;

	//			float depth = 1 - tex2D(_LastCameraDepthTexture, float2(0.0f, uvCoord)) * 1100;

	//			if (i.uv.x > depth * _height)
	//				depth = 0;
	//			else
	//				depth = 1;

	//			return depth;

	//			//return float4(uvCoord, 0, 0, 1);
	//			//return float4(1, 1, 1, tex2D(_LastCameraDepthTexture, float2(0, i.uv.x)));

	//			float fallOff = pow(clamp(1 - length(i.uv) / 50, 0, 1), 2);
	//			float sides = pow(dot(float2(1, 0), normalize(float2(i.uv.x, i.uv.y * 2))), 2);

	//			return float4(1, 1, 1, sides * fallOff);
	//		}
	//		ENDCG
	//	}
	}
}