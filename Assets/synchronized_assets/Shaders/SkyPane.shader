Shader "Universal Render Pipeline/2D/SkyPane"
{
    Properties
    {
        _MainTex("Diffuse", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}

        _Color1 ("Colour 1", Color) = (1,1,1,1)
        _Color2 ("Colour 2", Color) = (0,0,0,1)
        _Color3 ("Colour 3", Color) = (1,1,1,1)
        _Color4 ("Colour 4", Color) = (0,0,0,1)
        _Threshold1 ("Threshold 1", Range(0, 0.5)) = 0.3333
        _Threshold2 ("Threshold 2", Range(0.501, 1)) = 0.6667
        _SunY ("Sun Position Y", Range(0, 1)) = 0.9
        _SunX ("Sun Position X", Range(0, 1)) = 0.9
        _SunR ("Run Radius", Range(0, 1)) = 0.5

        // Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
        [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Tags { "LightMode" = "Universal2D" }

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex CombinedShapeLightVertex
            #pragma fragment CombinedShapeLightFragment

            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
            #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __
            #pragma multi_compile _ DEBUG_DISPLAY

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color        : COLOR;
                float2  uv          : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4  positionCS  : SV_POSITION;
                half4   color       : COLOR;
                float2  uv          : TEXCOORD0;
                half2   lightingUV  : TEXCOORD1;
                #if defined(DEBUG_DISPLAY)
                float3  positionWS  : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_MaskTex);
            SAMPLER(sampler_MaskTex);
            half4 _MainTex_ST;
            float4 _Color;
            half4 _RendererColor;

            #if USE_SHAPE_LIGHT_TYPE_0
            SHAPE_LIGHT(0)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_1
            SHAPE_LIGHT(1)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_2
            SHAPE_LIGHT(2)
            #endif

            #if USE_SHAPE_LIGHT_TYPE_3
            SHAPE_LIGHT(3)
            #endif

            Varyings CombinedShapeLightVertex(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.positionCS = TransformObjectToHClip(v.positionOS);
                #if defined(DEBUG_DISPLAY)
                o.positionWS = TransformObjectToWorld(v.positionOS);
                #endif
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.lightingUV = half2(ComputeScreenPos(o.positionCS / o.positionCS.w).xy);

                o.color = v.color * _Color * _RendererColor;
                return o;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;
            float _Threshold1;
            float _Threshold2;
            float _SunY;
            float _SunX;
            float _SunR;

            float4 maskSun(Varyings i, float2 o, float r) {
                float2 posWorld = mul(unity_ObjectToWorld, i.uv);
                float2 sunPosWorld = mul(unity_ObjectToWorld, o);

                float radius_wave = sin(r*_Time*100)/40;
                float ratio = 1 - step(r, length(posWorld - sunPosWorld) + radius_wave);
                float4 col = (1,1,1,1)*ratio - (1,1,1,0)*(1- ratio);

                radius_wave = sin(r*_Time*50 + 10)/30;
                float corona1 = 1 - step(1.5*r, length(posWorld - sunPosWorld) + radius_wave);
                float4 c1col = (1,1,1,0.6) * corona1 - (1,1,1,0) * (1-corona1);
                col = max(col, c1col);

                radius_wave = sin(r*_Time*25 + 10)/20;
                float corona2 = 1 - step(2*r, length(posWorld - sunPosWorld) + radius_wave);
                float4 c2col = (1,1,1,0.3) * corona2 - (1,1,1,0) * (1-corona2);
                col = max(col, c2col);
                
                return col;
            }

            half4 CombinedShapeLightFragment(Varyings i) : SV_Target
            {
                float4 col = i.color * SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.uv);

                // basic gradient
                float factor = 3;
                float ratio = 0;
                float4 col1 = _Color1;
                float4 col2 = _Color2;
                if (i.uv.y < _Threshold1) {
                    ratio = i.uv.y*factor;
                } else if (i.uv.y > _Threshold2) {
                    ratio = (i.uv.y - _Threshold2) * factor;
                    col1 = _Color3;
                    col2 = _Color4;
                } else {
                    ratio = (i.uv.y - _Threshold1) * factor;
                    col1 = _Color2;
                    col2 = _Color3;
                }

                col = col2 * ratio + col1 * (1 - ratio);

                
                ratio = maskSun(i, (_SunX, _SunY), _SunR);
                col = (1,1,1,1) * ratio + col * (1-ratio);

                const half4 main = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                const half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
                SurfaceData2D surfaceData;
                InputData2D inputData;

                InitializeSurfaceData(col.rgb, col.a, mask, surfaceData);
                InitializeInputData(i.uv, i.lightingUV, inputData);

                return CombinedShapeLightShared(surfaceData, inputData);
            }
            ENDHLSL
        }

        Pass
        {
            Tags { "LightMode" = "NormalsRendering"}

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex NormalsRenderingVertex
            #pragma fragment NormalsRenderingFragment

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
                float4 tangent      : TANGENT;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4  positionCS      : SV_POSITION;
                half4   color           : COLOR;
                float2  uv              : TEXCOORD0;
                half3   normalWS        : TEXCOORD1;
                half3   tangentWS       : TEXCOORD2;
                half3   bitangentWS     : TEXCOORD3;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_NormalMap);
            SAMPLER(sampler_NormalMap);
            half4 _NormalMap_ST;  // Is this the right way to do this?

            Varyings NormalsRenderingVertex(Attributes attributes)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(attributes);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.positionCS = TransformObjectToHClip(attributes.positionOS);
                o.uv = TRANSFORM_TEX(attributes.uv, _NormalMap);
                o.color = attributes.color;
                o.normalWS = -GetViewForwardDir();
                o.tangentWS = TransformObjectToWorldDir(attributes.tangent.xyz);
                o.bitangentWS = cross(o.normalWS, o.tangentWS) * attributes.tangent.w;
                return o;
            }

            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

            half4 NormalsRenderingFragment(Varyings i) : SV_Target
            {
                const half4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                const half3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uv));

                return NormalsRenderingShared(mainTex, normalTS, i.tangentWS.xyz, i.bitangentWS.xyz, i.normalWS.xyz);
            }
            ENDHLSL
        }

        Pass
        {
            Tags { "LightMode" = "UniversalForward" "Queue"="Transparent" "RenderType"="Transparent"}

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex UnlitVertex
            #pragma fragment UnlitFragment

            struct Attributes
            {
                float3 positionOS   : POSITION;
                float4 color        : COLOR;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4  positionCS      : SV_POSITION;
                float4  color           : COLOR;
                float2  uv              : TEXCOORD0;
                #if defined(DEBUG_DISPLAY)
                float3  positionWS  : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;
            half4 _RendererColor;

            Varyings UnlitVertex(Attributes attributes)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(attributes);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.positionCS = TransformObjectToHClip(attributes.positionOS);
                #if defined(DEBUG_DISPLAY)
                o.positionWS = TransformObjectToWorld(v.positionOS);
                #endif
                o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
                o.color = attributes.color * _Color * _RendererColor;
                return o;
            }

            float4 _Color1;
            float4 _Color2;
            float4 _Color3;
            float4 _Color4;
            float _Threshold1;
            float _Threshold2;

            float4 UnlitFragment(Varyings i) : SV_Target
            {
                /*
                float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                #if defined(DEBUG_DISPLAY)
                SurfaceData2D surfaceData;
                InputData2D inputData;
                half4 debugColor = 0;

                InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
                InitializeInputData(i.uv, inputData);
                SETUP_DEBUG_DATA_2D(inputData, i.positionWS);

                if(CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
                {
                    return debugColor;
                }
                #endif

                return mainTex;
                */

                // sample the texture
                float4 col = i.color * SAMPLE_TEXTURE2D(_MainTex,sampler_MainTex, i.uv);

                return _Color1;

                // basic gradient
                float factor = 3;
                float ratio = 0;
                float4 col1 = _Color1;
                float4 col2 = _Color2;
                if (i.uv.y < _Threshold1) {
                    ratio = i.uv.y*factor;
                } else if (i.uv.y > _Threshold2) {
                    ratio = (i.uv.y - _Threshold2) * factor;
                    col1 = _Color3;
                    col2 = _Color4;
                } else {
                    ratio = (i.uv.y - _Threshold1) * factor;
                    col1 = _Color2;
                    col2 = _Color3;
                }

                col = col2 * ratio + col1 * (1 - ratio);

                return col;
            }
            ENDHLSL
        }
    }

    Fallback "Sprites/Default"
}
