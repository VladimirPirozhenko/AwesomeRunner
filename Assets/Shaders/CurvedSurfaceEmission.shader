Shader "Custom/CurvedSurfaceEmissionOnly"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo", 2D) = "white" {}
        _EmissionMap("Emission Map", 2D) = "white" {}
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }

        SubShader
        {
            Tags { "RenderType" = "Fade" "Queue" = "Geometry" }
            LOD 200

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard  fullforwardshadows vertex:vert addshadow

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0
            sampler2D _EmissionMap;
            fixed4 _EmissionColor;
            //float4 MainTex_ST;
            sampler2D MainTex;
            //float _CurveStrength_x;
            //float _CurveStrength_y;

            struct Input
            {
                float2 uv_EmissionMap;
                float2 uv_MainTex;
                float2 texcoord;
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
                o.Emission = tex2D(_MainTex, IN.uv_EmissionMap).a * _EmissionColor;
                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
