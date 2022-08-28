Shader "Mobile/CustomFogCube"
{
	Properties
	{
	_FogStart("Fog Start", float) = 0 //объ€вл€ем наши новые переменные дл€ тумана
	_FogEnd("Fog End", float) = 50
	}
		SubShader
	{
	Tags{ "RenderType" = "Opaque" }
	Fog{ Mode off }
	Pass
	{
	CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma multi_compile _ LIGHTMAP_ON
	#include "UnityCG.cginc"
	half _FogStart;  //определ€ем новые переменные в рамках CGPROGRAM
	half _FogEnd;
	struct appdata
	{
	float4 vertex : POSITION;
	float4 color : COLOR;
	float4 uv : TEXCOORD1;
	};
	struct v2f
	{
	float4 pos : SV_POSITION;
	float4 uv : TEXCOORD1;
	half fog : TEXCOORD2;  //добавл€ем новую переменную дл€ расчета рассто€ни€ отображени€ тумана и последующей передачи в fragment функцию
	float4 color : COLOR;
	half3 viewDir : TEXCOORD3;
	};
	v2f vert(appdata v)
	{
	v2f o;
	o.color = v.color;
	o.pos = UnityObjectToClipPos(v.vertex);
	//lightmaps
	o.uv.xy = v.uv.xy * unity_LightmapST.xy + unity_LightmapST.zw;
	//fog высчитываем положение тумана в зависимости от заданных значений
	half fogz = UnityObjectToViewPos(v.vertex).z;
	o.fog = saturate((fogz + _FogStart) / (_FogStart - _FogEnd));
	float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
	o.viewDir = -(normalize(UnityWorldSpaceViewDir(worldPos)));
	return o;
	}
	half4 frag(v2f i) : COLOR
	{
	UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
	fixed4 c = i.color * 0.5;
	//lightmaps
	#ifdef LIGHTMAP_ON
	  fixed4 lm = UNITY_SAMPLE_TEX2D(unity_Lightmap, i.uv.xy);
	c.rgb *= lm.rgb * 4;
	#endif
	//fog замен€ем плавно цвет поверхности на цвет кубомапы (он же наш туман).  убомапу нужно задать в настройках освещение (Lighting > Scene > Environment Reflection > Source = Custom > Cubemap = ¬аша кубомапа)
	half4 fogCube = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, i.viewDir);
	return lerp(c, fogCube, i.fog);
	}
	ENDCG
	}
	}
		Fallback "Mobile/VertexLit"
}