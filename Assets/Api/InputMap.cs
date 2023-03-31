using System.Runtime.InteropServices;


namespace Api
{
    public struct InputMap
    {
        [MarshalAs(UnmanagedType.U1)] public bool LeftPressed;
        [MarshalAs(UnmanagedType.U1)] public bool RightPressed;
        [MarshalAs(UnmanagedType.U1)] public bool UpPressed;
        [MarshalAs(UnmanagedType.U1)] public bool DownPressed;
        [MarshalAs(UnmanagedType.U1)] public bool LeftMouseClicked;
    }
}