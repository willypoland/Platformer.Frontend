namespace Api
{
    public readonly ref struct GameContext
    {
        private readonly int _tickRate;
        private readonly Endpoint _peerEndpoint;

        public GameContext(int tickRate, Endpoint peerEndpoint)
        {
            _tickRate = tickRate;
            _peerEndpoint = peerEndpoint;
        }

        public int TickRate => _tickRate;
        public Endpoint PeerEndpoint => _peerEndpoint;
    }
}