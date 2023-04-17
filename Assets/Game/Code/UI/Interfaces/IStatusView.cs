namespace Game.Code.UI.Interfaces
{
    public interface IStatusView
    {
        void Inactive();
        void Warning();
        void Error();
        void Ok();
    }
}