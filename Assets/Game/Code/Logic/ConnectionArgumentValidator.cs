using Game.Code.Data;


namespace Game.Code.Logic
{

    public class ConnectionArgumentValidator : IConnectionArgumentValidator
    {
        public bool TryParseAddress(string input, out string ip, out ushort port)
        {
            throw new System.NotImplementedException();
        }

        public bool TryParsePort(string input, out ushort port)
        {
            return ushort.TryParse(input, out port);
        }

        public ConnectionArguments TryParseFromStartArgument(string[] args, out ushort port)
        {
            throw new System.NotImplementedException();
        }
    }
}