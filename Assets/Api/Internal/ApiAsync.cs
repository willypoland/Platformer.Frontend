using System.Net;
using System.Runtime.InteropServices;


namespace Api.Internal
{
    internal unsafe class ApiAsync : IApi
    {
        private const string DllName = "frontend_async.dll";

        string IApi.DllName => DllName;

        void IApi.Init(Location location)
        {
            LocationUnsafe raw; // TODO How name this variable?
            raw.Is1stPlayer = location.IsFirstPlayer; 
            raw.Position1stPlayer = location.PositionFirstPlayer;
            raw.Position2ndPlaer = location.PositionSecondPlayer;
            raw.PlatformsCount = (ulong) location.Platfroms.Length;

            fixed (Platform* platfroms = location.Platfroms)
            {
                raw.Platforms = platfroms;
                Init(raw);
            }
        }

        Endpoint IApi.GetPublicEndpoint(int localPort)
        {
            //EndpointUnsafe raw = GetPublicEndpoint(localPort);
            Endpoint endpoint;
            endpoint.RemoteHost = IPAddress.Loopback;
            endpoint.RemotePort = localPort;
            return endpoint;
        }

        void IApi.RegisterPeer(Endpoint peerEndpoint)
        {
        }

        void IApi.StartGame() => StartGame();

        void IApi.StopGame() => StopGame();

        void IApi.Update(InputMap inputMap) => Update(inputMap);

        int IApi.GetState(byte[] buffer)
        {
            int size;
            fixed (byte* pBuffer = buffer)
                size = GetState(pBuffer);

            return size;
        }

        GameStatus IApi.GetStatus() => GetStatus();

        PlatformerErrorCode IApi.GetErrorCode() => GetErrorCode();


        #region Extern import

        [DllImport(DllName, SetLastError = true)]
        private static extern void Init(LocationUnsafe location);

        // [DllImport(DllName, SetLastError = true)]
        // private static extern EndpointUnsafe GetPublicEndpoint(int local_port);

        // [DllImport(DllName, SetLastError = true)]
        // private static extern void RegisterPeer(EndpointUnsafe peer_endpoint);

        [DllImport(DllName, SetLastError = true)]
        private static extern void StartGame();

        [DllImport(DllName, SetLastError = true)]
        private static extern void StopGame();

        [DllImport(DllName, SetLastError = true)]
        private static extern void Update(InputMap input);

        [DllImport(DllName, SetLastError = true)]
        private static extern int GetState(byte* buf);

        [DllImport(DllName, SetLastError = true)]
        private static extern GameStatus GetStatus();

        [DllImport(DllName, SetLastError = true)]
        private static extern PlatformerErrorCode GetErrorCode();

        #endregion
    }
}