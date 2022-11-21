using System;
using BH_Test_Project.Code.Infrastructure.Data;
using BH_Test_Project.Code.UI;

namespace BH_Test_Project.Code.Infrastructure.StaticData
{
    [Serializable]
    public class WindowConfig
    {
        public WindowID ID;
        public WindowBase Prefab;
    }
}