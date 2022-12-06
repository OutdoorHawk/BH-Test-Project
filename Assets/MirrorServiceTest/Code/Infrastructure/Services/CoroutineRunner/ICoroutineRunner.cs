using System.Collections;
using MirrorServiceTest.Code.Infrastructure.DI;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.CoroutineRunner
{
    public interface ICoroutineRunner : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
