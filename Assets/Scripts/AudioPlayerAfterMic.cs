using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioPlayerAfterMic : MonoBehaviour
{
    private AudioSource microphoneSource;
    private AudioSource audioClipSource;

    public AudioClip audioClipToPlayAfterSilence;
    public float silenceThreshold = 0.02f;  // Adjust as needed for sensitivity
    public float firstThreshold = 0.04f;
    public float secondThreshold = 0.06f;
    public float silenceDuration = 2.0f;    // Time to detect silence (in seconds)

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

            while (!(Microphone.GetPosition(micDevice) > 0)) { }
            
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
        float[] samples = new float[256];
        microphoneSource.GetOutputData(samples, 0);

        // Calculate the average volume of the input
        float sum = 0;
        foreach (float sample in samples)
        {
            sum += Mathf.Abs(sample);
        }
        float averageVolume = sum / samples.Length;

        // Check if the input volume is below the silence threshold
        if (averageVolume > silenceThreshold)
        {
            // Increment the silence timer
            silenceTimer += Time.deltaTime;

            // If silence has been detected for the defined duration, stop the microphone and play the audio clip
            /*if (silenceTimer >= silenceDuration)
            {
                StopMicrophoneAndPlayClip();
            }*/
        }

        if (averageVolume > firstThreshold)
        {
            gloveSize = new Vector3(+0.5f, 0.5f, 0.5f);
            _glove.GetComponent<MeshRenderer>().material = firstThresholdMat;

        }

        if (averageVolume > secondThreshold)
        {
            gloveSize = new Vector3(+0.03f, 0.03f, 0.03f);
        }
        
        
        
        else
        {
            // Reset the silence timer if there's sound
            silenceTimer = 0.0f;
            
        }
    }

    /*void StopMicrophoneAndPlayClip()
    {
        // Stop recording from the microphone
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            microphoneSource.Stop();
            isRecording = false;
        }

        // Play the audio clip after the microphone input stops
        if (audioClipToPlayAfterSilence != null)
        {
            audioClipSource.clip = audioClipToPlayAfterSilence;
            audioClipSource.Play();
        }
    }*/
}
