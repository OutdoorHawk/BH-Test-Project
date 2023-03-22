
using MirrorServiceTest.Code.Infrastructure.DI;
using UnityEngine;

namespace MirrorServiceTest.Code.Infrastructure.Services.RecordingService
{
    public interface IRecordingService : IService
    {
        void Initialize();
        void SetPlayerRecording(Transform transform);
        void LoadFrameData(long frame);
        void CleanUp();
    }
}