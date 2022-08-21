// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/CurvedSurfaceStandard"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
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
            sampler2D _OcclusionMap;
            sampler2D _SmoothnessMap;
            sampler2D _EmissionMap;
            fixed4 _EmissionColor;

            float4 MainTex_ST;
 

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_SmoothnessMap;
                float2 uv_OcclusionMap;
                float2 uv_EmissionMap;
                half3 debugColor;
            };

            #include "CurvedCode.cginc"

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
