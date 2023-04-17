using System;


namespace Game.Code.UI.Interfaces
{
    public interface IConnectionSetupView
    {
        event Func<InputConnectionArguments, InputConnectionArguments> InputChanged;
        event Action ClickConnection;

        void SetArguments(InputConnectionArguments arguments);
    }
}