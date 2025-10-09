using UnityEngine;

public class PetController : MonoBehaviour
{
    [SerializeField] private float hungerTimerBase = 60f; // Base time in seconds before hunger increases
    [SerializeField] private float energyTimerBase = 120f; // Base time in seconds before dnergy decreases

    private float hungerTimer;
    private float energyTimer;

    [SerializeField] private float maxHunger = 100f;
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float maxAffection = 100f;

    [SerializeField] private FloatData currentHunger;
    [SerializeField] private FloatData currentEnergy;
    [SerializeField] private FloatData currentAffection;
    [SerializeField] private FloatData currentStink;

    private bool isStinky = false;

    [SerializeField] private PetMeterAdjustEvent meterEvent;

    private void OnEnable()
    {
        meterEvent.OnEventRaised += HandleMeterAdjust;
    }

    private void OnDisable()
    {
        meterEvent.OnEventRaised -= HandleMeterAdjust;
    }
    void Start()
    {
        hungerTimer = Random.Range(hungerTimerBase * 0.8f, hungerTimerBase * 1.2f);
        energyTimer = Random.Range(energyTimerBase * 0.8f, energyTimerBase * 1.2f);
    }

    void Update()
    {
        hungerTimer -= Time.deltaTime;
        if (hungerTimer <= 0)
        {
            currentHunger.Value = Mathf.Max(currentHunger - 1, 0);
            hungerTimer = Random.Range(hungerTimerBase * 0.8f, hungerTimerBase * 1.2f);
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
        switch (adjust.meter)
        {
            case PetMeter.Hunger:
                Feed(adjust.amount);
                break;
            case PetMeter.Energy:
                currentEnergy.Value = Mathf.Clamp(currentEnergy + adjust.amount, 0, maxEnergy);
                break;
            case PetMeter.Affection:
                Play(adjust.amount, adjust.energyCost, adjust.stinkAmount);
                break;
            case PetMeter.Stink:
                Clean(adjust.amount);
                break;
        }
    }

    public void Feed(float amount)
    {
        currentHunger.Value = Mathf.Min(currentHunger.Value + amount, maxHunger);
    }

    public void Play(float affectionAmount, float energyCost, float stinkAmount)
    {
        if (currentEnergy >= energyCost)
        {
            currentAffection.Value = Mathf.Min(currentAffection + affectionAmount, maxAffection);
            currentEnergy.Value = Mathf.Max(currentEnergy - energyCost, 0);
            StinkUp(stinkAmount);
        }
    }

    private void StinkUp(float stinkAmount)
    {
        currentStink.Value += stinkAmount;
        if (currentStink.Value > 50)
        {
            isStinky = true;
        }
    }

    public void Clean(float energyRestored)
    {
        currentStink.Value = 0;
        isStinky = false;

        currentEnergy.Value = Mathf.Min(currentEnergy + energyRestored, maxEnergy);
    }
}
