namespace RTCV.CorruptCore.Coroutines
{
    /// <summary>
    /// Note: counts the current frame as one frame
    /// </summary>
    public class WaitFrames : Yielder
    {
        int framesLeft;
        public WaitFrames(int frames = 1)
        {
            framesLeft = frames;
        }

        public override bool Process()
        {
            framesLeft--;
            return (framesLeft <= 0);
        }
    }
}
