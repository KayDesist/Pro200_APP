using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(Collider2D))]
public class ItemBase : MonoBehaviour
{
    [SerializeField] private Dictionary<PetMeter, float> meterAdjustments = new Dictionary<PetMeter, float>();
    [SerializeField] private PetMeterAdjustEvent petMeterEvent;

    private bool isOverPet = false;

    void Start()
    {

    }

    void Update()
    {
        if (Touch.activeTouches.Count > 0)
        {
            var touch = Touch.activeTouches[0];
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Vector2 touchPosition = touch.screenPosition;
                transform.position = Camera.main.ScreenToWorldPoint(touchPosition);
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
            }
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                ReleaseFinger();
                return;
            }
        }
    }


    private void ReleaseFinger()
    {
        if (isOverPet)
        {
            foreach (var adjustment in meterAdjustments)
            {
                PetMeterAdjust petMeterAdjust = new PetMeterAdjust
                {
                    meter = adjustment.Key,
                    amount = adjustment.Value
                };
                petMeterEvent.RaiseEvent(petMeterAdjust);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Pet"))
        {
            PetController pet = collision.GetComponent<PetController>();
            if (pet != null)
            {
                isOverPet = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Pet"))
        {
            PetController pet = collision.GetComponent<PetController>();
            if (pet != null)
            {
                isOverPet = false;
            }
        }
    }
}
