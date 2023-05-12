Shader "Unlit/NewUnlitShader"
{
      Properties
      {
          [NoScaleOffset] _MainTex ("Texture Image", 2D) = "white" {}
          _ColorTint ("Tint", Color) = (1.0, 0.6, 0.6, 1.0)
          _Alpha("Alpha", Range(0,1)) = 0.5
      }
   
      SubShader
      {
          Tags
          {
              "RenderType" = "Opaque"
              "IgnoreProjector" = "True"
          }
   
          LOD 200
   
          CGINCLUDE
          #include "UnityCG.cginc"
   
          struct appdata
          {
              float4 vertex : POSITION;
              float2 uv : TEXCOORD0;
          };
   
          struct v2f
          {
              float2 uv : TEXCOORD0;
              float4 vertex : SV_POSITION;
          };
   
          sampler2D _MainTex;
          float4 _MainTex_ST;
   
          fixed4 _ColorTint;
          float _Alpha;
   
          v2f vert(appdata v)
          {
              v2f o;
              o.vertex = UnityObjectToClipPos(v.vertex);
              o.uv = TRANSFORM_TEX(v.uv, _MainTex);
              return o;
          }
   
          fixed4 frag(v2f i) : SV_Target
          {
              fixed4 c = tex2D(_MainTex, i.uv);
              c *= _ColorTint;
              c.a = _Alpha;
   
              return c;
          }
   
          ENDCG
   
          Pass
          {
              ZWrite On
              Cull Front
              ColorMask 0
          }
   
          Pass
          {
              ZWrite Off
              Cull Back
              Blend SrcAlpha OneMinusSrcAlpha
   
              CGPROGRAM
              #pragma vertex vert
              #pragma fragment frag
              #include "UnityCG.cginc"
              ENDCG
          }
      }
      FallBack "Diffuse"
  }
