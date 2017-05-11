using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvJam2017.Inventory.Items
{
    class GreenItem : ItemBase
    {
        public override string name => "Green";

        public override string description => "A green item, what did you expect?";

        protected override void loadIcon()
        {
            _icon = ItemIcon.Green;
        }

        protected override void loadProps()
        {
        }
    }
}
