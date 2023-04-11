using System.Runtime.InteropServices;


namespace Api
{
    internal unsafe class ApiSync : IApi
    {
        private const string DllName = "frontend_sync.dll";

        string IApi.DllNAme => DllName;

        void IApi.StartGame()
        {
            StartGame();
        }

        void IApi.StopGame()
        {
            StopGame();
        }

        GameStatus IApi.GetStatus()
        {
            return GetStatus();
        }

        void IApi.Update(InputMap inputMap)
        {
            Update(inputMap);
        }

        int IApi.GetState(byte[] buffer)
        {
            int length;
            fixed (byte* pBuffer = buffer)
            {
                length = GetState(pBuffer);
            }
            return length;
        }

        void IApi.RegisterPeer(int localPort, bool isMaster, string remoteHost, int remotePort)
        {
        }

        [DllImport(DllName, SetLastError = true)]
        private static extern GameStatus GetStatus();

        [DllImport(DllName, SetLastError = true)]
        private static extern void StartGame();
        
        [DllImport(DllName, SetLastError = true)]
        private static extern void StopGame();
        
        [DllImport(DllName, SetLastError = true)]
        private static extern void Update(InputMap inputMap);
        
        [DllImport(DllName)]
        private static extern int GetState(byte* buf);
    }
}