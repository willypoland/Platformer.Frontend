using Game.Code.UI.Interfaces;
using UnityEngine;
using UnityEngine.UI;


namespace Game.Code.UI
{
    public class SimpleColoredStatus : MonoBehaviour, IStatusView
    {
        [SerializeField] private Image _target;
        [SerializeField] private Color _inactiveColor = Color.gray;
        [SerializeField] private Color _warningColor = Color.yellow;
        [SerializeField] private Color _okColor = Color.green;
        [SerializeField] private Color _errorColor = Color.red;

        private void SetColor(Color color) => _target.color = color;

        public void Inactive() => SetColor(_inactiveColor);

        public void Warning() => SetColor(_warningColor);

        public void Error() => SetColor(_errorColor);

        public void Ok() => SetColor(_okColor);
    }
}