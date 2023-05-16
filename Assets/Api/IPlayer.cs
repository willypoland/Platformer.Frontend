namespace Api
{
    public interface IPlayer
    {
        IGameObject Object { get; }
        PlayerState State { get; }
        AttackPhase AttackPhase { get; }
        int StateFrame { get; }
        bool OnGround { get; }
        bool OnDamage { get; }
        bool LeftDireciton { get; }
        int CurrentHealth { get; }
        int MaxHealth { get; }
    }
}