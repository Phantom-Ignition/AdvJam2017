using AdvJam2017.Inventory.Items;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvJam2017.Inventory
{
    class PlayerInventory
    {
        private List<ItemBase> _items;
        public List<ItemBase> items => _items;

        public PlayerInventory()
        {
            _items = new List<ItemBase>();
            _items.Add(new RedItem());
            _items.Add(new GreenItem());
            _items.Add(new RedItem());
        }
    }
}
