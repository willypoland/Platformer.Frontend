using Game.Code.UI.Interfaces;
using UnityEngine;


namespace Game.Code.UI
{
    public class ConnectionStatus : MonoBehaviour, IStatusView
    {
        [SerializeField] private SimpleColoredStatus _background;
        [SerializeField] private SimpleColoredStatus _foreground;

        

        public void Inactive()
        {
            _background.Inactive();
            _foreground.Inactive();
        }

        public void Warning()
        {
            _background.Warning();
            _foreground.Warning();
        }

        public void Error()
        {
            _background.Error();
            _foreground.Error();
        }

        public void Ok()
        {
            _background.Ok();
            _foreground.Ok();
        }
    }
}