using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(GlitchEffectRenderer), PostProcessEvent.AfterStack, "Custom/GlitchEffect")]
public sealed class GlitchEffect : PostProcessEffectSettings
{
    [Range(0f, 20f), Tooltip("size")]
    public FloatParameter size = new FloatParameter { value = 0.5f };

    
    [Range(0f, .1f), Tooltip("amount")]
    public FloatParameter amount = new FloatParameter { value = 0.5f };

    [Range(0f, 100f), Tooltip("speed")]
    public FloatParameter speed = new FloatParameter { value = 0.5f };

    
    [Range(0f, 1f), Tooltip("GlitchEffect blend")]
    public FloatParameter blend = new FloatParameter { value = 0.5f };


    
    
    [Range(0f, 5f), Tooltip("GlitchEffect effect intensity.")]
    public FloatParameter split = new FloatParameter { value = 0.5f };




    // Only use if the blend value is greater than 1
    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled.value
            && blend.value > 0f;
    }

}

public sealed class GlitchEffectRenderer : PostProcessEffectRenderer<GlitchEffect>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("PostProcessing/GlitchEffect"));
        sheet.properties.SetFloat("_Blend", settings.blend);
        sheet.properties.SetFloat("_Size", settings.size);
        sheet.properties.SetFloat("_Amount", settings.amount);
        sheet.properties.SetFloat("_Speed", settings.speed);
        sheet.properties.SetFloat("_Split", settings.split);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}