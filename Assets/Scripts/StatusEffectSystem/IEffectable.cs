
public interface IEffectable
{
    public void ApplyEffect(CharacterStats casterStats, StatusEffectData effect);
    public void RemoveEffect(StatusEffectData effect);
    public void HandleEffect();
}
