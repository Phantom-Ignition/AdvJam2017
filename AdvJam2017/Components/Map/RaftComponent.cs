using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using AdvJam2017.Components.Player;
using Nez.Tweens;
using AdvJam2017.Managers;
using System;

namespace AdvJam2017.Components.Map
{
    class RaftComponent : Component, IUpdatable
    {
        private Sprite _sprite;

        private Entity _playerEntity;
        private Entity _bagEntity;

        private BoxCollider _raftCollider;
        private BoxCollider _leftBarrierCollider;
        private BoxCollider _rightBarrierCollider;

        private Vector2 _position;
        private float _floatFactor;

        private int _playerOnTop;
        private ITween<float> _impactTween; 
        private float _impact;

        private float _raftMovement;

        public RaftComponent(Entity player, Entity bag)
        {
            _playerEntity = player;
            _bagEntity = bag;
            _impact = 0.0f;
        }

        public override void onAddedToEntity()
        {
            _position = new Vector2(191, 228);

            var raftTexture = entity.scene.content.Load<Texture2D>(Content.Misc.raft);
            _sprite = entity.addComponent(new Sprite(raftTexture));
            _sprite.transform.position = _position;

            var w = raftTexture.Width;
            var h = raftTexture.Height;
            _raftCollider = entity.addComponent(new BoxCollider(-w / 2, -h / 2, w, h));

            _leftBarrierCollider = entity.addComponent(new BoxCollider(-w / 2, -128, 1, 128));
            _rightBarrierCollider = entity.addComponent(new BoxCollider(w / 2, -128, 1, 128));
            _rightBarrierCollider.enabled = false;

            transform.position = _position;
        }

        void IUpdatable.update()
        {
            updateFloat();

            CollisionResult collisionResult;

            // Collision of the raft with the player
            var playerComponent = _playerEntity.getComponent<PlayerComponent>();
            var vel = playerComponent.Velocity * Time.deltaTime;
            if (_playerEntity.getComponent<BoxCollider>().collidesWith(_raftCollider, vel, out collisionResult))
            {
                _playerEntity.transform.position += (vel - collisionResult.minimumTranslationVector) * Vector2.UnitY;
                if (_playerOnTop <= 0)
                {
                    _impactTween = this.tween("_impact", 5.0f, 0.5f).setEaseType(EaseType.Punch);
                    _impactTween.start();
                }
                playerComponent.ForcedGround = true;
                playerComponent.platformerObject.velocity.Y = 0.0f;
                _playerOnTop = 10;
            }
            else if (_playerOnTop > 0)
            {
                _playerOnTop--;
            }

            // Collision of the left barrier with the player
            if (_playerEntity.getComponent<BoxCollider>().collidesWith(_leftBarrierCollider, vel, out collisionResult))
            {
                _playerEntity.transform.position += (vel - collisionResult.minimumTranslationVector) * Vector2.UnitX;
                playerComponent.platformerObject.velocity.X = 0.0f;
            }

            // Collision of the right barrier with the player
            if (_rightBarrierCollider.enabled && _playerEntity.getComponent<BoxCollider>().collidesWith(_rightBarrierCollider, vel, out collisionResult))
            {
                _playerEntity.transform.position += (vel - collisionResult.minimumTranslationVector) * Vector2.UnitX;
                playerComponent.platformerObject.velocity.X = 0.0f;
            }

            // Collision with the bag
            vel = Vector2.UnitY;
            if (_bagEntity.getComponent<BoxCollider>().collidesWith(_raftCollider, vel, out collisionResult))
            {
                _bagEntity.transform.position += (vel - collisionResult.minimumTranslationVector) * Vector2.UnitY;
                _bagEntity.getComponent<PlatformerObject>().velocity.Y = 0.0f;
            }

            if (Core.getGlobalManager<SystemManager>().getSwitch("picked_up_bag"))
            {
                _rightBarrierCollider.enabled = true;
                _raftMovement -= 10.0f * Time.deltaTime;

                var movementVector = _raftMovement * Vector2.UnitX;

                _position += movementVector;
                if (_playerOnTop > 5.0f) {
                    _playerEntity.transform.position += movementVector;
                }
            }
        }

        private void updateFloat()
        {
            _floatFactor += 0.10f;
            transform.position = _position + (Mathf.sin(_floatFactor) * 0.8f + _impact) * Vector2.UnitY;
            if (_impact == 5.0f)
            {
                _impactTween.stop();
                _impactTween = this.tween("_impact", 0.0f, 0.5f).setEaseType(EaseType.Punch);
                _impactTween.start();
            }
        }
    }
}
