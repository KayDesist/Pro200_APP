using System.Collections;
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
    [Header("Item Information")]
    [SerializeField] private int itemID;
    [SerializeField] private float cost;
    public float Cost { get => cost; }

    [Header("Item Usage")]
    [SerializeField] private List<RoomEnum> usableInRooms = new List<RoomEnum>();

    [SerializeField] private PetMeter[] affectedMeters;
    [SerializeField] private float[] meterAdjustmentValues;
    [SerializeField] private PetMeterAdjustEvent petMeterEvent;

    private bool isOverPet = false;
    private bool isDead = false;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (!isDead && Touch.activeTouches.Count > 0)
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
        if (isOverPet && usableInRooms.Contains(FindFirstObjectByType<GameController>().CurrentRoom))
        {
            for (int i = 0; i < affectedMeters.Length; i++)
            {
                if (i >= meterAdjustmentValues.Length)
                {
                    Debug.LogWarning($"ItemBase on {gameObject.name} has mismatched affectedMeters and meterAdjustmentValues lengths.");
                    break;
                }

                PetMeterAdjust petMeterAdjust = new PetMeterAdjust
                {
                    meter = affectedMeters[i],
                    amount = meterAdjustmentValues[i]
                };
                petMeterEvent.RaiseEvent(petMeterAdjust);
            }
            Destroy(gameObject);
        }
        else
        {
            isDead = true;
            FindFirstObjectByType<GameController>().inventory.AddItem(itemID, 1);

            StartCoroutine(ResetPosition(0.1f));
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

    private IEnumerator ResetPosition(float distancePerLoop)
    {
        yield return new WaitForSeconds(0.001f);
        Vector3 direction = (startPosition - transform.position).normalized;
        transform.position += direction * distancePerLoop;

        if (Vector3.Distance(transform.position, startPosition) > distancePerLoop)
        {
            StartCoroutine(ResetPosition(distancePerLoop));
        }
        else
        {
            transform.position = startPosition;
            Destroy(gameObject);
        }
    }
}
