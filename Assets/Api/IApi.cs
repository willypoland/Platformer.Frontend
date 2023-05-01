using System.Net;


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

        int GetState(byte[] buffer);

        GameStatus GetStatus();

        PlatformerErrorCode GetErrorCode();
    }


}