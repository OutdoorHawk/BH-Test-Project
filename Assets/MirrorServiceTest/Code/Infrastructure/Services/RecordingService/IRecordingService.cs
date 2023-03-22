﻿
using MirrorServiceTest.Code.Infrastructure.DI;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService
{
    public interface IRecordingService : IService
    {
        void Initialize();
        void SetPlayerRecording(Transform transform);
        void SetSlider(Slider timelineSlider);
        void CleanUp();
    }
}