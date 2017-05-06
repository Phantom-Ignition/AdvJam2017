using Nez;
using System.Collections.Generic;

namespace AdvJam2017.Managers
{
    public class SystemManager : IUpdatableManager
    {
        public Dictionary<string, bool> switches;
        public Dictionary<string, int> variables;

        public SystemManager()
        {
            switches = new Dictionary<string, bool>();
            variables = new Dictionary<string, int>();
        }

        public void update()
        { }
    }
}
