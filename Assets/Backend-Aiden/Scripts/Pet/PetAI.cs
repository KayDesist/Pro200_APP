using LLMUnity;
using UnityEngine;

public class PetAI : MonoBehaviour
{
    [SerializeField] private StringEvent onTranscriptionReceived;
    [SerializeField] private LLMCharacter llmChar;
    private PetController petController;

    private void Awake()
    {
        petController = GetComponent<PetController>();
    }

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

        PetEmotion newEmotion = PetEmotion.Neutral;
        if (response.ToLower().Equals("happy"))
        {
            newEmotion = PetEmotion.Happy;
        }
        else if (response.ToLower().Equals("sad"))
        {
            newEmotion = PetEmotion.Sad;
        }
        else if (response.ToLower().Equals("angry"))
        {
            newEmotion = PetEmotion.Angry;
        }
        else if (response.ToLower().Equals("confused"))
        {
            newEmotion = PetEmotion.Confused;
        }

        petController.SetEmotion(newEmotion);
    }
}
