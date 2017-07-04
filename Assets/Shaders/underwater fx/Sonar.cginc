#ifndef SONAR_INCLUDED
#define SONAR_INCLUDED

// SONAR DATA
uniform float4 _pulseOrigins[10];
uniform float _pulseDistances[10];
uniform float _outlineWidth;
uniform float _highlightWidth;
uniform float _fade;
uniform float _start;

uniform float4 _outlineColor;
uniform float4 _highlightColor;

// PING DATA
uniform float _pingDistances[10];
uniform float _pingMaxDistances[10];
uniform float4 _pingPositions[10];
uniform float4 _pingColors[10];
uniform float4 _sonarOrigins[10];

half4 pulseHighlight (float3 pos) 
{
	half4 color = half4(0,0,0,0);

	for(int i = 0; i < 10; i++) 
	{
		float pulseDist = _pulseDistances[i];
		
		if (pulseDist == -1.0)
			break;

		float fragDist = distance(pos, _pulseOrigins[i]);
		if (fragDist < pulseDist && fragDist > pulseDist - _fade)
		{
			float diff = 1 - (pulseDist - fragDist) / (_fade);

			color = _highlightColor;
			color *= diff;
		}
		if (fragDist < _start)
			color *= 0;
	}
	// masking pulse highlight
	float depthMask = 0;

	if (
		pos.z > -_outlineWidth / 2 && pos.z < _outlineWidth / 2 ||
		abs(pos.z) % 2.5f < 0.15f || abs(pos.y) % 2.5f < 0.15f || abs(pos.x) % 2.5f < 0.15f
		)
		depthMask = 1;

	depthMask *= max(0, 1 - abs(pos.z) * 0.2f);

	color *= depthMask;
	// return pulse
	return color;
}

half4 pulseOutline (float3 pos) 
{
	// calculating each pulse draw
	half4 color = half4(0,0,0,0);

	for(int i = 0; i < 10; i++) 
	{
		float pulseDist = _pulseDistances[i];

		if (pulseDist == -1.0)
			break;

		float fragDist = distance(pos, _pulseOrigins[i]);
		if (fragDist < pulseDist + _highlightWidth /2 && fragDist > pulseDist - _highlightWidth/2 && fragDist > _start)
			color = _outlineColor;
	}
	return color;
} 


half4 pulseForPaul (float3 pos) {
	half4 color = half4(0,0,0,0);

	for(int i = 0; i < 10; i++) 
	{
		float pulseDist = _pulseDistances[i];
		
		if (pulseDist == -1.0)
			break;

		float fragDist = distance(pos, _pulseOrigins[i]);
		if (fragDist < pulseDist && fragDist > pulseDist - 2)
		{
			float diff = 1 - (pulseDist - fragDist) / (_fade);

			color = _highlightColor;
			color *= diff;
		}
		if (fragDist < _start)
			color *= 0;
	}
//	float depthMask = 0;

//	float _fadeDiff = (_outlineWidth /2) / abs(pos.z);

//	if (pos.z > -_outlineWidth / 2 && pos.z < _outlineWidth / 2)
//		depthMask = 1;

//	color *= depthMask;
	return color;
}




half4 pulsePing (float3 pos) 
{
	half4 col = half4(0,0,0,0);
	
	for(int i = 0; i < 10; i++) 
	{
		float pingDist = _pingDistances[i];

		if (pingDist == -1)
			break;

		float2 pingPos = _pingPositions[i].xy;


		float fragDist = distance(pos.xy, pingPos);

		float multi = pow(max(0, dot(normalize(pingPos - pos.xy), normalize(pingPos - _sonarOrigins[i]))), 12) * max(0, 1 - fragDist / _pingMaxDistances[i]);


		if (fragDist < pingDist + _highlightWidth /2 && fragDist > pingDist - _highlightWidth /2 && pingDist > 2)
			col += _pingColors[i] * multi;

		if (fragDist < pingDist - 3 + _highlightWidth /2 && fragDist > pingDist - 3 - _highlightWidth /2 && pingDist > 2)
			col += _pingColors[i] * multi;
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