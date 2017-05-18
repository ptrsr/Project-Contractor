#ifndef FOG_INCLUDED
#define FOG_INCLUDED

uniform half4 _startColor;
uniform half4 _endColor;
uniform float _depth;
uniform int   _fogIntensity;

half4 fogColor (float2 uv) {
	// blend color from y0 to yDeep
	half4 fogCol = lerp(_startColor, _endColor, _depth);
	return fogCol;
}

float fogBlend (float linearDepth)
{
	return pow(linearDepth, _fogIntensity);
}

#endif