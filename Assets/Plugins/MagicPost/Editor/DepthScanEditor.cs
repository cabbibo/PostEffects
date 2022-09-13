
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DepthScan))]
public class DepthScanEditor : Editor 
{
    public override void OnInspectorGUI()
    { 
        DepthScan myTarget = (DepthScan)target;



    
        if(GUILayout.Button("Do Scan"))
        {
            myTarget.StartScan();
        }

        
        DrawDefaultInspector();
    }
}
