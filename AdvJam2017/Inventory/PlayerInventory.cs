using AdvJam2017.Inventory.Items;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvJam2017.Inventory
{
    public class PlayerInventory
    {
        public const int MaxItems = 25;
        public const int MaxColumns = 5;

        private ItemBase[] _items;
        public ItemBase[] items => _items;

        public PlayerInventory()
        {
            _items = new ItemBase[MaxItems];
            _items[0] = new RedItem();
            _items[1] = new GreenItem();
            _items[2] = new RedItem();
        }

        public ItemBase getItem(int x, int y)
        {
            return _items[x + y * MaxColumns];
        }
    }
}
