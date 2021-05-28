namespace DChild.Gameplay.Cinematics.Cameras
{
    public interface ICameraShakeHandle
    {
        bool isDone { get; }

        void SetShakeTo(CameraShakeInfo cameraShakeInfo);
        void UpdateShake(ICinema cinema,float delta);
    }

}