Shader "TowerDefenseKit/CrystalHaloURP"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Size ("Size", Float) = 0.025
        _FallOff ("Fall Off", Range(0, 2)) = 1
        _Edge ("Edge", Range(0, 15)) = 0
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent+2"
            "RenderPipeline"="UniversalPipeline"
        }

        Pass
        {
            Name "FORWARD"
            Tags { "LightMode"="UniversalForward" }
            Blend One One
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            float _Size;
            float _FallOff;
            float _Edge;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                IN.positionOS.xyz += IN.normalOS * (_Size * 0.02 + 0.01);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float3 viewDirection = normalize(_WorldSpaceCameraPos - IN.positionWS);
                float3 normalDirection = normalize(IN.normalWS);

                float3 glow = _Color.rgb * _FallOff * pow(saturate(dot(normalDirection, viewDirection)), _Edge);
                return half4(glow, 1.0);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode"="ShadowCaster" }
            Offset 1, 1

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float _Size;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : TEXCOORD0;
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                IN.positionOS.xyz += IN.normalOS * (_Size * 0.02 + 0.01);
                OUT.positionCS = TransformWorldToHClip(TransformObjectToWorld(IN.positionOS.xyz));
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return 0; // Shadow caster pass doesn't output color
            }
            ENDHLSL
        }
    }
    FallBack "Universal Render Pipeline/Lit"
}