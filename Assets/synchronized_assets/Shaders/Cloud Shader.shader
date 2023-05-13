Shader "Unlit/Cloud Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorSun ("Sun Color", Color) = (1,1,1,1)
        _ColorShadow ("Shadow Color", Color) = (0,0,0,1)
        _ColorBounce ("Bounce Light Color", Color) = (0,0,0,1)
        _BounceFactor ("Inverse Bounce Factor", Range(1, 10)) = 1
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Opaque"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        LOD 100
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

            float4 _ColorShadow;
            float4 _ColorSun;
            float4 _ColorBounce;
            float _BounceFactor;

            fixed4 frag (v2f i) : SV_Target
            {
                float4 posWorld = mul(unity_ObjectToWorld, i.uv);
                // float s = 0.4 - 16*0.4 * ((i.uv.x - 0.5) * (i.uv.x -0.5) * (i.uv.x - 0.5) * (i.uv.x -0.5));
                float s = (sin(i.uv.y)/10 + sin(posWorld.x/4 + _Time))/1.75;
                s *= 0.9 + (sin(i.uv.y) + sin(posWorld.x*2))/4;
                s += (sin(posWorld.y) + sin(posWorld.x*5 + _Time*10))/40;
                s += (sin(posWorld.y) + sin(posWorld.x*5 + _Time*44))/100;

                //s *= ov;

                if (i.uv.y > s) {
                    return (0,0,0,0);
                }

                // sample the texture
                float4 col = tex2D(_MainTex, i.uv);
                
                col = _ColorSun * (i.uv.y) + _ColorShadow * (1 - i.uv.y);
                col.a = 1 - i.uv.y;
                float bounce_factor = _BounceFactor;
                col = _ColorBounce * (col.a/bounce_factor) + col * (1 - col.a/bounce_factor);
                // apply fog
                // UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
