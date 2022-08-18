// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// code from Endless Runner - Sample Game

#include "UnityCG.cginc"

struct appdata
{
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
	float4 color : COLOR;
};

struct v2f
{
	float2 uv : TEXCOORD0;
	UNITY_FOG_COORDS(1)
	float4 color : TEXCOORD2;
	float4 vertex : SV_POSITION;
};

sampler2D _MainTex;
float4 _MainTex_ST;
float _CurveStrength_x;
float _CurveStrength_y;

v2f vert(appdata v)
{
	v2f o;

	o.vertex = UnityObjectToClipPos(v.vertex);


	float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(o.vertex.z);

	//float4 worldPosition = mul(unity_ObjectToWorld, o.vertex);
	//// get world space position of vertex
	//// distance squared from vertex to the camera, this power gives the curvature
	////worldPosition.y -= distance * 0.001;
	//worldPosition.y -= _CurveStrength_y * dist * dist * _ProjectionParams.x;
	//worldPosition.x -= _CurveStrength_x * dist * dist * _ProjectionParams.x;
	// offset vertical position by factor and square of distance.
	// the default 0.01 would lower the position by 1cm at 1m distance, 1m at 10m and 100m at 100m
	//o.vertex = mul(unity_WorldToObject, worldPosition);

	//o.vertex = UnityObjectToClipPos(v.vertex);

	//WORKING
	//float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(o.vertex.z);

	//o.vertex.y -= _CurveStrength_y * dist * dist * _ProjectionParams.x;
	//o.vertex.x -= _CurveStrength_x * dist * dist * _ProjectionParams.x;

	float4 worldPosition = mul(unity_ObjectToWorld, o.vertex);
	worldPosition.y -= _CurveStrength_y * dist * dist * _ProjectionParams.x;
	worldPosition.x -= _CurveStrength_x * dist * dist * _ProjectionParams.x;
	// offset vertical position by factor and square of distance.
	// the default 0.01 would lower the position by 1cm at 1m distance, 1m at 10m and 100m at 100m
	o.vertex = mul(unity_WorldToObject, worldPosition);

	o.uv = TRANSFORM_TEX(v.uv, _MainTex);

	o.color = v.color;

	UNITY_TRANSFER_FOG(o, o.vertex);
	return o;
}

fixed4 frag(v2f i) : SV_Target
{
	// sample the texture
	fixed4 col = tex2D(_MainTex, i.uv) * i.color;
// apply fog
UNITY_APPLY_FOG(i.fogCoord, col);
return col;
}