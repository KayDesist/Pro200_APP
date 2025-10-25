using Vosk;

public class VoiceToText
{
    public VoiceToText()
    {
        modelPath = new Model(@"Assets\Vosk\vosk-model-small");
        recognizer = new VoskRecognizer(modelPath, 16000.0f);
    }

    public string Recognize(byte[] audioData, int length)
    {
        if (recognizer.AcceptWaveform(audioData, length))
        {
            return recognizer.Result();
        }
        else
        {
            return recognizer.PartialResult();
        }
    }

    private Model modelPath;
    private VoskRecognizer recognizer;
}
