
public interface IEffectable
{
    public void ApplyEffect(StatusEffectData effect);
    public void RemoveEffect(StatusEffectData effect);
    public void HandleEffect();
}
