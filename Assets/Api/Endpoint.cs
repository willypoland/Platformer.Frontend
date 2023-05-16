using System;
using System.Net;


namespace Api
{
    public readonly unsafe ref struct Endpoint
    {
        private readonly byte* _remoteHost;
        private readonly int _remotePort;

        public Endpoint(IPAddress host, int port)
        {
            fixed (byte* p = host.GetAddressBytes())
                _remoteHost = p;

            _remotePort = port;
        }

        public IPAddress RemoteHost => new IPAddress(new ReadOnlySpan<byte>(_remoteHost, 4));

        public int RemotePort => _remotePort;
    }
}