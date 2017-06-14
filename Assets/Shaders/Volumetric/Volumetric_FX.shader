Shader "custom/fx_volumetric"
{
	SubShader
	{
		//------------------PASS 0--------------------
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
		//-----------------PASS 1------------------
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
		//----------------------PASS 2----------------------
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


		//----------------------PASS 3----------------------
		// TEMP
		Pass
			{
				Cull Off ZTest LEqual ZWrite Off
				Blend SrcAlpha OneMinusSrcAlpha

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

			//triangle size
			uniform sampler2D _LastCameraDepthTexture;
			uniform float2 _nCorner;
			uniform float2 _fCorner;
			uniform float _cDir;

			//lit values
			uniform float _litDistance;
			uniform float _litAngle;
			uniform float _fallOff;

			fixed4 frag(v2f i) : SV_Target
			{
				float height = (i.uv.x - _nCorner.x) * _cDir + _nCorner.y;
				
				if (i.uv.y < 0)
					height = -height;
				
				float depthTexturePos = i.uv.y / height; // coordinate of depth texture (between -1 and 1)

				if (i.uv.y < 0)
					depthTexturePos = -depthTexturePos;

				float2 nearPlanePos = float2(_nCorner.x, _nCorner.y * depthTexturePos); // frag projected onto nearplane
				float2 farPlanePos = float2(_fCorner.x, _fCorner.y * depthTexturePos); // frag projected onto farplane

				float nDist = distance(nearPlanePos, i.uv);

				float near = sqrt(_nCorner.x * _nCorner.x + nearPlanePos.y * nearPlanePos.y);
				float far = sqrt(_fCorner.x *_fCorner.x + farPlanePos.y * farPlanePos.y);

				float a = far / (far - near);
				float b = far * near / (near - far);

				float textureDepthValue = 1 - tex2D(_LastCameraDepthTexture, float2(0, (depthTexturePos / 2.0f) + 0.5f));
				float depth = b / (textureDepthValue - a);

				if (depth - nearPlanePos.x > nDist)
				{
					float litDistance = pow(1 - (distance(nearPlanePos, i.uv) / distance(nearPlanePos, farPlanePos)), _litDistance);
					float litAngle = pow(dot(float2(1, 0), normalize(i.uv)), _litAngle);

					float lit = pow(litDistance * litAngle, _fallOff);
					return fixed4(1, 1, 1, lit);
				}
				else
					return 0;

			}
			ENDCG
		}
	}
}