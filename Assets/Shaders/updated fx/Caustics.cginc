#ifndef CAUSTICS_INCLUDED
#define CAUSTICS_INCLUDED

uniform sampler2D _Caustics;
uniform float causticsSize;
uniform float causticsIntensity;
uniform float causticsDepth;
uniform float surface;

float3 worldPosition (float depth, float4 ray) {
	// calculating fragments world position
	float4 dir = depth * ray;
	float3 pos = _WorldSpaceCameraPos + dir;
	return pos;
}

half4 Caustics (float3 worldPos) {
	// texture shift over time
	float time = _Time;
	float2 shift1 = float2(time, 0);
	float2 shift2 = float2(0.5, time);
	// texture overlay in world space
	float2 fragUV = float2(worldPos.x, worldPos.z) / causticsSize;
	half4 c1 = tex2D(_Caustics, fragUV + shift1);
	half4 c2 = tex2D(_Caustics, fragUV + shift2);
	half4 c = (c1 * causticsIntensity) * (c2 * causticsIntensity);
	// return caustics
	return c;
}

float causticsDepthBlend (float3 worldPos) {
	float fragDepth = worldPos.y;
	float depthBlend = 1 - ((fragDepth - surface) / (causticsDepth - surface));
	if(fragDepth < causticsDepth)
		depthBlend = 0;
	if(fragDepth > surface)
		depthBlend = 1;
	// return blend diff
	return pow(depthBlend, 5);
}

float causticsMask (float3 worldNormal, float3 inVector) {
	float diff = pow(max(0, dot(worldNormal, inVector)), 2);
	return diff;
}

#endif