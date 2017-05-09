using Nez;

namespace AdvJam2017.Scenes.SceneMapExtensions
{
    public interface ISceneMapExtensionable
    {
        Scene Scene { get; set; }
        void initialize();
        void update();
    }
}
