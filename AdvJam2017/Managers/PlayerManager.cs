using AdvJam2017.Inventory;
using Nez;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvJam2017.Managers
{
    class PlayerManager : IUpdatableManager
    {
        public const int MaxHp = 5;

        private int _hp;
        public int hp => _hp;

        private PlayerInventory _inventory;
        public PlayerInventory inventory => _inventory;

        public PlayerManager()
        {
            _inventory = new PlayerInventory();
        }

        public void update()
        {
        }
    }
}
