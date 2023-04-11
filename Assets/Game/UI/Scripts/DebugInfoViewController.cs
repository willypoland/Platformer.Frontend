using UnityEngine;


public class DebugInfoViewController : MonoBehaviour
{
    [SerializeField] private KeyCode _showPlayerState = KeyCode.Tilde;
    [SerializeField] private PlayerStateView[] _playerStateViews;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(_showPlayerState))
        {
            foreach (var item in _playerStateViews)
                item.gameObject.SetActive(!item.gameObject.activeSelf);
        }
    }
}