using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PetController : MonoBehaviour
{
    [Header("Meter Management")]
    [SerializeField] private float hungerTimerBase = 60f; // Base time in seconds before hunger increases
    [SerializeField] private float energyTimerBase = 120f; // Base time in seconds before dnergy decreases

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
    [SerializeField] private Sprite happySprite;
    [SerializeField] private Sprite sadSprite;
    [SerializeField] private Sprite angrySprite;
    [SerializeField] private Sprite confusedSprite;

    private SpriteRenderer spriteRenderer;

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
        hungerTimer -= Time.deltaTime;
        if (hungerTimer <= 0)
        {
            currentHunger.Value = Mathf.Max(currentHunger - 5, 0);
            hungerTimer = Random.Range(hungerTimerBase * 0.8f, hungerTimerBase * 1.2f);
            if (currentHunger / maxHunger < 0.2f)
            {
                SetEmotion(PetEmotion.Sad, true);
            }
        }

        energyTimer -= Time.deltaTime;
        if (energyTimer <= 0)
        {
            currentEnergy.Value = Mathf.Max(currentEnergy - 1, 0);
            energyTimer = Random.Range(energyTimerBase * 0.8f, energyTimerBase * 1.2f);
        }
    }

    private void HandleMeterAdjust(PetMeterAdjust adjust)
    {
        Debug.Log($"Adjusting {adjust.meter} by {adjust.amount}");
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
    }

    private void StinkUp(float stinkAmount)
    {
        currentStink.Value += stinkAmount;
        if (currentStink > stinkThreshold)
        {
            isStinky = true;
        }
    }

    private void Clean(float stinkReduction)
    {
        currentStink.Value = Mathf.Max(currentStink - stinkReduction, 0);
        if (currentStink <= stinkThreshold)
        {
            isStinky = false;
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