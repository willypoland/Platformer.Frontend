namespace Api
{
    public interface IApi // TODO Should rename
    {
        string DllName { get; }

        void Init(Location location);

        Endpoint GetPublicEndpoint(int localPort);

        void RegisterPeer(Endpoint peerEndpoint);

        void StartGame();

        void StopGame();

        void Update(InputMap inputMap);

        void GetState(byte[] buffer, out int length, out float dx);

        GameStatus GetStatus();

        PlatformerErrorCode GetErrorCode();
    }


}