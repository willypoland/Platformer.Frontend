using Game.Code.Data;


namespace Game.Code.Logic
{
    public interface IConnectionArgumentValidator
    {
        public bool TryParseAddress(string input, out string ip, out ushort port);
        public bool TryParsePort(string input, out ushort port);
        public ConnectionArguments TryParseFromStartArgument(string[] args, out ushort port);
    }
}