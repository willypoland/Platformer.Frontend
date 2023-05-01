using System.Runtime.InteropServices;


namespace Api
{
    public struct InputMap
    {
        [MarshalAs(UnmanagedType.I1)] public bool LeftPressed;
        [MarshalAs(UnmanagedType.I1)] public bool RightPressed;
        [MarshalAs(UnmanagedType.I1)] public bool UpPressed;
        [MarshalAs(UnmanagedType.I1)] public bool DownPressed;
        [MarshalAs(UnmanagedType.I1)] public bool LeftMouseClicked;
        [MarshalAs(UnmanagedType.I1)] public bool RightMouseClicked;
    }
}