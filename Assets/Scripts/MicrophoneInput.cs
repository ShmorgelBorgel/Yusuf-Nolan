using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneInput : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartMicrophone();
    }

    void StartMicrophone()
    {
        // Get a microphone device (check if any is available)
        if (Microphone.devices.Length > 0)
        {
            string micDevice = Microphone.devices[0];
            audioSource.clip = Microphone.Start(micDevice, true, 10, 44100);
            audioSource.loop = true;

            // Wait until the microphone starts recording
            while (!(Microphone.GetPosition(micDevice) > 0)) { }

            // Play the audio source to hear the microphone input
            audioSource.Play();
        }
        else
        {
            Debug.Log("No microphone device detected.");
        }
    }

    void StopMicrophone()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            audioSource.Stop();
        }
    }
}
