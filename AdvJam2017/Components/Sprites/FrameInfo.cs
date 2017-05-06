using Nez.Textures;

namespace AdvJam2017.Components.Sprites
{
    public class FrameInfo
    {
        public Subtexture Subtexture { get; private set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        public FrameInfo(Subtexture subtexture, int offsetX, int offsetY)
        {
            Subtexture = subtexture;
            OffsetX = offsetX;
            OffsetY = offsetY;
        }
    }
}
