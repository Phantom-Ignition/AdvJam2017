using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Tiled;
using AdvJam2017.Components.PlayerStates;
using AdvJam2017.Components.Sprites;
using AdvJam2017.FSM;
using AdvJam2017.Managers;
using System.Collections.Generic;

namespace AdvJam2017.Components
{
    public class PlayerComponent : Component, IUpdatable
    {
        //--------------------------------------------------
        // Animations

        public enum Animations
        {
            Stand,
            Walking,
            JumpPreparation,
            JumpUpwards,
            JumpFalling,
            JumpLanding,
            Hit,
            Slash
        }

        //--------------------------------------------------
        // Sprite

        public AnimatedSprite<Animations> sprite;

        //--------------------------------------------------
        // Movement Input

        private VirtualIntegerAxis _movementInput;

        //--------------------------------------------------
        // Platformer Object

        PlatformerObject _platformerObject;

        //--------------------------------------------------
        // Collision State

        public TiledMapMover.CollisionState CollisionState => _platformerObject.collisionState;

        //--------------------------------------------------
        // Velocity

        public Vector2 Velocity => _platformerObject.velocity;

        //--------------------------------------------------
        // Finite State Machine

        private FiniteStateMachine<PlayerState, PlayerComponent> _fsm;
        public FiniteStateMachine<PlayerState, PlayerComponent> FSM => _fsm;

        //----------------------//------------------------//

        public override void initialize()
        {
            var texture = entity.scene.content.Load<Texture2D>("characters");

            sprite = entity.addComponent(new AnimatedSprite<Animations>(texture, Animations.Stand));
            sprite.CreateAnimation(Animations.Stand, 0);
            sprite.AddFrames(Animations.Stand, new List<Rectangle>()
            {
                new Rectangle(0, 32, 32, 32),
            });

            sprite.CreateAnimation(Animations.Walking, 0.1f);
            sprite.AddFrames(Animations.Walking, new List<Rectangle>()
            {
                new Rectangle(0, 32, 32, 32),
                new Rectangle(32, 32, 32, 32),
                new Rectangle(64, 32, 32, 32),
                new Rectangle(96, 32, 32, 32),
            });

            sprite.CreateAnimation(Animations.JumpPreparation, 0.1f);
            sprite.AddFrames(Animations.JumpPreparation, new List<Rectangle>()
            {
                new Rectangle(128, 32, 32, 32)
            });

            sprite.CreateAnimation(Animations.JumpUpwards, 0.1f);
            sprite.AddFrames(Animations.JumpUpwards, new List<Rectangle>()
            {
                new Rectangle(160, 32, 32, 32)
            });

            sprite.CreateAnimation(Animations.JumpFalling, 0.1f);
            sprite.AddFrames(Animations.JumpFalling, new List<Rectangle>()
            {
                new Rectangle(192, 32, 32, 32)
            });

            sprite.CreateAnimation(Animations.JumpLanding, 0.1f);
            sprite.AddFrames(Animations.JumpLanding, new List<Rectangle>()
            {
                new Rectangle(224, 32, 32, 32)
            });

            sprite.CreateAnimation(Animations.Hit, 0.1f);
            sprite.AddFrames(Animations.Hit, new List<Rectangle>()
            {
                new Rectangle(224, 32, 32, 32),
                new Rectangle(256, 32, 32, 32),
                new Rectangle(224, 32, 32, 32),
            });

            sprite.CreateAnimation(Animations.Slash, 0.1f, false);
            sprite.AddFrames(Animations.Slash, new List<Rectangle>()
            {
                new Rectangle(320, 32, 32, 32),
                new Rectangle(352, 32, 32, 32),
                new Rectangle(384, 32, 32, 32),
            });

            _fsm = new FiniteStateMachine<PlayerState, PlayerComponent>(this, new StandState());

            _movementInput = new VirtualIntegerAxis();
            _movementInput.nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
        }

        public override void onAddedToEntity()
        {
            _platformerObject = entity.getComponent<PlatformerObject>();
            _platformerObject.jumpHeight = 5 * 16;
        }

        public void update()
        {
            // Update FSM
            _fsm.update();
            
            var velocity = _movementInput.value;
            if (Core.getGlobalManager<InputManager>().isMovementAvailable() && (velocity > 0 || velocity < 0))
            {
                var po = _platformerObject;
                var mms = po.maxMoveSpeed;
                po.velocity.X = (int)MathHelper.Clamp(po.velocity.X + po.moveSpeed * velocity, -mms, mms);
                sprite.spriteEffects = velocity < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            }
            else
            {
                _platformerObject.velocity.X = 0;
            }
        }

        public void SetAnimation(Animations animation)
        {
            if (sprite.CurrentAnimation != animation)
                sprite.play(animation);
        }

        public void Jump()
        {
            _platformerObject.jump();
        }
    }
}
