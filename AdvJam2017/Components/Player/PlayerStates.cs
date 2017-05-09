using Microsoft.Xna.Framework.Input;
using Nez;
using AdvJam2017.FSM;
using AdvJam2017.Managers;

namespace AdvJam2017.Components.Player.PlayerStates
{
    public class PlayerState : State<PlayerState, PlayerComponent>
    {
        public override void begin() { }

        public override void end() { }

        public override void handleInput()
        {
            if (Core.getGlobalManager<InputManager>().isMovementAvailable())
            {
                if (Input.isKeyPressed(Keys.F))
                {
                    fsm.pushState(new SlashingState());
                }
                if (entity.isOnGround() && Input.isKeyDown(Keys.Z))
                {
                    fsm.changeState(new JumpingState(true));
                }
            }
        }

        public override void update() { }
    }

    public class StandState : PlayerState
    {
        public override void begin()
        {
            entity.SetAnimation(PlayerComponent.Animations.Stand);
        }
        public override void update()
        {
            if (entity.isOnGround())
            {
                if (entity.Velocity.X > 0 || entity.Velocity.X < 0)
                {
                    entity.SetAnimation(PlayerComponent.Animations.Walking);
                }
                else
                {
                    entity.SetAnimation(PlayerComponent.Animations.Stand);
                }
            }
            else
            {
                fsm.changeState(new JumpingState(false));
            }
            base.handleInput();
        }
    }

    public class JumpingState : PlayerState
    {
        private float _jumpTime;
        private float _landTime;
        private bool _needJump;

        public JumpingState(bool needJump)
        {
            _needJump = needJump;
        }

        public override void begin()
        {
            entity.SetAnimation(PlayerComponent.Animations.JumpPreparation);
            if (_needJump)
                entity.Jump();
        }

        public override void update()
        {
            if (entity.isOnGround())
            {
                entity.SetAnimation(PlayerComponent.Animations.JumpLanding);
                _landTime += Time.deltaTime;
                if (_landTime >= 0.1f)
                    fsm.changeState(new StandState());
                return;
            }

            _jumpTime += Time.deltaTime;

            if (_jumpTime >= 0.05f)
            {
                if (entity.Velocity.Y < 0)
                {
                    entity.SetAnimation(PlayerComponent.Animations.JumpUpwards);
                }
                else if (entity.Velocity.Y > 0)
                {
                    entity.SetAnimation(PlayerComponent.Animations.JumpFalling);
                }
            }

            base.handleInput();
        }
    }

    public class SlashingState : PlayerState
    {
        public override void begin()
        {
            entity.SetAnimation(PlayerComponent.Animations.Slash);
        }

        public override void update()
        {
            base.update();
            if (entity.sprite.Looped)
            {
                fsm.popState();
            }
        }
    }
}
