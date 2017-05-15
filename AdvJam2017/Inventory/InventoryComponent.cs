using AdvJam2017.Inventory.Items;
using AdvJam2017.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using System;

namespace AdvJam2017.Inventory
{
    class InventoryComponent : RenderableComponent, IUpdatable, IDisposable
    {
        //--------------------------------------------------
        // Constants

        private const int ItemSize = 16;
        private const int ItemSpacing = 5;
        private const int InventoryWidth = 110;

        //--------------------------------------------------
        // Inventory

        private PlayerInventory _inventory => Core.getGlobalManager<PlayerManager>().inventory;

        //--------------------------------------------------
        // Background

        private float _backgroundAlpha;
        private Texture2D _backgroundTexture;

        //--------------------------------------------------
        // Component Size

        public override float width => 110;
        public override float height => 200;

        //--------------------------------------------------
        // Cursor

        private int _x;
        private int _y;

        private Texture2D _cursorTexture;

        //--------------------------------------------------
        // Item Sheet Texture

        private Texture2D _itemSheetTexture;

        //--------------------------------------------------
        // Is Active?
        
        private bool _active;

        //--------------------------------------------------
        // MarkupText Component

        private MarkupText _descriptionText;

        //----------------------//------------------------//

        public InventoryComponent()
        {
            _backgroundTexture = Graphics.createSingleColorTexture(1, 1, Color.Black);
        }

        public override void onAddedToEntity()
        {
            _itemSheetTexture = entity.scene.content.Load<Texture2D>(Content.System.itemsheet);
            _cursorTexture = entity.scene.content.Load<Texture2D>(Content.System.inventorySelector);

            _descriptionText = entity.addComponent<MarkupText>();
            _descriptionText.setTextWidth(150);
            _descriptionText.localOffset = new Vector2(InventoryWidth + 10, 10);
            onIndexChange();

            //activate();
        }

        public void activate()
        {
            Core.getGlobalManager<InputManager>().IsBusy = true;
            _active = true;
            this.tween("_backgroundAlpha", 0.7f, 0.5f).setEaseType(Nez.Tweens.EaseType.SineOut).start();
        }

        public override void render(Graphics graphics, Camera camera)
        {
            if (_active)
            {
                var rect = new Rectangle(0, 0, Core.scene.sceneRenderTargetSize.X, Core.scene.sceneRenderTargetSize.Y);
                graphics.batcher.draw(_backgroundTexture, rect, Color.White * _backgroundAlpha);
            }

            drawItems(graphics);
            drawCursor(graphics);
            drawItemName(graphics);
        }

        private void drawItems(Graphics graphics)
        {
            for (var i = 0; i < _inventory.items.Length; i++)
            {
                var item = _inventory.items[i];
                if (item == null) continue;

                var position = itemPosition(i % PlayerInventory.MaxColumns, i / PlayerInventory.MaxColumns) + entity.position.ToPoint();
                var rect = new Rectangle(position.X, position.Y, ItemSize, ItemSize);
                var iconRect = ItemIcons.getIconRect(item.icon);
                graphics.batcher.draw(_itemSheetTexture, rect, iconRect, Color.White);
            }
        }

        private void drawCursor(Graphics graphics)
        {
            var position = itemPosition(_x, _y).ToVector2() + entity.position + new Vector2(ItemSize / 2, ItemSize / 2);
            var cursorOriginRect = new Rectangle(0, 0, ItemSize, ItemSize);
            var origin = new Vector2(ItemSize) / 2;
            graphics.batcher.draw(_cursorTexture, position, cursorOriginRect, Color.White, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }

        private void drawItemName(Graphics graphics)
        {
            var item = _inventory.getItem(_x, _y);
            if (item == null) return;

            var position = new Vector2(InventoryWidth + 10, 0) + entity.position;
            graphics.batcher.drawString(GameMain.bigBitmapFont, item.name, position, Color.White);
        }

        void IDisposable.Dispose()
        {
            _backgroundTexture.Dispose();
        }

        void IUpdatable.update()
        {
            var oldX = _x;
            var oldY = _y;
            var maxColumn = PlayerInventory.MaxColumns;
            var maxRow = PlayerInventory.MaxItems / maxColumn;
            var inputManager = Core.getGlobalManager<InputManager>();
            if (inputManager.LeftButton.isPressed)
            {
                _x = _x - 1 < 0 ? maxColumn - 1 : _x - 1;
            }
            if (inputManager.RightButton.isPressed)
            {
                _x = _x + 1 >= maxColumn ? 0 : _x + 1;
            }
            if (inputManager.DownButton.isPressed)
            {
                _y = _y + 1 >= maxRow ? 0 : _y + 1;
            }
            if (inputManager.UpButton.isPressed)
            {
                _y = _y - 1 < 0 ? maxRow - 1 : _y - 1;
            }
            if (_x != oldX || _y != oldY)
            {
                onIndexChange();
            }
        }

        private void onIndexChange()
        {
            var item = _inventory.getItem(_x, _y);
            if (item == null)
            {
                _descriptionText.setText(wrapText("Empty"));
            }
            else
            {
                _descriptionText.setText(wrapText(item.description));
            }
            _descriptionText.compile();
        }

        private Point itemPosition(int x, int y)
        {
            var px = ItemSpacing + (ItemSize + ItemSpacing) * x;
            var py = ItemSpacing + (ItemSize + ItemSpacing) * y;
            return new Point(px, py);
        }

        private string wrapText(string text)
        {
            var model = "<markuptext face='default' color='#ffffff' align='left'><p>{0}</p></markuptext>";
            return string.Format(model, text);
        }
    }
}
