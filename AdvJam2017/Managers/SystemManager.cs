using Nez;
using System.Collections.Generic;

namespace AdvJam2017.Managers
{
    public class SystemManager : IUpdatableManager
    {
        //--------------------------------------------------
        // Switches & Variables

        public Dictionary<string, bool> switches;
        public Dictionary<string, int> variables;

        //--------------------------------------------------
        // Postprocessors

        public float cinematicAmount;

        //--------------------------------------------------
        // Player

        public Entity playerEntity;

        //----------------------//------------------------//

        public SystemManager()
        {
            switches = new Dictionary<string, bool>();
            variables = new Dictionary<string, int>();
        }

        public void setPlayer(Entity playerEntity)
        {
            this.playerEntity = playerEntity;
        }

        public void update()
        { }
    }
}
