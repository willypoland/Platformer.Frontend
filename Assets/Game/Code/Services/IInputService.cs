using Api;


namespace Game.Code.Logic
{
    public interface IInputService
    {
        InputMap ReadInputMap();

        bool Exit { get; }
        
        float Wheel { get; }
    }
}