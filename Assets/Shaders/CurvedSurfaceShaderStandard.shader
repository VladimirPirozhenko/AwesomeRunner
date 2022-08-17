Shader "Custom/CurvedSurfaceShaderStandard"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        _BumpMap("Normal Map", 2D) = "bump" {}
        _OcclusionMap("Occlusion Map", 2D) = "white" {}
        _SmoothnessMap("Smoothness Map", 2D) = "black" {}
        //[HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "DisableBatching" = "true" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:vert

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        sampler2D MainTex;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;
        sampler2D _SmoothnessMap;

        float4 MainTex_ST;
        float _CurveStrength_x;
        float _CurveStrength_y;      

        struct Input
        {    
            float2 uv_MainTex;  
            float2 uv_BumpMap;
            float2 uv_SmoothnessMap;
            float2 uv_OcclusionMap;
            float2 texcoord;
        };

        void vert(inout appdata_full v, out Input o)
        {
            float4 pos ;
            float2 uv ; 
            UNITY_INITIALIZE_OUTPUT(Input, o);
            pos = UnityObjectToClipPos(v.vertex);
            uv = v.texcoord;

            //this question helped https://stackoverflow.com/questions/50512600/send-vertice-shader-changes-to-surface-shader
            float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(pos.z);
            float4 worldPosition = mul(unity_ObjectToWorld, v.vertex);
            // get world space position of vertex
            half2 wpToCam = _WorldSpaceCameraPos.xz - worldPosition.xz;
            // get vector to camera and dismiss vertical component
            half distance = dot(wpToCam, wpToCam);
            // distance squared from vertex to the camera, this power gives the curvature
            //worldPosition.y -= distance * 0.001;
            worldPosition.y -= _CurveStrength_y * dist * dist * _ProjectionParams.x;
            worldPosition.x -= _CurveStrength_x * dist * dist * _ProjectionParams.y;
            // offset vertical position by factor and square of distance.
            // the default 0.01 would lower the position by 1cm at 1m distance, 1m at 10m and 100m at 100m
            v.vertex = mul(unity_WorldToObject, worldPosition);
            UNITY_TRANSFER_FOG(o, pos);
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

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            o.Occlusion = tex2D(_OcclusionMap, IN.uv_OcclusionMap).rgb;
            //o.Emission = c.rgb * tex2D(_MainTex, IN.uv_MainTex).a * _EmissionColor;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = tex2D(_SmoothnessMap, IN.uv_SmoothnessMap).rgb * _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
