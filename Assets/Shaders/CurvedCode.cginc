#include "UnityCG.cginc"
float _CurveStrength_x;
float _CurveStrength_y;
float3 _CurveOrigin;

void vert(inout appdata_full v, out Input o)
{
    float4 pos;
    float2 uv;
    UNITY_INITIALIZE_OUTPUT(Input, o);
    float4 modifiedPos = v.vertex;
    float4 positionInCameraSpace = mul(unity_WorldToCamera, mul(unity_ObjectToWorld, v.vertex));
    float dist = length(float2(positionInCameraSpace.x, positionInCameraSpace.z));
    //float dist = distance(positionInCameraSpace - _OriginPosition);
    positionInCameraSpace.y -= _CurveStrength_y * dist * dist;
    positionInCameraSpace.x -= _CurveStrength_x * dist * dist;

    modifiedPos = mul(unity_WorldToObject, mul(unity_CameraToWorld, positionInCameraSpace));
    v.vertex = modifiedPos;
}