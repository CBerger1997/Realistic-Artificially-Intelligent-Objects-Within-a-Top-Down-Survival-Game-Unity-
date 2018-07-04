Shader "Transparent/Cutout/Complete" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Transparency (A)", 2D) = "white" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
	_Spec ("Specular (RGB), Gloss (A)", 2D) = "grey" {}
	_Shininess ("Shininess", Range (0.01, 2)) = 1
	_Glossiness ("Glossiness", Range(0.01, 2)) = 0.078125
	
	_BumpMap ("Normalmap", 2D) = "bump" {}
	
	_Illum ("Illumination (RGB)", 2D) = "black" {}
	_IllumMag ("Intensity", Range(0, 2)) = 1
	
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 400
	
CGPROGRAM
#pragma surface surf BlinnPhong alphatest:_Cutoff
#pragma exclude_renderers flash

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _Spec;
sampler2D _Illum;
fixed4 _Color;
half _Shininess;
half _Glossiness;
half _IllumMag;

struct Input {
	float2 uv_MainTex;
	float2 uv_Spec;
	float2 uv_Illum;
	float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
	fixed4 texS = tex2D(_Spec, IN.uv_Spec);
	fixed4 texI = tex2D(_Illum, IN.uv_Illum);
	o.Albedo = tex.rgb * _Color.rgb;
	o.Emission = texI.rgb * _IllumMag;
	o.Gloss = texS.rgb * _Shininess;
	o.Alpha = tex.a * _Color.a;
	o.Specular = texS.a * _Glossiness;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
}

FallBack "Transparent/Cutout/BumpedSpecular"
}
