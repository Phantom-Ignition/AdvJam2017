using AdvJam2017.Inventory.Items;
using AdvJam2017.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using System;

namespace AdvJam2017.Inventory
{
    class InventoryComponent : RenderableComponent, IUpdatable, IDisposable
    {
        private PlayerInventory _inventory => Core.getGlobalManager<PlayerManager>().inventory;

        private Texture2D _backgroundTexture;

        public override float width => (ItemSize + ItemSpacing) * MaxItems;
        public override float height => (ItemSize * MaxItems) + ItemSpacing * 2;

        private float _backgroundAlpha;

        private int _index;
        public int index => _index;

        private Texture2D _cursorTexture;
        private Texture2D _itemSheetTexture;

        private bool _active;

        private const int ItemSize = 16;
        private const int ItemSpacing = 2;
        private const int MaxItems = 10;

        public InventoryComponent()
        {
            _backgroundTexture = Graphics.createSingleColorTexture(1, 1, Color.Black);
        }

        public override void onAddedToEntity()
        {
            _itemSheetTexture = entity.scene.content.Load<Texture2D>(Content.System.itemsheet);
            _cursorTexture = entity.scene.content.Load<Texture2D>(Content.System.inventoryCursor);
            activate();
        }

        public void activate()
        {
            _active = true;
            _backgroundAlpha = 0.7f;
        }

        public override void render(Graphics graphics, Camera camera)
        {
            var i = 0;

            if (_active)
            {
                var rect = new Rectangle(0, 0, Core.scene.sceneRenderTargetSize.X, Core.scene.sceneRenderTargetSize.Y);
                graphics.batcher.draw(_backgroundTexture, rect, Color.White * _backgroundAlpha);
            }

            foreach (var item in _inventory.items)
            {
                var x = ItemSize * i + ItemSpacing * i;
                var rect = new Vector2(x, 0) + entity.position;
                var iconRect = ItemIcons.getIconRect(item.icon);
                graphics.batcher.draw(_itemSheetTexture, new Rectangle((int)rect.X, (int)rect.Y, 32, 32), iconRect, Color.White);
                i++;
            }

            drawCursor(graphics);
        }

        private void drawCursor(Graphics graphics)
        {
            var x = ItemSize * _index + ItemSpacing * _index + ItemSize / 2;
            var position = new Vector2(x, ItemSize / 2) + entity.position;
            var cursorOriginRect = new Rectangle(0, 0, 32, 32);
            var origin = new Vector2(ItemSize) / 2;
            graphics.batcher.draw(_cursorTexture, position, cursorOriginRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }

        void IDisposable.Dispose()
        {
            _backgroundTexture.Dispose();
        }

        void IUpdatable.update()
        {
            var numCurrentItems = _inventory.items.Count;
            var inputManager = Core.getGlobalManager<InputManager>();
            if (inputManager.LeftButton.isPressed)
            {
                _index = _index - 1 < 0 ? numCurrentItems - 1 : _index - 1;
            }
            if (inputManager.RightButton.isPressed)
            {
                _index = _index + 1 >= numCurrentItems ? 0 : _index + 1;
            }
        }
    }
}
