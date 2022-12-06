using System.Collections;
using BH_Test_Project.Code.Infrastructure.DI;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.Services.CoroutineRunner
{
    public interface ICoroutineRunner : IService
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
