public enum PetMeter
{
    Hunger,
    Energy,
    Affection,
    Stink
}

public struct PetMeterAdjust
{
    public PetMeter meter;
    public float amount;
    public float energyCost;
    public float stinkAmount;
    public PetMeterAdjust(PetMeter meter, float amount, float energyCost = 0f, float stinkAmount = 0f)
    {
        this.meter = meter;
        this.amount = amount;
        this.energyCost = energyCost;
        this.stinkAmount = stinkAmount;
    }
}