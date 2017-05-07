using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvJam2017.NPCs
{
    class NpcTest : NpcBase
    {
        public NpcTest(string name) : base(name)
        {
        }

        protected override void createActionList()
        {
            message("Hello!");
            closeMessage();
        }

        protected override void loadTexture()
        {
            TextureName = Content.Characters.elliot;
        }
    }
}
