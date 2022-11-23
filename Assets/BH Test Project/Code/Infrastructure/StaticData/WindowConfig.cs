using System;
using BH_Test_Project.Code.Infrastructure.Data;
using UnityEngine;

namespace BH_Test_Project.Code.Infrastructure.StaticData
{
    [Serializable]
    public class WindowConfig
    {
        public WindowID ID;
        public MonoBehaviour WindowPrefab;
    }
}