// using System;
// using System.Net;
// using System.Runtime.InteropServices;
//
//
// namespace Api.Internal
// {
//     internal unsafe class ApiGGPO : IApi
//     {
//         private const string DllName = "frontend_ggpo.dll";
//
//         string IApi.DllName => DllName;
//
//         public void Init(GameContext context)
//         {
//             throw new NotImplementedException();
//         }
//
//         public void SetLocation(Location location)
//         {
//             throw new NotImplementedException();
//         }
//
//         void IApi.Init(Location location)
//         {
//             LocationUnsafe lu;
//             lu.Is1stPlayer = location.IsFirstPlayer;
//             lu.Position1stPlayer = location.PositionFirstPlayer;
//             lu.Position2ndPlaer = location.PositionSecondPlayer;
//             lu.PlatformsCount = (ulong) location.Platfroms.Length;
//
//             fixed (Platform* platfroms = location.Platfroms)
//             {
//                 lu.Platforms = platfroms;
//                 Init(lu);
//             }
//         }
//
//         Endpoint IApi.GetPublicEndpoint(int localPort)
//         {
//             EndpointUnsafe raw = GetPublicEndpoint(localPort);
//             Endpoint endpoint;
//             endpoint.RemoteHost = new IPAddress(new ReadOnlySpan<byte>(raw.RemoteHost, 4));
//             endpoint.RemotePort = raw.RemotePort;
//             return endpoint;
//         }
//
//         void IApi.RegisterPeer(Endpoint peerEndpoint)
//         {
//             EndpointUnsafe raw;
//             raw.RemotePort = peerEndpoint.RemotePort;
//             byte[] address = peerEndpoint.RemoteHost.GetAddressBytes();
//
//             fixed (byte* pAddress = address)
//             {
//                 raw.RemoteHost = pAddress;
//                 RegisterPeer(raw);
//             }
//         }
//
//         void IApi.StartGame() => StartGame();
//
//         void IApi.StopGame() => StopGame();
//
//         void IApi.Update(InputMap inputMap) => Update(inputMap);
//
//         public void GetState(byte[] buffer, out int length)
//         {
//             throw new NotImplementedException();
//         }
//
//         public long GetMicrosecondsInOneTick()
//         {
//             throw new NotImplementedException();
//         }
//
//         void IApi.GetState(byte[] buffer, out int length, out float dx)
//         {
//             fixed (byte* pBuffer = buffer)
//                 GetState(pBuffer, out length, out dx);
//         }
//
//         GameStatus IApi.GetStatus() => GetStatus();
//
//         PlatformerErrorCode IApi.GetErrorCode() => GetErrorCode();
//
//
//         #region Extern import
//
//         [DllImport(DllName, SetLastError = true)]
//         private static extern void Init(LocationUnsafe location);
//
//         [DllImport(DllName, SetLastError = true)]
//         private static extern EndpointUnsafe GetPublicEndpoint(int local_port);
//
//         [DllImport(DllName, SetLastError = true)]
//         private static extern void RegisterPeer(EndpointUnsafe peer_endpoint);
//
//         [DllImport(DllName, SetLastError = true)]
//         private static extern void StartGame();
//
//         [DllImport(DllName, SetLastError = true)]
//         private static extern void StopGame();
//
//         [DllImport(DllName, SetLastError = true)]
//         private static extern void Update(InputMap input);
//
//         [DllImport(DllName, SetLastError = true)]
//         private static extern void GetState(byte* buf, out int length, out float dx);
//
//         [DllImport(DllName, SetLastError = true)]
//         private static extern GameStatus GetStatus();
//
//         [DllImport(DllName, SetLastError = true)]
//         private static extern PlatformerErrorCode GetErrorCode();
//
//         #endregion
//
//     }
//
//
// }