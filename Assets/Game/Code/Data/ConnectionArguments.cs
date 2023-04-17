using System;
using System.Net;


namespace Game.Code.Data
{
    [System.Serializable]
    public class ConnectionArguments
    {
        private bool _isMaster = true;
        private ushort _localPort = 7000;
        private string _remoteIp = "127.0.0.1";
        private ushort _remotePort = 7001;

        public event Action<bool> IsMasterChanged;
        public event Action<ushort> LocalPortChanged;
        public event Action<string> RemoteIpChanged;
        public event Action<ushort> RemotePortChanged;
        public event Action AnyChanged;

        public static bool ValidateComandLineArgument(string[] args, out bool isMaster, out ushort localPort,
                                                      out string remoteIp, out ushort remotePort)
        {
            isMaster = false;
            localPort = 0;
            remoteIp = null;
            remotePort = 0;
            
            if (args.Length < 5)
                return false;
            
            if (args[2].Contains("local", StringComparison.InvariantCultureIgnoreCase))
                isMaster = true;
            else if (args[2].Contains("remot", StringComparison.InvariantCultureIgnoreCase))
                isMaster = false;
            else
                return false;

            if (!ValidatePort(args[3], out localPort))
                return false;

            if (!ValidateAddress(args[4], out remoteIp, out remotePort))
                return false;

            return true;
        }

        public static bool ValidatePort(string str, out ushort port)
        {
            return ushort.TryParse(str, out port);
        }

        public static bool ValidateAddress(string str, out string ip, out ushort port)
        {
            ip = null;
            port = 0;
            
            var arr = str.Split(':');
            if (arr == null || arr.Length < 2)
                return false;
            
            var ipStr = arr[0];
            var portStr = arr[1];
            
            if (!IPAddress.TryParse(ipStr, out var ipAddress))
                return false;

            if (!ValidatePort(portStr, out port))
                return false;

            ip = ipAddress.ToString();
            return true;
        }

        public bool IsMaster
        {
            get => _isMaster;
            set => UpdateAndCallEvent(ref _isMaster, IsMasterChanged, value);
        }

        public ushort LocalPort
        {
            get => _localPort;
            set => UpdateAndCallEvent(ref _localPort, LocalPortChanged, value);
        }

        public string RemoteIp
        {
            get => _remoteIp;
            set => UpdateAndCallEvent(ref _remoteIp, RemoteIpChanged, value);
        }

        public ushort RemotePort
        {
            get => _remotePort;
            set => UpdateAndCallEvent(ref _remotePort, RemotePortChanged, value);
        }

        private void UpdateAndCallEvent<T>(ref T source, Action<T> e, T value) where T : IEquatable<T>
        {
            if (!source.Equals(value))
            {
                source = value;
                e?.Invoke(value);
                AnyChanged?.Invoke();
            }
        }
    }
}