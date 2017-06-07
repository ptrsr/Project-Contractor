#ifndef VIGNETTE
#define VIGNETTE

uniform float _vignetteRange;
uniform float _vignetteWidth;
uniform float _vignetteIntensity;

half4 Vignette(float2 uv)
{
	float dist = distance(uv, float2(0.5, 0.5));
	float start = _vignetteRange - _vignetteWidth;

	float diff = saturate((dist - start) / (_vignetteRange - start));
	diff = min(diff, _vignetteIntensity);

	half4 vignette = half4(1, 1, 1, 1);

	return vignette * diff;
}

#endif