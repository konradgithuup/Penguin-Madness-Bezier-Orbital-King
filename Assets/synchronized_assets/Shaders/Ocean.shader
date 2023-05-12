Shader "Unlit/OceanShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Colour 1", Color) = (1,1,1,1)
        _Color2 ("Colour 2", Color) = (0,0,0,1)
        _Speed ("Speed", Float) = 10
        _GradientShift ("Gradient Shift", Range(-1,1)) = 0
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
            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.pos);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 _Color1;
            fixed4 _Color2;
            float _Speed;
            half _GradientShift;
            float4 _MainTex_TexelSize;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 c = tex2D(_MainTex, i.uv);

                // apply cutoff
                float4 posWorld = mul(unity_ObjectToWorld, i.uv);
                float wave = 0.5+((cos(posWorld.y) + cos(posWorld.x * 5 + _Time * _Speed)))/50;
                if (wave < i.uv.y) {
                    return (0,0,0,0);
                }

                // apply gradient
                half ratio = (i.uv.y/wave) + _GradientShift;
                float4 col = _Color1 * (ratio) + _Color2 * (1 - ratio);

                float _Alpha;
   
                c *= col;
                c.a = ratio*0.5 + (1 - ratio)*1.0;
   
                return c;
            }
            ENDCG
        }
    }
}
