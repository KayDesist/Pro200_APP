using System.Collections;
using UnityEngine;

public class VoiceRecorder : MonoBehaviour
{
    private VoiceToText voiceToText;
    private AudioClip audioClip;
    private const int sampleRate = 16000;
    private int lastSamplePosition = 0;
    private bool isRecording = false;
    private const float chunkDuration = 0.1f; // 100ms

    void Start()
    {
        voiceToText = new VoiceToText();
    }

    public void StartRecording()
    {
        if (isRecording) return;
        audioClip = Microphone.Start(null, true, 10, sampleRate); // 10 seconds looping buffer
        lastSamplePosition = 0;
        isRecording = true;
        Debug.Log("Recording started.");
        StartCoroutine(StreamAudioCoroutine());
    }

    public void StopRecording()
    {
        if (!isRecording) return;
        isRecording = false;
        Microphone.End(null);
        Debug.Log("Recording stopped.");
        StopCoroutine(StreamAudioCoroutine());
    }

    private IEnumerator StreamAudioCoroutine()
    {
        while (isRecording)
        {
            int currentPosition = Microphone.GetPosition(null);
            int samplesToRead = currentPosition - lastSamplePosition;
            if (samplesToRead < 0)
            {
                // Buffer wrapped
                samplesToRead += audioClip.samples;
            }

            if (samplesToRead > 0)
            {
                float[] samples = new float[samplesToRead * audioClip.channels];
                audioClip.GetData(samples, lastSamplePosition);

                // Convert float samples to 16-bit PCM
                byte[] audioData = new byte[samples.Length * 2];
                int index = 0;
                foreach (var sample in samples)
                {
                    short intSample = (short)(sample * short.MaxValue);
                    audioData[index++] = (byte)(intSample & 0xFF);
                    audioData[index++] = (byte)((intSample >> 8) & 0xFF);
                }

                // Stream to recognizer (if supported)
                string result = voiceToText.Recognize(audioData, audioData.Length);
                Debug.Log("Recognition Result: " + result);

                lastSamplePosition = currentPosition;
            }

            yield return new WaitForSeconds(chunkDuration);
        }
    }
}
