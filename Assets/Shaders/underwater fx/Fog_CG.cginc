#ifndef FOG_INCLUDED
#define FOG_INCLUDED

uniform half4 _startColor;
uniform half4 _endColor;
uniform float _depth;
uniform float _fogFallOff;
uniform float _fogDepth;

half4 fogColor (float3 worldPos) {
	float fragDepth = worldPos.y;
	float depthBlend = ((fragDepth - _surface) / (_fogDepth - _surface));
	depthBlend = saturate(depthBlend);
	// blend color from y0 to yDeep
	half4 fogCol = lerp(_startColor, _endColor, pow(depthBlend, 1));
	// return fog color
	return fogCol;
}

float fogBlend (float linearDepth) {
	return pow(linearDepth, _fogFallOff);
}

#endif