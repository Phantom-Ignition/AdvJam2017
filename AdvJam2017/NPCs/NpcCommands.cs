using Nez;
using AdvJam2017.Managers;
using AdvJam2017.NPCs;

namespace NezTest.NPCs.Commands
{
    public abstract class NpcCommand
    {
        protected NpcBase _npc;

        public NpcCommand(NpcBase npc)
        {
            _npc = npc;
        }

        public abstract void start();
        public abstract bool update();
    }

    public class NpcMessageCommand : NpcCommand
    {
        private string _text;
        private float _maxWidth;

        public NpcMessageCommand(NpcBase npc, string text, float maxWidth) : base(npc)
        {
            _text = text;
            _maxWidth = maxWidth;
        }

        public override void start()
        {
            _npc.TextWindowComponent.start(_text, _maxWidth);
        }

        public override bool update()
        {
            if (Core.getGlobalManager<InputManager>().InteractionButton.isPressed)
            {
                if (_npc.TextWindowComponent.Playing)
                {
                    _npc.TextWindowComponent.forceFinish();
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class NpcCloseMessageCommand : NpcCommand
    {
        public NpcCloseMessageCommand(NpcBase npc) : base(npc)
        { }

        public override void start()
        {
            _npc.TextWindowComponent.close();
        }

        public override bool update()
        {
            return true;
        }
    }

    public class NpcWaitCommand : NpcCommand
    {
        private float _duration;
        private float _current;

        public NpcWaitCommand(NpcBase npc, float duration) : base(npc)
        {
            _duration = duration;
        }

        public override void start()
        { }

        public override bool update()
        {
            _current += Time.deltaTime;
            if (_current >= _duration)
            {
                return true;
            }
            return false;
        }
    }

    public class NpcSetSwitchCommand : NpcCommand
    {
        private bool _isLocal;
        private string _name;
        private bool _value;

        public NpcSetSwitchCommand(NpcBase npc, bool isLocal, string name, bool value) : base(npc)
        {
            _isLocal = isLocal;
            _name = name;
            _value = value;
        }

        public override void start()
        {
            if (_isLocal)
            {
                _npc.Switches[_name] = _value;
            }
            else
            {
                Core.getGlobalManager<SystemManager>().switches[_name] = _value;
            }
        }

        public override bool update()
        {
            return true;
        }
    }

    public class NpcSetVariableCommand : NpcCommand
    {
        private bool _isLocal;
        private string _name;
        private int _value;

        public NpcSetVariableCommand(NpcBase npc, bool isLocal, string name, int value) : base(npc)
        {
            _isLocal = isLocal;
            _name = name;
            _value = value;
        }

        public override void start()
        {
            if (_isLocal)
            {
                _npc.Variables[_name] = _value;
            }
            else
            {
                Core.getGlobalManager<SystemManager>().variables[_name] = _value;
            }
        }

        public override bool update()
        {
            return true;
        }
    }
}
