namespace AdvJam2017.Inventory.Items
{
    public abstract class ItemBase
    {
        protected string _name;
        public string name => _name;

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
