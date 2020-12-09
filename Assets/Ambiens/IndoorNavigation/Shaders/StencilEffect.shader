﻿Shader "Custom/StencilEffect" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
			Tags { "RenderType" = "Opaque"}

			Stencil {
				Ref 0
				Comp LEqual
				Pass IncrSat
			}

			CGPROGRAM
			#pragma surface surf Lambert

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			void surf(Input IN, inout SurfaceOutput o) {
				half4 c = tex2D(_MainTex, IN.uv_MainTex);
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
			ENDCG
	}
		FallBack "Diffuse"
}