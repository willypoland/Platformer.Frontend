using System;


namespace Game.Code.UI
{
    public interface IConnectionSetupWindow
    {
        event Func<InputConnectionArguments, InputConnectionArguments> InputChanged;
        event Action ClickConnection;

        void SetArguments(InputConnectionArguments arguments);
    }
}