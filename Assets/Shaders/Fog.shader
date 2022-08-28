Shader "Hidden/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NearColor("Near color", Color) = (0.75, 0.35, 0, 1)
        _FarColor("Far color", Color) = (1, 1, 1, 1)
        _NearValue("Near value", Float) = 1
        _FarValue("Far value", Float) = 1000
        _Density("Density", Float) = 1000
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;            
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;    
                o.screenPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.screenPos.z);
                return o;
            }

            sampler2D _MainTex;
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            //sampler2D _CameraDepthTexture;
            fixed4 _NearColor;
            fixed4 _FarColor;
            float _NearValue;
            float _FarValue;
            float _Density;

            fixed4 frag (v2f i) : SV_Target
            {

                  float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                 float depth = sceneZ - i.screenPos.z;
                 //fixed4 col = _FarColor;
                fixed4 colTex = tex2D(_MainTex, i.uv);
                fixed4 col = tex2D(_CameraDepthTexture, i.uv);
               // float depth = UNITY_SAMPLE_DEPTH(col);
               // depth = Linear01Depth(depth);
                fixed depthFading = saturate((abs(pow(depth, _NearValue))) / _FarValue);
                //colTex *= depthFading;
                float fogFactor = saturate(1.0 - (_FarValue - depth) / (_FarValue - _NearValue));
                //return colTex;
               // float fogFactor = _FarValue - depth / _FarValue - _NearValue;
                //color.rgb = lerp(color.rgb
                return lerp(colTex, _FarColor, fogFactor);
            }
            ENDCG
        }
    }
}
