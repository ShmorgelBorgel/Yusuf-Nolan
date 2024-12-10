using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GloveSizer : MonoBehaviour
{
    private AudioSource microphoneSource;
    private AudioSource audioClipSource;

    public AudioClip audioClipToPlayAfterSilence;
    public float silenceThreshold = 0.02f;
    public float firstThreshold = 0.04f;
    public float secondThreshold = 0.06f;
    public float silenceDuration = 2.0f; // Time to detect silence (in seconds)

    [Range(0,0.1f)]
    public float test;

    public bool testing;
    
    public Material firstThresholdMat;
    public Material secondThresholdMat;

    private float silenceTimer = 0.0f;
    private bool isRecording = true;

    public GameObject _glove;
    public Vector3 gloveSize;

    void Start()
    {
        // Get the AudioSource components (one for the microphone, one for the clip to play)
        microphoneSource = GetComponent<AudioSource>();
        audioClipSource = gameObject.AddComponent<AudioSource>();
        StartMicrophone();
    }

    void StartMicrophone()
    {
        if (Microphone.devices.Length > 0)
        {
            string micDevice = Microphone.devices[0];
            microphoneSource.clip = Microphone.Start(micDevice, true, 10, 44100);
            microphoneSource.loop = true;

            while (!(Microphone.GetPosition(micDevice) > 0))
            {
            }

            microphoneSource.Play();
        }
        else
        {
            Debug.Log("No microphone device detected.");
        }
    }

    void Update()
    {
        if (isRecording)
        {
            MonitorMicrophoneInput();
        }
    }

    void MonitorMicrophoneInput()
    {
        // Get microphone audio levels by analyzing the current microphone output
        float averageVolume = test;
        if (!testing)
        {
            float[] samples = new float[256];
            microphoneSource.GetOutputData(samples, 0);


            // Calculate the average volume of the input
            float sum = 0;
            foreach (float sample in samples)
            {
                sum += Mathf.Abs(sample);
            }
            averageVolume = sum / samples.Length;
        }
        
        
        if (averageVolume > secondThreshold)
        {
            gloveSize = new Vector3(8f, 8f, 8f);
            _glove.GetComponentInChildren<MeshRenderer>().material = secondThresholdMat;
            Debug.Log("Volume crossed second threshold!");
        }
        else if (averageVolume > firstThreshold)
        {
            gloveSize = new Vector3(5f, 5f, 5f);
            _glove.GetComponentInChildren<MeshRenderer>().material = firstThresholdMat;
            Debug.Log("Volume crossed first threshold!");
            
        }
        else if (averageVolume > silenceThreshold)
        {
            // Increment the silence timer
            // silenceTimer += Time.deltaTime;

            // If silence has been detected for the defined duration, stop the microphone and play the audio clip
            /*if (silenceTimer >= silenceDuration)
            {
                StopMicrophoneAndPlayClip();
            }*/
        }
        else
        {
            // Reset the silence timer if there's sound
            silenceTimer = 0.0f;
        }
        _glove.transform.localScale = gloveSize;
    }
}
