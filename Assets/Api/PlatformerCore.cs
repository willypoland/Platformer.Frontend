using System;
using Google.Protobuf;
using UnityEngine;


namespace Api
{
    public class PlatformerCore
    {
        public const int MaxBufferSize = 512;
        
        private readonly byte[] _buffer = new byte[MaxBufferSize];
        private readonly IApi _api;
        private Ser.GameState _gs;

        public Ser.GameState GameState => _gs;

        public static PlatformerCore CreateSync() => new PlatformerCore(new ApiSync());
        public static PlatformerCore CreateAsync() => new PlatformerCore(new ApiAsync());
        public static PlatformerCore CreateGGPO() => new PlatformerCore(new ApiGGPO());
        
        private PlatformerCore(IApi api)
        {
            _api = api;
        }

        public void StartGame(int localPort, bool isLocal, string ip, int remotePort)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            _api.RegisterPeer(localPort, isLocal, ip, remotePort);
            _api.StartGame();
            Debug.Log("Start game");
            
        }

        public void StopGame()
        {
            Debug.Log("Stop game");
            _api.StopGame();
        }

        public GameStatus UpdateTick(InputMap inputMap)
        {
            var status = _api.GetStatus(); 
            if (status == GameStatus.RUN)
            {
                _api.Update(inputMap);
                int length = _api.GetState(_buffer);
                _gs = Ser.GameState.Parser.ParseFrom(_buffer, 0, length);
            }

            return status;
        }
    }
}