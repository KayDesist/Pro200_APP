using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameController : MonoBehaviour
{
    void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    void Update()
    {
        
    }
}
