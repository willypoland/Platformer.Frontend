using System.Net;


namespace Api
{
    public interface IApi
    {
        string DllName { get; }

        GameStatus GetStatus();

        void StartGame();

        void StopGame();

        void Update(InputMap inputMap);

        int GetState(byte[] buffer);

        void RegisterPeer(int localPort, bool isMaster, string remoteHost, int remotePort);
    }
}