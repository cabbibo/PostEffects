using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GlitchHit))]
public class GlitchHitEditor : Editor 
{
    public override void OnInspectorGUI()
    { 
        GlitchHit myTarget = (GlitchHit)target;



    
        if(GUILayout.Button("Do GlitchHit"))
        {
            myTarget.StartGlitch();
        }

        
        DrawDefaultInspector();
    }
}

