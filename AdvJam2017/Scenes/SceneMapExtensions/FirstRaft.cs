using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            raft.addComponent(new RaftComponent(Scene.findEntity("player")));
        }

        public void update()
        {
        }
    }
}
