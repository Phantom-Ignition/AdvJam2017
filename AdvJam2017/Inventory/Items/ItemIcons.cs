using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace AdvJam2017.Inventory.Items
{
    public enum ItemIcon
    {
        Red,
        Green,
        White
    }

    public static class ItemIcons
    {
        public static Dictionary<ItemIcon, Rectangle> _iconMap = new Dictionary<ItemIcon, Rectangle>()
        {
            { ItemIcon.Red, new Rectangle(0, 0, 16, 16) },
            { ItemIcon.Green, new Rectangle(16, 0, 16, 16) },
            { ItemIcon.White, new Rectangle(32, 0, 16, 16) },
        };

        public static Rectangle getIconRect(ItemIcon icon)
        {
            return _iconMap[icon];
        }
    }
}
