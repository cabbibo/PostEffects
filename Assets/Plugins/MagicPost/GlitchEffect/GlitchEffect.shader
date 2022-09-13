Shader "PostProcessing/GlitchEffect"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        float _Blend;
        float _Amount;
        float _Size;
        float _Speed;
        float _Split;

        float4 Frag(VaryingsDefault i) : SV_Target
        {


            float2 uvR = i.texcoord;
            float2 uvG = i.texcoord;
            float2 uvB = i.texcoord;

            uvR.x += sin( floor(uvR.y * 80  * _Size + _Time.y * _Speed ) ) * _Amount * (.1 + .1 * _Split);
            uvR.x += sin( floor(uvR.y * 27  * _Size + _Time.y * _Speed ) ) * _Amount * (.6 + .2 * _Split);
            uvR.x += sin( floor(uvR.y * 8   * _Size + _Time.y * _Speed ) ) * _Amount * ( 1 + .3 * _Split);

            uvG.x += sin( floor(uvR.y * 80  * _Size + _Time.y * _Speed ) ) * _Amount * (.1 + .2 * _Split);
            uvG.x += sin( floor(uvR.y * 27  * _Size + _Time.y * _Speed ) ) * _Amount * (.6 + .4 * _Split);
            uvG.x += sin( floor(uvR.y * 8   * _Size + _Time.y * _Speed ) ) * _Amount * ( 1 + .6 * _Split);

            
            uvB.x += sin( floor(uvR.y * 80  * _Size + _Time.y * _Speed ) ) * _Amount * (.1 + .3 * _Split);
            uvB.x += sin( floor(uvR.y * 27  * _Size + _Time.y * _Speed ) ) * _Amount * (.6 + .6 * _Split);
            uvB.x += sin( floor(uvR.y * 8   * _Size + _Time.y * _Speed ) ) * _Amount * ( 1 + .9 * _Split);

            float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvR);
            color.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvG).g;
            color.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvB).b;

            float4 bgCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            color= lerp(bgCol, color , _Blend );
            //float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
            //float luminance = dot(color.rgb, float3(0.2126729, 0.7151522, 0.0721750));
            //color.rgb = color.rgb;//lerp(color.rgb, luminance.xxx, _Blend.xxx);
            return color;
        }

    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}