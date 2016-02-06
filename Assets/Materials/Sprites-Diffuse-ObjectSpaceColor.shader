Shader "Sprites/Diffuse-VertexColor"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _Color("Tint", Color) = (1,1,1,1)
		_Color2("ObjectSpace", Color) = (1,1,1,1)
		VertexEffect("Effect Strength", Range(0,1)) = 1
		VertexEffectScale("Effect Scale", Range(8,10)) = 1
		[PerRendererData] _ObjectPosition("ObjectPosition", Vector) = (0,0,0)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			CGPROGRAM
			#pragma target 3.0
			#pragma surface surf Lambert vertex:vert nofog keepalpha
			#pragma multi_compile _ PIXELSNAP_ON

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _Color2;
			fixed3 _ObjectPosition;
			float VertexEffect;
			float VertexEffectScale;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			struct Input
			{
				float2 uv_MainTex;
				fixed4 color;
				fixed4 color2;
			};

			void vert(inout appdata_full v, out Input o)
			{
				#if defined(PIXELSNAP_ON)
				v.vertex = UnityPixelSnap(v.vertex);
				#endif

				UNITY_INITIALIZE_OUTPUT(Input, o);
				float lerpv = (1 - max(min((mul(_Object2World, v.vertex).xyz.y - _ObjectPosition.y) * (10 - VertexEffectScale), 1), 0));

				o.color = v.color * _Color;
				o.color2 = _Color2;
				o.color2.a = lerpv * VertexEffect;
			}

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

	#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D(_AlphaTex, uv).r;
	#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				float lerpv = IN.color2.a;
				fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
				IN.color2.a = c.a;
				c = lerp(c, IN.color2, lerpv);
				o.Albedo = c.rgb * c.a;
				o.Alpha = c.a;
			}
			ENDCG
		}

			Fallback "Transparent/VertexLit"
}