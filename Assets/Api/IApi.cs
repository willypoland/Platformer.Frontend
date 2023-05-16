namespace Api
{
    public interface IApi // TODO Should rename
    {
        string DllName { get; }

        void Init(GameContext context);

        void SetLocation(Location location);

        Endpoint GetPublicEndpoint(int localPort);

        void StartGame();

        void StopGame();

        void Update(InputMap inputMap);

        void GetState(byte[] buffer, out int length);

        long GetMicrosecondsInOneTick();

        GameStatus GetStatus();

        PlatformerErrorCode GetErrorCode();
    }
}