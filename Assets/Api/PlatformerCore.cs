using System;
using Google.Protobuf;
using UnityEngine;


namespace Api
{
    public class PlatformerCore
    {
        public const int MaxBufferSize = 512;
        
        private readonly byte[] _buffer = new byte[MaxBufferSize];
        private Ser.GameState _gs;

        public Ser.GameState GameState => _gs;
        
        public void StartGame(int localPort, bool isLocal, string ip, int remotePort)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            Api.RegisterPeer(localPort, isLocal, ip, remotePort);
            Api.StartGame();
            Debug.Log("Start game");
            
        }

        public void StopGame()
        {
            Debug.Log("Stop game");
            Api.StopGame();
        }

        public void UpdateTick(InputMap inputMap)
        {
            if (Api.GetStatus() == GameStatus.RUN)
            {
                Api.Update(inputMap);
                int length = Api.GetState(_buffer);
                _gs = Ser.GameState.Parser.ParseFrom(_buffer, 0, length);
            }
        }
    }
}