namespace Game.Code.Data
{
    [System.Serializable]
    public class ConnectionArguments
    {
        public bool IsMaster = true;
        public ushort LocalPort = 7000;
        public string RemoteIp = "127.0.0.1";
        public ushort RemotePort = 7001;
    }
}