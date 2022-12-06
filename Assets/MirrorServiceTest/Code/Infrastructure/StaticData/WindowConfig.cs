using System;
using MirrorServiceTest.Code.Infrastructure.Data;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.StaticData
{
    [Serializable]
    public class WindowConfig
    {
        public WindowID ID;
        public MonoBehaviour WindowPrefab;
    }
}