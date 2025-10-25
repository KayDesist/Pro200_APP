using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class GameController : MonoBehaviour
{
    [Header("Room Management")]
    public RoomEnum CurrentRoom { get; private set; }
    [SerializeField] private RoomEnum startingRoom;

    [Header("Inventory Management")]
    public Inventory inventory { get => _inventory; }
    [SerializeField] private Inventory _inventory;

    void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    void Start()
    {
        CurrentRoom = startingRoom;
    }

    public void ChangeRoom(RoomEnum newRoom)
    {
        CurrentRoom = newRoom;
    }
}
