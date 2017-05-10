using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvJam2017.Inventory.Items
{
    class RedItem : ItemBase
    {
        protected override void loadIcon()
        {
            _icon = ItemIcon.Red;
        }

        protected override void loadProps()
        {
            _name = "Red";
        }
    }
}
