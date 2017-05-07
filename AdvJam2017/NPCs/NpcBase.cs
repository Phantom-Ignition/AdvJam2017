using AdvJam2017.Components;
using AdvJam2017.Components.Player;
using AdvJam2017.Components.Sprites;
using AdvJam2017.Components.Windows;
using AdvJam2017.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using NezTest.NPCs.Commands;
using System.Collections;
using System.Collections.Generic;

namespace AdvJam2017.NPCs
{
    public abstract class NpcBase : Component
    {
        //--------------------------------------------------
        // Sprite

        public enum Animations
        {
            Stand = 0
        }

        public AnimatedSprite<Animations> sprite;
        public bool FlipX { get; set; }

        //--------------------------------------------------
        // Platform Object

        private PlatformerObject _platformerObject;

        //--------------------------------------------------
        // Text Window Component

        private TextWindowComponent _textWindowComponent;
        public TextWindowComponent TextWindowComponent => _textWindowComponent;

        //--------------------------------------------------
        // Name

        public string Name { get; set; }

        //--------------------------------------------------
        // Render Layer

        private int _renderLayer;

        //--------------------------------------------------
        // Commands

        private List<NpcCommand> _commands;

        //--------------------------------------------------
        // Local Switches

        private Dictionary<string, bool> _switches;
        public Dictionary<string, bool> Switches => _switches;

        //--------------------------------------------------
        // Local Variables

        private Dictionary<string, int> _variables;
        public Dictionary<string, int> Variables => _variables;

        //--------------------------------------------------
        // Texture

        protected Texture2D _texture;
        protected string TextureName
        {
            set
            {
                _texture = entity.scene.content.Load<Texture2D>(value);
            }
        }

        //----------------------//------------------------//

        public NpcBase(string name)
        {
            Name = name;
            _commands = new List<NpcCommand>();
            _switches = new Dictionary<string, bool>();
            _variables = new Dictionary<string, int>();
        }

        public override void onAddedToEntity()
        {
            _platformerObject = entity.getComponent<PlatformerObject>();
            _textWindowComponent = entity.getComponent<TextWindowComponent>();

            loadTexture();
            createSprite();
            createAnimations();
            sprite.play(default(Animations));
        }

        protected abstract void loadTexture();
        protected abstract void createActionList();

        private void createSprite()
        {
            sprite = entity.addComponent(new AnimatedSprite<Animations>(_texture, default(Animations)));
            sprite.renderLayer = _renderLayer;
            sprite.flipX = FlipX;
        }

        public void setRenderLayer(int renderLayer)
        {
            _renderLayer = renderLayer;
        }

        protected virtual void createAnimations()
        {
            sprite.CreateAnimation(Animations.Stand);
            sprite.AddFrames(Animations.Stand, new List<Rectangle>()
            {
                new Rectangle(0, 0, 64, 64)
            }, new int[] { 0 }, new int[] { -12 });
        }

        private IEnumerator actionList()
        {
            var index = 0;
            while (true)
            {
                Input.update();
                var command = _commands[index];
                command.start();
                while (!command.update())
                {
                    yield return 0;
                }
                if (++index >= _commands.Count)
                {
                    Core.getGlobalManager<InputManager>().IsBusy = false;
                    sprite.flipX = FlipX;
                    yield break;
                }
            }
        }

        public void executeActionList(bool turnToPlayer)
        {
            if (turnToPlayer)
            {
                var player = Core.getGlobalManager<SystemManager>().playerEntity;
                var differece = transform.position - player.position;
                sprite.flipX = differece.X > 0;
            }
            _commands.Clear();
            createActionList();
            Core.startCoroutine(actionList());
        }

        #region NPC Commands

        protected void message(string message, float maxWidth = -1.0f)
        {
            _commands.Add(new NpcMessageCommand(this, message, maxWidth));
        }

        protected void closeMessage()
        {
            _commands.Add(new NpcCloseMessageCommand(this));
        }

        protected void wait(float duration)
        {
            _commands.Add(new NpcWaitCommand(this, duration));
        }

        protected void setSwitch(string name, bool value)
        {
            _commands.Add(new NpcSetSwitchCommand(this, true, name, value));
        }

        protected bool getSwitch(string name)
        {
            return _switches.ContainsKey(name) ? _switches[name] : false;
        }

        protected void setGlobalSwitch(string name, bool value)
        {
            _commands.Add(new NpcSetSwitchCommand(this, false, name, value));
        }

        protected bool getGlobalSwitch(string name)
        {
            var globalSwitches = Core.getGlobalManager<SystemManager>().switches;
            return globalSwitches.ContainsKey(name) ? globalSwitches[name] : false;
        }

        protected void setVariable(string name, int value)
        {
            _commands.Add(new NpcSetVariableCommand(this, true, name, value));
        }

        protected int getVariable(string name)
        {
            return _variables.ContainsKey(name) ? _variables[name] : 0;
        }

        protected void setGlobalVariable(string name, int value)
        {
            _commands.Add(new NpcSetVariableCommand(this, false, name, value));
        }

        protected int getGlobalVariable(string name)
        {
            var globalVariables = Core.getGlobalManager<SystemManager>().variables;
            return globalVariables.ContainsKey(name) ? globalVariables[name] : 0;
        }

        protected void focusCamera(Entity target)
        {
            _commands.Add(new NpcFocusCameraCommand(this, target));
        }

        protected void cinematic(float amount, float duration)
        {
            _commands.Add(new NpcCinematicCommand(this, amount, duration));
        }

        #endregion
    }
}
