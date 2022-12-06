using System;
using UnityEngine;

namespace MirrorServiceTest.Code.StaticData
{
    [Serializable]
    public struct WorldStaticData
    {
        [Range(1, 10)] public int GameEndScore;
        [Range(1, 10)] public float GameRestartDelay;
    }
}