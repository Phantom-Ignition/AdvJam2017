using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvJam2017.Inventory.Items
{
    class RedItem : ItemBase
    {
        public override string name => "Red";

        public override string description => "A red item, what did you expect?";

        protected override void loadIcon()
        {
            _icon = ItemIcon.Red;
        }

        protected override void loadProps()
        {
        }
    }
}
