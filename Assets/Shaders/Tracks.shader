Shader "Custom/Tracks"
{
    Properties
    {
		_Tess("Tessellation", Range(1,32)) = 4
        _MainColor ("Main Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_Normal("Normal Map", 2D) = "white" {}
		_GroundColor("Ground Color", Color) = (1,1,1,1)
		_GroundTex("Ground Texture", 2D) = "white" {}
		_Splat("SplatMap", 2D) = "black" {}
		_Displacement("Displacement", Range(0, 1.0)) = 0.3
        
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows vertex:disp tessellate:tessDistance

       
        #pragma target 4.6

		#include "Tessellation.cginc"

		struct appdata {
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float2 texcoord : TEXCOORD0;
			float2 texcoord1 : TEXCOORD1;
			float2 texcoord2 : TEXCOORD2;

		};

		float _Tess;

		float4 tessDistance(appdata v0, appdata v1, appdata v2) {
			//float minDist = 20; // 10.0;
			//float maxDist = 700;//25.0;
			//return UnityDistanceBasedTess(v0.vertex, v1.vertex, v2.vertex, minDist, maxDist, _Tess);
			return _Tess;
		}

		sampler2D _Splat;
		float _Displacement;

		void disp(inout appdata v)
		{
			float d = tex2Dlod(_Splat, float4(v.texcoord.xy,0,0)).r * _Displacement;
			if(d > 0)
				v.vertex.xyz -= v.normal * d;
			v.vertex.xyz += v.normal * _Displacement;
		}

        sampler2D _MainTex;
		sampler2D _GroundTex;
		sampler2D _Normal;

        struct Input
        {
			float2 uv_MainTex;
			float2 uv_Normal;
			float2 uv_GroundTex;
			float2 uv_Splat;
        };

        half _Glossiness;
        half _Metallic;
		fixed4 _MainColor;
		fixed4 _GroundColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
			float amount = tex2Dlod(_Splat, float4(IN.uv_Splat, 0, 0)).r;
			if (amount < 0)
				amount = 0;
			
			fixed4 c = lerp(tex2D(_MainTex, IN.uv_MainTex) * _MainColor, tex2D(_GroundTex, IN.uv_GroundTex) * _GroundColor, amount);
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _MainColor;
            o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));//test
			
            // Metallic and smoothness come from slider variables
			o.Metallic = 0; //_Metallic;
			o.Smoothness = 0;// _Glossiness;
			o.Alpha = 1;//c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
