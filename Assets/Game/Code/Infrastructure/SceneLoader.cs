using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Game.Code.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Game.Code.Infrastructure
{
    public class SceneLoader
    {
        private readonly LoadingCurtain _loadingCurtain;

        public SceneLoader(LoadingCurtain loadingCurtain)
        {
            _loadingCurtain = loadingCurtain;
        }

        public async UniTask Load(string sceneName, Action onLoadedCallback = null)
        {
            _loadingCurtain.Show();
            await LoadScene(sceneName, onLoadedCallback);
            await UniTask.Delay(2000);
            _loadingCurtain.Hide();
        }

        private async Task LoadScene(string nextScene, Action onLoadedCallback)
        {
            if (SceneManager.GetActiveScene().name == nextScene)
            {
                onLoadedCallback?.Invoke();
                await UniTask.Yield();
            }

            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
            {
                _loadingCurtain.SetProgress(waitNextScene.progress);
                await UniTask.Yield();
            }
            
            onLoadedCallback?.Invoke();
        }
    }


}