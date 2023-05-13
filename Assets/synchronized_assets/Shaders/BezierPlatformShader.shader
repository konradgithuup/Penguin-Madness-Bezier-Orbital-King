Shader "Unlit/BezierPlatformShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color1 ("Colour 1", Color) = (1,1,1,1)
        _Color2 ("Colour 2", Color) = (0,0,0,1)
        _Color3 ("Colour 3", Color) = (0,0,0,1)
        _GradientShift ("Gradient Shift", Range(-1,1)) = 0
        _Anchor1 ("Anchor 1 (Relative Height)", Range(0,1)) = 0.8
        _Anchor2 ("Anchor 2 (Relative Height)", Range(0,1)) = 0.8
        _Control1 ("Anchor 2 (Relative Height)", Range(0,1)) = 0.5
        _Control2 ("Anchor 2 (Relative Height)", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags {"RenderType"="Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

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

            fixed _Anchor1;
            fixed _Anchor2;
            fixed _Control1;
            fixed _Control2;

            half2 lerpVec(fixed2 a, fixed2 b, half t)
            {
                return (b - a)*t + a;
            }

            half2 querp(fixed2 a, fixed2 b, fixed2 c, half t)
            {
                fixed2 p0 = lerpVec(a, b, t);
                fixed2 p1 = lerpVec(b, c, t);
                return lerpVec(p0, p1, t);
            }

            half2 cerp(fixed2 a, fixed2 b, fixed2 c, fixed2 d, half t)
            {
                fixed p0 = querp(a, b, c, t);
                fixed p1 = querp(b, c, d, t);

                return lerpVec(p0, p1, t);
            }

            half surface(half x)
            {
                float2 a = (0, _Anchor1);
                float2 d = (1, _Anchor2);
                float2 b = (1/3, _Control1);
                float2 c = (2/3, _Control2);
                return cerp(a, b, c, d, x).y;
            }

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
            fixed4 _Color3;
            half _GradientShift;
            float4 _MainTex_TexelSize;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                half4 c = tex2D(_MainTex, i.uv);

                // cut off at surface
                half s = surface(i.uv.x);
                if (i.uv.y > s) {
                    return (1,1,1,0);
                }

                // apply gradient
                half ratio = (i.uv.y/s) + _GradientShift;  
                float4 col = _Color1 * (ratio) + _Color2 * (1 - ratio);

                half wave = i.uv.x;
                ratio = wave*0.5/(1-(i.uv.y/s));
                col = col * (1 - ratio) + _Color3 * (ratio);

                c.rgb = col;

                return c;
            }
            ENDCG
        }
    }
}
