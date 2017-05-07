using AdvJam2017.Managers;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvJam2017.NPCs
{
    class ElliotMother : NpcBase
    {
        public ElliotMother(string name) : base(name)
        {
        }

        protected override void createActionList()
        {
            cinematic(30, 1);
            wait(2);
            focusCamera(entity);
            message("Elliot, stop doing that!");
            closeMessage();
            cinematic(0, 1);
            focusCamera(Core.getGlobalManager<SystemManager>().playerEntity);
        }

        protected override void loadTexture()
        {
            TextureName = Content.Characters.elliotMother;
        }
    }
}
