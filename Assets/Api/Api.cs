using System.Runtime.InteropServices;

/*
 interface
 struct Input { bool leftPressed, rightPressed, upPressed, downPressed, leftMouseClicked; };
 EXPORT void RegisterPeer(int local_port, bool is_master, const char* remote_host, int remote_port);
 EXPORT void StartGame();
 EXPORT void StopGame();
 EXPORT void Update(const Input input);
 EXPORT int GetState(unsigned char* buf);
*/

namespace Api
{
    internal static unsafe class Api
    {
        //private const string DllName = "frontend_ggpo.dll";
        private const string DllName = "frontend_sync.dll";
        
        [DllImport(DllName, SetLastError = true)]
        private static extern void RegisterPeer(int localPort, bool isMaster, sbyte* remoteHost, int remotePort);

        [DllImport(DllName, SetLastError = true)]
        public static extern GameStatus GetStatus();

        [DllImport(DllName, SetLastError = true)]
        public static extern void StartGame();
        
        [DllImport(DllName, SetLastError = true)]
        public static extern void StopGame();
        
        [DllImport(DllName, SetLastError = true)]
        public static extern void Update(InputMap inputMap);
        
        [DllImport(DllName)]
        private static extern int GetState(byte* buf);

        public static void RegisterPeer(int localPort, bool isMaster, string remoteHost, int remotePort)
        { 
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(remoteHost);
            fixed (byte* cstr = bytes)
            {
                //RegisterPeer(localPort, isMaster, (sbyte*)cstr, remotePort);
            }
        }
        
        public static int GetState(byte[] buffer)
        {
            int length;
            fixed (byte* pBuffer = buffer)
            {
                length = GetState(pBuffer);
            }
            return length;
        }
    }


}
