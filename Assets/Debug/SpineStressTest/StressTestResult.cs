namespace DChildDebug.Spine.Tests
{
    public class StressTestResult
    {
        public StressTestResult(string name, int instanceCount, FPSLog fps, string result)
        {
            this.name = name;
            this.instanceCount = instanceCount;
            this.fps = fps;
            this.result = result;
        }

        public string name { get; }
        public int instanceCount { get; }
        public FPSLog fps { get; }
        public string result { get; }

        public override string ToString()
        {
            return $"{result}: ({instanceCount}) {name} [{fps.aveFPS}FPS] \n";
        }
    }
}