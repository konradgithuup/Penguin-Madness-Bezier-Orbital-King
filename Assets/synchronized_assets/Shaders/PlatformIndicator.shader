Shader "Unlit/PlatformIndicator"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Highlight ("Color", Color) = (0,0,0,0)
        _Intensity ("Intensity", Range(0,1)) = 0
    }

    SubShader
    {
                Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 _Highlight;
            float _Intensity;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                float ratio = 0.4 - 16*0.4 * ((i.uv.x-0.5) * (i.uv.x-0.5) * (i.uv.x-0.5) * (i.uv.x-0.5));

                float a = (1 - i.uv.y) * _Intensity;
                ratio = 0.5 - (i.uv.x-0.5) * (i.uv.x-0.5);
                col = ratio * (1,1,1,1) + (1 - ratio) * _Highlight;
                col.a = a;

                return col;
            }
            ENDCG
        }
    }
}
