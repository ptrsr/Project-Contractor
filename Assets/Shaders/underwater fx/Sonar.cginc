#ifndef SONAR_INCLUDED
#define SONAR_INCLUDED

uniform int _pulselength;
uniform float _pulses[10];
uniform float4 originarray[10];
uniform float width;
uniform float fade;
uniform float edgeWidth;
uniform float _start;

uniform sampler2D sonarScene;

half4 pulseColor (float3 pos, float2 uv) {
	// calculating each pulse draw
	half4 pulsecol = half4(0,0,0,0);

	float altDepth = DecodeFloatRG(tex2D(sonarScene, uv));
	altDepth = Linear01Depth(altDepth);

	for(int i = 0; i < _pulselength; i++) {
		float dist = distance(pos, originarray[i]);
		if (dist < _pulses[i] && dist > _pulses[i] - fade) {
			float diff = 1 - (_pulses[i] - dist) / (fade);

			pulsecol = half4(1,0,0,1);

			if(altDepth < 0.9)
				pulsecol = half4(0,1,0,1);

			pulsecol *= diff;
		}
		if (dist < _start)
			pulsecol *= 0;
	}
	// masking pulse highlight
	float depthMask = 0;

	float fadeDiff = (width/2) / abs(pos.z);

	if (pos.z > -width/2 && pos.z < width/2)
		depthMask = 1;

	pulsecol *= fadeDiff;
	pulsecol *= depthMask;
	// return pulse
	return pulsecol;
}

// UPDATED ———————————————————————————————
half4 edgeCol (float3 pos) {
	// calculating each pulse draw
	half4 col = half4(0,0,0,0);
	for(int i = 0; i < _pulselength; i++) {
		float dist = distance(pos, originarray[i]);
		if (dist < _pulses[i] + edgeWidth/2 && dist > _pulses[i] - edgeWidth/2 && dist > _start) {
			col = half4(1,1,1,1);
		}
	}
	// return color
	return col;
} 
// UPDATED ———————————————————————————————


float4 horizBars(float2 p) {
	return 1 - saturate(round(abs(frac(p.y * 100) * 2)));
}


float3 xyPlanePosition (float4 ray) {
	// vector data
	float3 camPos = _WorldSpaceCameraPos;
	float3 fragDir = normalize(ray.xyz);
	float3 vb = float3(0, 0, -camPos.z);
	float b = length(vb);
	vb = normalize(vb);
	// x0 depth calculation
	float cosalpha = dot(vb, fragDir) / (length(vb) * length(fragDir));
	float c = b / cosalpha;
	float3 newDir = c * normalize(fragDir);
	float3 xPlanePos = camPos + newDir;
	// return fake fragment position
	return xPlanePos;
}

#endif