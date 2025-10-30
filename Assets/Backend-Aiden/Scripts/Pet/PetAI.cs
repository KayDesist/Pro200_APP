using LLMUnity;
using UnityEngine;

public class PetAI : MonoBehaviour
{
    [SerializeField] private StringEvent onTranscriptionReceived;
    [SerializeField] private LLMCharacter llmChar;

    private void OnEnable()
    {
        onTranscriptionReceived.Subscribe(TalkToPet);
    }

    private void OnDisable()
    {
        onTranscriptionReceived.Unsubscribe(TalkToPet);
    }

    private void TalkToPet(string input)
    {
        llmChar.Chat(input, HandleResponse);
    }

    private void HandleResponse(string response)
    {
        Debug.Log("Pet Response: " + response);
    }
}
