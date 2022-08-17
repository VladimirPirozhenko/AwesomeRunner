Shader "Custom/CurvedSurfaceStandardCutout"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
        _AlphaCutoff("Alpha Cutoff", Range(0,1)) = 0.5
        _EmissionMap("Emission Map", 2D) = "white" {}
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader
    {
        Tags { "RenderType" = "Cutout" "Queue" = "Geometry" }//"DisableBatching" = "true" 
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard   fullforwardshadows vertex:vert addshadow

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        sampler2D MainTex;
        sampler2D _BumpMap;
        sampler2D _OcclusionMap;
        sampler2D _SmoothnessMap;
        sampler2D _EmissionMap;
        fixed4 _EmissionColor;

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
            float4 worldPosition = mul(unity_WorldToCamera, mul(unity_ObjectToWorld, v.vertex));
            float dist = length(float2(worldPosition.x, worldPosition.z));
            worldPosition.y -= _CurveStrength_y * dist * dist;//* _ProjectionParams.x;
            worldPosition.x -= _CurveStrength_x * dist * dist; //* _ProjectionParams.x;
            v.vertex = mul(unity_WorldToObject, mul(unity_CameraToWorld, worldPosition));
            //this question helped https://stackoverflow.com/questions/50512600/send-vertice-shader-changes-to-surface-shader
            //float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(pos.z);
            //float4 worldPosition = mul(unity_ObjectToWorld, v.vertex);  // get world space position of vertex
            //worldPosition.y -= _CurveStrength_y * dist * dist * _ProjectionParams.x;
            //worldPosition.x -= _CurveStrength_x * dist * dist * _ProjectionParams.x;
            //// offset vertical position by factor and square of distance.
            //// the default 0.01 would lower the position by 1cm at 1m distance, 1m at 10m and 100m at 100m
            //v.vertex = mul(unity_WorldToObject, worldPosition);
        }

        sampler2D _MainTex;

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        half _AlphaCutoff;
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
            o.Emission = c.rgb * tex2D(_MainTex, IN.uv_MainTex).a * _EmissionColor;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = tex2D(_SmoothnessMap, IN.uv_SmoothnessMap).rgb * _Glossiness;
            o.Alpha = c.a;
            clip(o.Alpha - _AlphaCutoff);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
