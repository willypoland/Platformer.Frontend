namespace Api
{
    public interface IPlayer
    {
        IGameObject Object { get; }
        PlayerState State { get; }
        int StateFrame { get; }
        InputMap PrevNput { get; }
        bool OnGround { get; }
        bool OnDamage { get; }
        bool LeftDireciton { get; }
    }
}