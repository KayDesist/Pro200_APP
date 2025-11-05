using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class GameController : MonoBehaviour
{
    [Header("Room Management")]
    public RoomEnum CurrentRoom { get; private set; }
    [SerializeField] private RoomEnum startingRoom;
    [SerializeField] private SpriteRenderer backgroundRenderer;

    [SerializeField] private Sprite bedRoomBackground;
    [SerializeField] private Sprite kitchenBackground;
    [SerializeField] private Sprite yardBackground;
    [SerializeField] private Sprite bathroomBackground;

    [Header("Inventory Management")]
    public Inventory inventory { get => _inventory; }
    [SerializeField] private Inventory _inventory;

    private VoiceRecorder voiceRecorder;

    void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    void Start()
    {
        voiceRecorder = GetComponent<VoiceRecorder>();

        CurrentRoom = startingRoom;
    }

    private void Update()
    {
        if (Touch.activeTouches.Count > 0)
        {
            if (Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                voiceRecorder.StartRecording();
            }
            else if (Touch.activeTouches[0].phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                voiceRecorder.StopRecording();
            }
        }
    }

    public void ChangeRoom(RoomEnum newRoom)
    {
        CurrentRoom = newRoom;

        if (backgroundRenderer == null)
        {
            Debug.LogWarning("Background Renderer is not assigned in GameController.");
            return;
        }

        switch(newRoom)
        {
            case RoomEnum.Bedroom:
                backgroundRenderer.sprite = bedRoomBackground;
                break;
            case RoomEnum.Kitchen:
                backgroundRenderer.sprite = kitchenBackground;
                break;
            case RoomEnum.Yard:
                backgroundRenderer.sprite = yardBackground;
                break;
            case RoomEnum.Bathroom:
                backgroundRenderer.sprite = bathroomBackground;
                break;
            default:
                Debug.LogWarning($"Unhandled room type: {newRoom}");
                break;
        }
    }
}
