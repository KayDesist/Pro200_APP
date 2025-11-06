using System.Collections;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(SpriteRenderer))]
public class PetController : MonoBehaviour
{
    [Header("Meter Management")]
    [SerializeField] private float hungerTimerBase = 10f; // Base time in seconds before hunger increases
    [SerializeField] private float energyTimerBase = 15f; // Base time in seconds before dnergy decreases

    private float hungerTimer;
    private float energyTimer;

    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float maxAffection = 200f;
    [SerializeField] private float stinkThreshold = 50f;

    [SerializeField] private FloatData currentHunger;
    [SerializeField] private FloatData currentEnergy;
    [SerializeField] private FloatData currentAffection;
    [SerializeField] private FloatData currentStink;

    private bool isStinky = false;
    private PetEmotion currentEmotion = PetEmotion.Neutral;

    public float MaxHunger { get => maxHunger; }
    public float MaxEnergy { get => maxEnergy; }
    public float MaxAffection { get => maxAffection; }
    public float CurrentHunger { get => currentHunger; }
    public float CurrentEnergy { get => currentEnergy; }
    public float CurrentAffection { get => currentAffection; }

    public bool IsStinky { get => isStinky; }

    [SerializeField] private PetMeterAdjustEvent meterEvent;

    [Header("Emotion Management")]
    [SerializeField] private Sprite neutralSprite;
    [SerializeField] private Sprite stinkySprite;
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite sadSprite;
    [SerializeField] private Sprite angrySprite;
    [SerializeField] private Sprite confusedSprite;

    private SpriteRenderer spriteRenderer;

    [SerializeField] private int pokesToAngry = 5;
    private int pokeCount = 0;

    private void OnEnable()
    {
        meterEvent.Subscribe(HandleMeterAdjust);
    }

    private void OnDisable()
    {
        meterEvent.Unsubscribe(HandleMeterAdjust);
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        hungerTimer = Random.Range(hungerTimerBase * 0.8f, hungerTimerBase * 1.2f);
        energyTimer = Random.Range(energyTimerBase * 0.8f, energyTimerBase * 1.2f);
    }

    void Update()
    {
        if (TouchOverPet())
        {
            pokeCount++;
            if (pokeCount >= pokesToAngry)
            {
                SetEmotion(PetEmotion.Angry);
                pokeCount = 0;
            }
        }

        hungerTimer -= Time.deltaTime;
        if (hungerTimer <= 0)
        {
            currentHunger.Value = Mathf.Max(currentHunger - Random.Range(3, 8), 0);
            hungerTimer = Random.Range(hungerTimerBase * 0.8f, hungerTimerBase * 1.2f);
            if (currentHunger / maxHunger < 0.2f)
            {
                SetEmotion(PetEmotion.Sad, true);
            }
        }

        energyTimer -= Time.deltaTime;
        if (energyTimer <= 0)
        {
            currentEnergy.Value = Mathf.Max(currentEnergy - Random.Range(1, 2), 0);
            energyTimer = Random.Range(energyTimerBase * 0.8f, energyTimerBase * 1.2f);
        }
    }

    private bool TouchOverPet()
    {
        if (Touch.activeTouches.Count > 0)
        {
            var touch = Touch.activeTouches[0];
            if (touch.phase != UnityEngine.InputSystem.TouchPhase.Began) return false;
            Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.screenPosition);
            Collider2D collider = GetComponent<Collider2D>();
            if (collider.OverlapPoint(touchPosition))
            {
                return true;
            }
        }
        return false;
    }

    private void HandleMeterAdjust(PetMeterAdjust adjust)
    {
        switch (adjust.meter)
        {
            case PetMeter.Hunger:
                currentHunger.Value = Mathf.Clamp(currentHunger + adjust.amount, 0, maxHunger);
                break;
            case PetMeter.Energy:
                currentEnergy.Value = Mathf.Clamp(currentEnergy + adjust.amount, 0, maxEnergy);
                break;
            case PetMeter.Affection:
                currentAffection.Value = Mathf.Clamp(currentAffection + adjust.amount, 0, maxAffection);
                if (adjust.amount > 0)
                {
                    SetEmotion(PetEmotion.Happy);
                }
                else
                {
                    SetEmotion(PetEmotion.Angry);
                }
                break;
            case PetMeter.Stink:
                if (adjust.amount > 0)
                {
                    StinkUp(adjust.amount);
                }
                else
                {
                    Clean(-adjust.amount);
                }
                break;
        }
        SetSprite();
    }

    private void StinkUp(float stinkAmount)
    {
        currentStink.Value += stinkAmount;
        if (currentStink > stinkThreshold)
        {
            isStinky = true;
            SetSprite();
        }
    }

    private void Clean(float stinkReduction)
    {
        currentStink.Value = Mathf.Max(currentStink - stinkReduction, 0);
        if (currentStink <= stinkThreshold)
        {
            isStinky = false;
            SetSprite();
        }
    }

    public void SetEmotion(PetEmotion newEmotion, bool persistent = false)
    {
        currentEmotion = newEmotion;
        switch (currentEmotion)
        {
            case PetEmotion.Happy:
                currentAffection.Value = Mathf.Min(currentAffection + 5f, maxAffection);
                break;
            case PetEmotion.Sad:
                currentAffection.Value = Mathf.Max(currentAffection - 5f, 0);
                break;
            case PetEmotion.Angry:
                currentAffection.Value = Mathf.Max(currentAffection - 10f, 0);
                break;
        }

        SetSprite();

        if (!persistent) StartCoroutine(ResetEmotionCoroutine());
    }

    private void SetSprite()
    {
        if (isStinky)
        {
            spriteRenderer.sprite = stinkySprite;
            return;
        }

        switch (currentEmotion)
        {
            case PetEmotion.Neutral:
                spriteRenderer.sprite = neutralSprite;
                break;
            case PetEmotion.Happy:
                spriteRenderer.sprite = happySprite;
                break;
            case PetEmotion.Sad:
                spriteRenderer.sprite = sadSprite;
                break;
            case PetEmotion.Angry:
                spriteRenderer.sprite = angrySprite;
                break;
            case PetEmotion.Confused:
                spriteRenderer.sprite = confusedSprite;
                break;
        }
    }

    private IEnumerator ResetEmotionCoroutine()
    {
        yield return new WaitForSeconds(3f);
        currentEmotion = PetEmotion.Neutral;
        SetSprite();
    }
}

public enum PetEmotion
{
    Neutral,
    Happy,
    Sad,
    Angry,
    Confused
}