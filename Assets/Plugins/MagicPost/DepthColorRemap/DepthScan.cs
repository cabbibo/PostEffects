using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
 


[ExecuteAlways]
public class DepthScan : MonoBehaviour
{

    public float amount = .01f;
    public float speed = .1f;
    public float size = 10 ;
    public float split = 1 ;
    public float blend = 1;
    public float scanLength = 1;
 
    public PostProcessVolume volume;
    private DepthScanEffect scan;

    private bool inScan =false;
    private float scanStartTime;


    public bool debug;
    void OnEnable()
    {

        // Getting our settings to alter
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out scan);

        // dont start in scan
        inScan = false;
    }
 
    void Update()
    {


        // if for some reason we dont have the scan settings
        // ( incorrect volume or something )
        if( scan != null ){

            if( inScan ){

                float v = (Time.time - scanStartTime) / scanLength;

                if( v > 1 ){ v=1; }
                
                // Getting a value from 0 -> 1  to define the 'power' of the scan
                // it turns on REAL quick, then turns off slower 
                float scanStrength = Mathf.Pow(Mathf.Min(v * 40, (1-v)) ,4);
                
                scan.amount.value = v;//scanStrength *  amount;
                if( v==1){ EndScan(); }

            }else{
                scan.amount.value =0;
            }


            scan.speed.value = speed;
            scan.size.value = size;
            scan.split.value = split;
            scan.blend.value = blend;

            if( inScan == false && debug == true){
                StartScan();
            }

        }

    }

    public void StartScan(){

        inScan = true;
        scanStartTime = Time.time;

    }

    public void EndScan(){
        inScan = false;
    }
 
}

