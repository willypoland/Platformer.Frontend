using System;
using System.Net;
using System.Runtime.InteropServices;


namespace Api.Internal
{
    internal unsafe class ApiAsync : IApi
    {
        private const string DllName = "frontend_async.dll";

        string IApi.DllName => DllName;

        void IApi.Init(GameContext context) => Init(context);

        void IApi.SetLocation(Location location) => SetLocation(location);

        Endpoint IApi.GetPublicEndpoint(int localPort) => new(IPAddress.Loopback, localPort);

        void IApi.GetState(byte[] buffer, out int length)
        {
            fixed (byte* p = buffer)
                GetState(p, out length);
        }

        void IApi.StartGame() => StartGame();

        void IApi.StopGame() => StopGame();

        void IApi.Update(InputMap inputMap) => Update(inputMap);

        long IApi.GetMicrosecondsInOneTick() => getMicrosecondsInOneTick();

        GameStatus IApi.GetStatus() => GetStatus();

        PlatformerErrorCode IApi.GetErrorCode() => GetErrorCode();


        #region Extern

        [DllImport(DllName, SetLastError = true)]
        private static extern void Init(GameContext context);
        
        [DllImport(DllName, SetLastError = true)]
        private static extern void SetLocation(Location location);

        // [DllImport(DllName, SetLastError = true)]
        // private static extern EndpointUnsafe GetPublicEndpoint(int local_port);

        [DllImport(DllName, SetLastError = true)]
        private static extern void StartGame();

        [DllImport(DllName, SetLastError = true)]
        private static extern void StopGame();

        [DllImport(DllName, SetLastError = true)]
        private static extern void Update(InputMap input);

        [DllImport(DllName, SetLastError = true)]
        private static extern void GetState(byte* buf, out int length);
        
        [DllImport(DllName, SetLastError = true)]
        private static extern long getMicrosecondsInOneTick();

        [DllImport(DllName, SetLastError = true)]
        private static extern GameStatus GetStatus();

        [DllImport(DllName, SetLastError = true)]
        private static extern PlatformerErrorCode GetErrorCode();

        #endregion
    }
}