#ifndef FOG_INCLUDED
#define FOG_INCLUDED

uniform half4 _startColor;
uniform half4 _endColor;
uniform float  _depth;
uniform float _fogEnd;

half4 fogColor (float2 uv) {
	// blend color from y0 to yDeep
	half4 fogCol = lerp(_startColor, _endColor, _depth - uv.y / 5);
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