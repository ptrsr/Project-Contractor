#ifndef FOG_INCLUDED
#define FOG_INCLUDED

uniform half4 _startColor;
uniform half4 _endColor;
//uniform float _depth;
uniform float _fogDepth;


uniform float _intensity;
uniform float _curve;

uniform int    _darkZones;
uniform float4 _darkPositions[10];
uniform float4 _darkColors[10];
uniform float _darkFarRadius[10];
uniform float  _darkCloseRadius[10];


half4 fogColor (float3 worldPos) {
	float fragDepth = worldPos.y;
	float depthBlend = ((fragDepth - surface) / (_fogDepth - surface));
	depthBlend = saturate(depthBlend);
	half4 fogCol = lerp(_startColor, _endColor, pow(depthBlend, 1));


	for (int i = 0; i < _darkZones; i++)
	{
		float dist = distance(_darkPositions[i].xy, worldPos.xy);
		float close = _darkCloseRadius[i];
		float far = _darkFarRadius[i];
		float4 color = float4(_darkColors[i].xyz, 1);

		if (dist > close + far)
			continue;

		if (dist < close)
			return color;

		fogCol = float4(lerp(fogCol.xyz, color.xyz, 1 - ((dist - close) / far)), 1);
	}

	return fogCol;
}

float fogBlend (float linearDepth) {
	float diff = saturate(linearDepth / _intensity);
	return pow(diff, _curve);
}

#endif