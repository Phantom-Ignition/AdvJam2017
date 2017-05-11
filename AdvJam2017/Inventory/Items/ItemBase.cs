namespace AdvJam2017.Inventory.Items
{
    public abstract class ItemBase
    {
        public abstract string name { get; }
        public abstract string description { get; }

        protected ItemIcon _icon;
        public ItemIcon icon => _icon;

        public ItemBase()
        {
            loadProps();
            loadIcon();
        }

        protected abstract void loadIcon();
        protected abstract void loadProps();
    }
}
