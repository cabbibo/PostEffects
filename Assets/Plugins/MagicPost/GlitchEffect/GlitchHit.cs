using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
 


[ExecuteAlways]
public class GlitchHit : MonoBehaviour
{

    public float amount = .01f;
    public float speed = .1f;
    public float size = 10 ;
    public float split = 1 ;
    public float blend = 1;
    public float glitchLength = 1;
 
    public PostProcessVolume volume;
    private GlitchEffect glitch;

    private bool inGlitch =false;
    private float glitchStartTime;


    public bool debug;
    void OnEnable()
    {

        // Getting our settings to alter
        volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out glitch);

        // dont start in glitch
        inGlitch = false;
    }
 
    void Update()
    {


        // if for some reason we dont have the glitch settings
        // ( incorrect volume or something )
        if( glitch != null ){

            if( inGlitch ){

                float v = (Time.time - glitchStartTime) / glitchLength;

                if( v > 1 ){ v=1; }
                
                // Getting a value from 0 -> 1  to define the 'power' of the glitch
                // it turns on REAL quick, then turns off slower 
                float glitchStrength = Mathf.Pow(Mathf.Min(v * 40, (1-v)) ,4);
                
                glitch.amount.value =glitchStrength *  amount;
                if( v==1){ EndGlitch(); }

            }else{
                glitch.amount.value =0;
            }


            glitch.speed.value = speed;
            glitch.size.value = size;
            glitch.split.value = split;
            glitch.blend.value = blend;

            if( inGlitch == false && debug == true){
                StartGlitch();
            }

        }

    }

    public void StartGlitch(){

        inGlitch = true;
        glitchStartTime = Time.time;

    }

    public void EndGlitch(){
        inGlitch = false;
    }
 
}

