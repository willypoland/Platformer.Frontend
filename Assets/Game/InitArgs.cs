using System.Net;


namespace Game
{
    public struct StartArgs
    {
        public bool IsLocal;
        public ushort LocalPort;
        public IPAddress Ip;
        public ushort RemotePort;
    }
}