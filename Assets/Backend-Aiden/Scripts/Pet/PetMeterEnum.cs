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

    public PetMeterAdjust(PetMeter meter, float amount)
    {
        this.meter = meter;
        this.amount = amount;
    }
}