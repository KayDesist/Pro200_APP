using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

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

    void Update()
    {
        
    }

    public void ChangeRoom(RoomEnum newRoom)
    {
        CurrentRoom = newRoom;
    }
}
