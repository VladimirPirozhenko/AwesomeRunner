// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/CurvedSurfaceStandard"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _OcclusionMap("Occlusion Map", 2D) = "white" {}
        _SmoothnessMap("Smoothness Map", 2D) = "black" {}
        _EmissionMap("Emission Map", 2D) = "white" {}
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
       
    SubShader
    {
        Tags { "RenderType" = "Opaque" "DisableBatching" = "False" }//"Queue" = "Geometry"  " "LightMode" = "ShadowCaster""DisableBatching" = "true" "DisableBatching" = "True" "LightMode" = "ShadowCaster"
        LOD 200
        CGPROGRAM

            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard vertex:vert fullforwardshadows  addshadow

            // Use shader model 3.5 target, to get nicer looking lighting and to get emission map to work
            #pragma target 3.5
            sampler2D MainTex;
            sampler2D _BumpMap;
            sampler2D _OcclusionMap;
            sampler2D _SmoothnessMap;
            sampler2D _EmissionMap;
            fixed4 _EmissionColor;

            float4 MainTex_ST;
            float _CurveStrength_x;
            float _CurveStrength_y;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_BumpMap;
                float2 uv_SmoothnessMap;
                float2 uv_OcclusionMap;
                float2 uv_EmissionMap;
                half3 debugColor;

            };
     
            void vert(inout appdata_full v, out Input o)
            {
                float4 pos;
                float2 uv;
                UNITY_INITIALIZE_OUTPUT(Input, o);
                float4 modifiedPos = v.vertex;
                float4 positionInCameraSpace = mul(unity_WorldToCamera, mul(unity_ObjectToWorld, v.vertex));
                float dist = length(float2(positionInCameraSpace.x, positionInCameraSpace.z));

                positionInCameraSpace.y -= _CurveStrength_y * dist * dist;
                positionInCameraSpace.x -= _CurveStrength_x * dist * dist;

                modifiedPos = mul(unity_WorldToObject, mul(unity_CameraToWorld, positionInCameraSpace));
                v.vertex = modifiedPos;

            }             
            sampler2D _MainTex;

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
                // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
                o.Occlusion = tex2D(_OcclusionMap, IN.uv_OcclusionMap).rgb;
                o.Emission = tex2D(_EmissionMap, IN.uv_EmissionMap).rgb * _EmissionColor;
                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Smoothness = tex2D(_SmoothnessMap, IN.uv_SmoothnessMap).rgb * _Glossiness;
                o.Alpha = c.a;
            }
            ENDCG
        }
    FallBack "Diffuse"
}
