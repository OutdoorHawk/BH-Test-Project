namespace MirrorServiceTest.Code.Infrastructure.Services.TimeControlService
{
    public interface IPauseHandler
    {
        void EnablePause();
        void DisablePause();
    }
}