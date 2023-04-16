using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Range(1,20000)]  //Creates a slider in the inspector
    public float frequency1;
 
    [Range(1,20000)]  //Creates a slider in the inspector
    public float frequency2;

    [Range(0,1)]
    public float gain = 1f;
 
    public float sampleRate = 44100;
    public float waveLengthInSeconds = 2.0f;
 
    AudioSource audioSource;
    int timeIndex = 0;

    float timer = 0.2f;
    float pause = 0.1f;
 
    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Play(); //avoids audiosource from starting to play automatically
    }

    // Update is called once per frame
    void Update()
    {
            if(timer>=0.0f)
            {
                audioSource.volume = 0.0f;
                timer -= Time.deltaTime;
            }else
            {
                audioSource.volume = 0.7f;
                pause -= Time.deltaTime;
                if(pause<=0.0f){
                    timer = 0.2f;
                    pause = 0.1f;
                }
            }

    }
   
    void OnAudioFilterRead(float[] data, int channels)
    {
        for(int i = 0; i < data.Length; i+= channels)
        {          
            data[i] = CreateSine(timeIndex, frequency1, sampleRate);
           
            if(channels == 2)
                data[i+1] = CreateSine(timeIndex, frequency2, sampleRate);
           
            timeIndex++;
           
            //if timeIndex gets too big, reset it to 0
            if(timeIndex >= (sampleRate * waveLengthInSeconds))
            {
                timeIndex = 0;
            }
        }
        
    }
   
    //Creates a sinewave
    public float CreateSine(int timeIndex, float frequency, float sampleRate)
    {
        return Mathf.Sin(2 * Mathf.PI * timeIndex * frequency / sampleRate);
    }

    
}
