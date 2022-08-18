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
            float _CurveStrength_x;
            float _CurveStrength_y;

            struct Input
            {
                float2 uv_EmissionMap;
                float2 uv_MainTex;
                float2 texcoord;
            };

            void vert(inout appdata_full v, out Input o)
            {
                float4 pos;
                float2 uv;
                UNITY_INITIALIZE_OUTPUT(Input, o);
                pos = UnityObjectToClipPos(v.vertex);
                uv = v.texcoord;

                //this question helped https://stackoverflow.com/questions/50512600/send-vertice-shader-changes-to-surface-shader
                //float dist = UNITY_Z_0_FAR_FROM_CLIPSPACE(pos.z);
                //float4 worldPosition = mul(unity_ObjectToWorld, v.vertex);
                //// get world space position of vertex
                //half2 wpToCam = _WorldSpaceCameraPos.xz - worldPosition.xz;
                //// get vector to camera and dismiss vertical component
                //half distance = dot(wpToCam, wpToCam);
                //// distance squared from vertex to the camera, this power gives the curvature
                ////worldPosition.y -= distance * 0.001;
                //worldPosition.y -= _CurveStrength_y * dist * dist * _ProjectionParams.x;
                //worldPosition.x -= _CurveStrength_x * dist * dist * _ProjectionParams.x;
                //// offset vertical position by factor and square of distance.
                //// the default 0.01 would lower the position by 1cm at 1m distance, 1m at 10m and 100m at 100m
                //v.vertex = mul(unity_WorldToObject, worldPosition);
                float4 worldPosition = mul(unity_WorldToCamera, mul(unity_ObjectToWorld, v.vertex));
                float dist = length(float2(worldPosition.x, worldPosition.z));
                worldPosition.y -= _CurveStrength_y * dist * dist;//* _ProjectionParams.x;
                worldPosition.x -= _CurveStrength_x * dist * dist; //* _ProjectionParams.x;
                v.vertex = mul(unity_WorldToObject, mul(unity_CameraToWorld, worldPosition));
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
                o.Emission = tex2D(_MainTex, IN.uv_EmissionMap).a * _EmissionColor;
                // Metallic and smoothness come from slider variables
                o.Metallic = _Metallic;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}
