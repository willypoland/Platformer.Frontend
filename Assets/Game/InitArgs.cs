using System.Net;


namespace Game
{
    public struct InitArgs
    {
        public bool IsLocal;
        public ushort LocalPort;
        public string Ip;
        public ushort RemotePort;
    }
}