#ifndef FOG_INCLUDED
#define FOG_INCLUDED

uniform half4 _startColor;
uniform half4 _endColor;
uniform float _depth;
uniform float _fogEnd;
uniform float _fogDepth;
uniform float4 _darkZones[10];
uniform float _rangeData[10];

half4 fogColor (float3 worldPos) {
	float fragDepth = worldPos.y;
	float depthBlend = ((fragDepth - surface) / (_fogDepth - surface));
	depthBlend = saturate(depthBlend);
	// blend color from y0 to yDeep
	half4 fogCol = lerp(_startColor, _endColor, pow(depthBlend, 1));
	// return fog color
	return fogCol;
}

float fogBlend (float eyeDepth) {
	// blend fog over scene depth
	float diff = eyeDepth / _fogEnd;
	if(eyeDepth > _fogEnd)
		diff = 1;
	return diff;
}

#endif