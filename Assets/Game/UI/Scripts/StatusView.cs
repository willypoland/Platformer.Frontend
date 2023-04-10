using System;
using UnityEngine;

public class StatusView : MonoBehaviour
{
    public enum Status
    {
        Disconnected,
        Sync,
        Run,
        Stopped,
    }

    [SerializeField] private GameObject _run;
    [SerializeField] private GameObject _stopped;
    [SerializeField] private GameObject _sync;
    [SerializeField] private GameObject _disconnected;
    [SerializeField] private Status _status;

    private void Start()
    {
        _run.SetActive(false);
        _stopped.SetActive(false);
        _sync.SetActive(false);
        _disconnected.SetActive(false);
        GetGameObjectStatus(_status).SetActive(true);
    }

    public void SetStatus(Status value)
    {
        if (_status == value)
            return;

        GetGameObjectStatus(_status).SetActive(false);
        GetGameObjectStatus(value).SetActive(true);
        _status = value;
    }

    private GameObject GetGameObjectStatus(Status status)
    {
        return status switch
        {
            Status.Run => _run,
            Status.Sync => _sync,
            Status.Stopped => _stopped,
            Status.Disconnected => _disconnected,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}