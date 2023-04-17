using UnityEngine;
using UnityEngine.UI;


namespace Game.Code.UI
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _foreground;

        public void SetProgress(float fillAmount)
        {
            _foreground.fillAmount = fillAmount;
        }
    }
}