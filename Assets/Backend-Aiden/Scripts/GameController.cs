using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class GameController : MonoBehaviour
{
    [Header("Room Management")]
    public RoomEnum CurrentRoom { get; private set; }
    [SerializeField] private RoomEnum startingRoom;

    [SerializeField] private GameObject bedroomBackground;
    [SerializeField] private GameObject kitchenBackground;
    [SerializeField] private GameObject yardBackground;
    [SerializeField] private GameObject bathroomBackground;

    private GameObject currentBackground;

    [Header("Inventory Management")]
    public Inventory inventory { get => _inventory; }
    [SerializeField] private Inventory _inventory;

    void Awake()
    {
        EnhancedTouchSupport.Enable();
        ChangeRoom(startingRoom);
    }

    public void ChangeRoom(RoomEnum newRoom)
    {
        CurrentRoom = newRoom;

        currentBackground?.SetActive(false);
        switch (newRoom)
        {
            case RoomEnum.Bedroom:
                currentBackground = bedroomBackground;
                break;
            case RoomEnum.Kitchen:
                currentBackground = kitchenBackground;
                break;
            case RoomEnum.Yard:
                currentBackground = yardBackground;
                break;
            case RoomEnum.Bathroom:
                currentBackground = bathroomBackground;
                break;
            default:
                Debug.LogWarning($"Unhandled room type: {newRoom}");
                break;
        }
        currentBackground?.SetActive(true);
    }
}
