using Holysoft.Event;

namespace DChild.Gameplay.Environment
{
    public struct SurfaceDetectedEventArgs : IEventActionArgs
    {
        public SurfaceData.FXGroup fxGroup { get; private set; }

        public void Set(SurfaceData.FXGroup fXGroup) => this.fxGroup = fXGroup;
    }
}