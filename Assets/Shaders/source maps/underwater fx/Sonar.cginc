#ifndef SONAR_INCLUDED
#define SONAR_INCLUDED

// SONAR DATA
uniform int _pulselength;
uniform float _pulses[10];
uniform float4 originarray[10];
uniform float width;
uniform float widthFade;
uniform float fade;
uniform float edgeWidth;
uniform float _start;
uniform float _distance;

// PING DATA
uniform float _pingInter[10];
uniform float4 _originInter[10];
uniform int _pingsInter;
uniform float _pingHostile[10];
uniform float4 _originHostile[10];
uniform int _pingsHostile;

half4 pulseColor (float3 pos, float2 uv) {
	half4 pulsecol = half4(0,0,0,0);
	float depthMask = 0;
	if (abs(pos.z) < width/2 + widthFade)
		depthMask = saturate( 1 - ((abs(pos.z) - width/2) / widthFade) );
	// calculating each pulse draw
	for(int i = 0; i < _pulselength; i++) {
		float dist = distance(pos.xy, originarray[i].xy);
		if (dist < _pulses[i] && dist > _pulses[i] - fade && dist > _start) {
			pulsecol = half4(1,0,0,1);
			float diff = 1 - (_pulses[i] - dist) / (fade);
			pulsecol *= diff;
		}
	}
	pulsecol *= depthMask;
	return pulsecol;
}

half4 edgeCol (float3 pos) {
	// calculating each pulse draw
	half4 col = half4(0,0,0,0);
	for(int i = 0; i < _pulselength; i++) {
		float dist = distance(pos, originarray[i]);
		if (dist < _pulses[i] + edgeWidth/2 && dist > _pulses[i] - edgeWidth/2 && dist > _start)
			col = half4(1,1,1,1);
	}
	return col;
} 

half4 pingInterColor (float3 pos) {
	half4 col = half4(0,0,0,0);
	for(int i = 0; i < _pingsInter; i++) {
		// double ping draw
		float dist = distance(pos, _originInter[i]);
		if (dist < _pingInter[i] + edgeWidth/2 && dist > _pingInter[i] - edgeWidth/2 && dist > 2)
			col = half4(0,1,0,1);
//		if (dist < _pingInter[i] - 3 + edgeWidth/2 && dist > _pingInter[i] - 3 - edgeWidth/2 && dist > 2)
//			col = half4(0,1,0,1);
	}
	return col;
}

half4 pingHostileColor (float3 pos) {
	half4 col = half4(0,0,0,0);
	for(int i = 0; i < _pingsHostile; i++) {
		float dist = distance(pos, _originHostile[i]);
		if (dist < _pingHostile[i] + edgeWidth/2 && dist > _pingHostile[i] - edgeWidth/2 && dist > 2)
			col = half4(1,0,0,1);
	}
	return col;
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