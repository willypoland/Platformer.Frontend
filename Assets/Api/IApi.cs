namespace Api
{
    internal interface IApi
    {
        string DllNAme { get; }
        GameStatus GetStatus();
        void StartGame();
        void StopGame();
        void Update(InputMap inputMap);
        int GetState(byte[] buffer);
        void RegisterPeer(int localPort, bool isMaster, string remoteHost, int remotePort);
    }
}