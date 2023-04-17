using System.Collections;
using System.Linq;
using UnityEngine;


namespace Game.Code.UI
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private ProgressBar _loadingProgressBar;
        [SerializeField, Tooltip("in second")] private float _fadeSpeed = 0.333333f;
        
        private float _currentProgressValue;
        private float _endProgressValue;

        private void Awake()
        {
            SetActvie(_canvasGroup, false);
            DontDestroyOnLoad(this);
        }

        public void SetProgress(float progress)
        {
            _endProgressValue += progress;
        }

        public void Show()
        {
            SetActvie(_canvasGroup, true);
            _currentProgressValue = 0f;
            _endProgressValue = 1f;
            
            StartCoroutine(UpdateLoadingBarRoutine());
        }

        public void Hide()
        {
            StartCoroutine(DoFadeInRoutine());
        }

        private IEnumerator UpdateLoadingBarRoutine()
        {
            float currentTime = 0f;

            while (currentTime < _endProgressValue)
            {
                UpdateProgressBar(currentTime);
                currentTime += Time.deltaTime;
                yield return null;
            }
        }

        private void UpdateProgressBar(float currentTime)
        {
            float t = currentTime / _endProgressValue;
            t = Mathf.Sin(t * Mathf.PI * 0.5f);
            var fillAmount = Mathf.Lerp(_currentProgressValue, _endProgressValue, t);
            _loadingProgressBar.SetProgress(fillAmount);
        }

        private IEnumerator DoFadeInRoutine()
        {
            while (_canvasGroup.alpha > 0)
            {
                _canvasGroup.alpha -= Time.unscaledDeltaTime * _fadeSpeed;
                yield return null;
            }
            
            SetActvie(_canvasGroup, false);
        }

        private static void SetActvie(CanvasGroup canvasGroup, bool isActive)
        {
            canvasGroup.alpha = isActive ? 1f : 0f;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }

        private static bool Interactable(CanvasGroup[] groups)
        {
            return groups != null && groups.Length > 0 && groups.All(x => x.interactable);
        }

    }
}