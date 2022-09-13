Shader "PostProcessing/DepthScanEffect"
{
    HLSLINCLUDE

        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl" 

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        TEXTURE2D_SAMPLER2D(_ColorMap, sampler_ColorMap);

        
		TEXTURE2D_SAMPLER2D(_CameraNormalsTexture, sampler_CameraNormalsTexture);
		TEXTURE2D_SAMPLER2D(_CameraDepthTexture, sampler_CameraDepthTexture);
		TEXTURE2D_SAMPLER2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture);
        
        float _Blend;
        float _Amount;
        float _Size;
        float _BandSize;
        float _ScanFade;
        float _StartTime;

        float _CameraNear;
        float _CameraFar;

        float4x4 _ClipToWorld;

        

        float4 Frag(VaryingsDefault i) : SV_Target
        {
                float4 clip = float4(i.vertex.xy, 0.0, 1.0);
				float3 worldDirection = mul(_ClipToWorld, clip) - _WorldSpaceCameraPos;

            float4 depthNor = SAMPLE_DEPTH_TEXTURE(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, i.texcoord);
            float depth      = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, i.texcoord);
            float4 nor    = SAMPLE_TEXTURE2D(_CameraNormalsTexture, sampler_CameraNormalsTexture, i.texcoord);

            depth = LinearEyeDepth(depth);


            //depth = _Amount * 10000 - depth;

            //depth = (depth - _CameraNear) / ( _CameraFar - _CameraNear );



              // float3 worldspace = worldDirection * depth + _WorldSpaceCameraPos;
            float4 color = 1;
            //color.rgba =  sin(depth);//worldspace * .0001;// nor * sin(depth * 1000+ _Time.y* 10);//linearEyeDepth;//sin( depth * 10000) ;//1 - abs( (_Time.y * 10 %10) - nor1 * 1000);// *100;

            //color.rgb = nor2 * nor2 * nor2;

            float4 bgCol = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
            /*color = lerp( color , 1 , sin(depth));

            if( depth < 0 ){
                color = 1;
            }else{
                color = 0;
            }*/



            float depthDelta = abs(depth-_Amount);

            if( depthDelta < _Size){
                

                float val = sin(depth * _BandSize * 6.28);
                color = lerp( bgCol , 1 , saturate(val));
            }else{
                color = bgCol;
            }

            //color = (depth - _Amount) * 100;//_Amount * depth;
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