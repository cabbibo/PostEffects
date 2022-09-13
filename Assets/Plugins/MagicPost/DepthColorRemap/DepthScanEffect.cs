using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(DepthScanEffectRenderer), PostProcessEvent.BeforeStack, "Custom/DepthScanEffect")]
public sealed class DepthScanEffect : PostProcessEffectSettings
{


    public ColorParameter color = new ColorParameter{ value = Color.red };
    
    [Range(0f, 20f), Tooltip("size")]
    public FloatParameter size = new FloatParameter { value = 0.5f };


    [Range(0f, 20f), Tooltip("speed")]
    public FloatParameter speed = new FloatParameter { value = 0.5f };

        [Range(0f, 20f), Tooltip("split")]
    public FloatParameter split = new FloatParameter { value = 0.5f };



    
    [Range(0f, 20f), Tooltip("blend")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };

    
    [Range(0f, 1000f), Tooltip("amount")]
    public FloatParameter amount = new FloatParameter { value = 0.5f };

    
    [Range(0f, 1000f), Tooltip("band size")]
    public FloatParameter bandSize = new FloatParameter { value = 0.5f };

    [Range(0f, 1000f), Tooltip("scanFade")]
    public FloatParameter scanFade = new FloatParameter { value = 0.5f };


    // Only use if the blend value is greater than 1
    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled.value
            && blend.value > 0f;
    }

}

public sealed class DepthScanEffectRenderer : PostProcessEffectRenderer<DepthScanEffect>
{
    public override void Render(PostProcessRenderContext context)
    {

        var sheet = context.propertySheets.Get(Shader.Find("PostProcessing/DepthScanEffect"));

        	// More clerification of whats going on is needed!
		var p = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, false);// Unity flips its 'Y' vector depending on if its in VR, Editor view or game view etc... (facepalm)
		p[2, 3] = p[3, 2] = 0.0f;
		p[3, 3] = 1.0f;
		var clipToWorld = Matrix4x4.Inverse(p * context.camera.worldToCameraMatrix) * Matrix4x4.TRS(new Vector3(0, 0, -p[2,2]), Quaternion.identity, Vector3.one);
		sheet.properties.SetMatrix("_ClipToWorld", clipToWorld);

        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetFloat("_Size", settings.size);
        sheet.properties.SetFloat("_Split", settings.split);
        sheet.properties.SetFloat("_Amount", settings.amount);
        sheet.properties.SetFloat("_BandSize", settings.bandSize);
        sheet.properties.SetFloat("_ScanFade", settings.scanFade);
        sheet.properties.SetFloat("_CameraNear",context.camera.nearClipPlane );
        sheet.properties.SetFloat("_CameraFar",context.camera.farClipPlane );
        sheet.properties.SetFloat("_Speed",settings.speed);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}