#ifndef FOG_INCLUDED
#define FOG_INCLUDED

uniform half4 _startColor;
uniform half4 _endColor;
uniform float _depth;
uniform float _fogDepth;

uniform float _intensity;
uniform float _curve;

half4 fogColor (float3 worldPos) {
	float fragDepth = worldPos.y;
	float depthBlend = ((fragDepth - surface) / (_fogDepth - surface));
	depthBlend = saturate(depthBlend);
	half4 fogCol = lerp(_startColor, _endColor, pow(depthBlend, 1));
	return fogCol;
}

float fogBlend (float linearDepth) {
	float diff = saturate(linearDepth / _intensity);
	return pow(diff, _curve);
}

#endif