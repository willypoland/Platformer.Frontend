using System.Collections;
using UnityEngine;


namespace Game.Code.Infrastructure.Services
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);

        void StopAllCoroutines();
    }
}