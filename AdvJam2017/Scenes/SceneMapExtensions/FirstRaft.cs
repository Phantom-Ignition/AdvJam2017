using Nez;
using AdvJam2017.Components.Map;

namespace AdvJam2017.Scenes.SceneMapExtensions
{
    class FirstRaft : ISceneMapExtensionable
    {
        public Scene Scene { get; set; }

        public void initialize()
        {
            var raft = Scene.createEntity("raft");
            var player = Scene.findEntity("player");
            var bag = Scene.findEntity("Bag:0");
            raft.addComponent(new RaftComponent(player, bag));
        }

        public void update()
        {

        }
    }
}
