using System;
using Google.Protobuf;


namespace Api
{
    public class PlatformerCore
    {
        public const int MaxBufferSize = 512;
        
        private readonly byte[] _buffer = new byte[MaxBufferSize];
        private readonly Ser.GameState _gs = new();

        public Ser.GameState GameState => _gs;
        
        public void StartGame(int localPort, bool isLocal, string ip, int remotePort)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            Api.RegisterPeer(localPort, isLocal, ip, remotePort);
            Api.StartGame();
        }

        public void StopGame()
        {
            Api.StopGame();
        }

        public void UpdateTick(InputMap inputMap)
        {
            Api.Update(inputMap);
            int length = Api.GetState(_buffer);
            _gs.MergeFrom(_buffer, 0, length);
        }
    }
}