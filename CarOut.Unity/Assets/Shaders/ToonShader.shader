Shader "Unlit/ToonShader"
{
    Properties
    {
        
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="Universal Pipeline" }

        Pass
        {
            HLSLPROGRAM

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag

            struct App
            {
                float4 positionOS: POSITION;
                half3 normal : NORMAL;
            };

            struct v2f
            {
                float4 positionHCS : SV_POSITION;
                half3 normal : TEXCOORD0;
                half3 worldPos : TEXCOORD1;
                half3 viewDir : TEXCOORD2;
            };

            v2f vert(App IN)
            {
                v2f OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normal = TransformObjectToWorldNormal(IN.normal);
                OUT.worldPos = mul(unity_ObjectToWorld, IN.positionOS);
                OUT.viewDir = normalize(GetWorldSpaceViewDir(OUT.worldPos));
                
                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                float dotProduct = dot(IN.normal, IN.viewDir);
                dotProduct = step(0.5, dotProduct);
                half3 fillColor = IN.normal * 0.5 + 0.5;
                half3 finalColor = fillColor;
                return half4(finalColor, 1.0);
            }
            
            ENDHLSL
        }
    }
}
