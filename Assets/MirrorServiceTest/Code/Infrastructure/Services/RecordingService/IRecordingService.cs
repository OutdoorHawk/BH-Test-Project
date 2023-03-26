﻿
using MirrorServiceTest.Code.Infrastructure.DI;
using MirrorServiceTest.Code.Runtime.Player;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService
{
    public interface IRecordingService : IService
    {
        void Initialize();
        void SetPlayerRecording(PlayerBehavior playerBehavior);
        void SetSlider(Slider timelineSlider);
        void CleanUp();
    }
}