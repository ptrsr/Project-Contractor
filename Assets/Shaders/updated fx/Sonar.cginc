#ifndef SONAR_INCLUDED
#define SONAR_INCLUDED

int _pulselength;
float _pulses[20];
float4 originarray[20];
float width;

half4 pulseColor (float3 pos) {
	// calculating each pulse draw
	half4 pulsecol = half4(0,0,0,0);
	for(int i = 0; i < _pulselength; i++) {
		float dist = distance(pos, originarray[i]) +.5;
		if (dist < _pulses[i] && dist > _pulses[i] - width) {
			float diff = 1 - (_pulses[i] - dist) / (width);
			pulsecol = half4(1,0,0,1);
			pulsecol *= diff;
		}
	}
	// masking pulse highlight
	float depthMask = 0;
	if (pos.z > 0 && pos.z < 1)
		depthMask = 1;
	pulsecol *= depthMask;
	// return pulse
	return pulsecol;
}

half4 edgeColor (float3 pos) {
	// white pulse edge
	half4 edgecol = half4(0,0,0,0);
	for(int i = 0; i < _pulselength; i++) {
		float dist = distance(pos, originarray[i]);
		if (dist < _pulses[i] && dist > _pulses[i] - 0.5) {
			edgecol = half4(1,1,1,1);
		}
	}

	// return edge
	return edgecol;
}

float3 xPlanePosition (float4 ray) {

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