using System.Collections.Generic;

namespace AdvJam2017.Components.Sprites
{
    public class FramesList
    {
        public float Delay { get; set; }
        public List<FrameInfo> Frames { get; set; }
        public bool Loop { get; set; }
        public bool Reset { get; set; }
        public FramesList(float delay)
        {
            Frames = new List<FrameInfo>();

            Delay = delay;
            Loop = Delay > 0;
            Reset = Loop;
        }
    }
}
