using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using AdvJam2017.Components.Player;
using Nez.Tweens;

namespace AdvJam2017.Components.Map
{
    class RaftComponent : Component, IUpdatable
    {
        private Sprite _sprite;
        private Entity _player;

        private BoxCollider _raftCollider;

        private Vector2 _position;
        private float _floatFactor;

        private int _playerOnTop;
        private ITween<float> _impactTween; 
        private float _impact;

        public RaftComponent(Entity player)
        {
            _player = player;
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

            transform.position = _position;
        }

        void IUpdatable.update()
        {
            updateFloat();

            CollisionResult collisionResult;

            var collider = _raftCollider;
            var playerComponent = _player.getComponent<PlayerComponent>();
            var vel = playerComponent.Velocity * Time.deltaTime;

            if (_player.getComponent<BoxCollider>().collidesWith(collider, vel, out collisionResult))
            {
                _player.transform.position += (vel - collisionResult.minimumTranslationVector) * Vector2.UnitY;
                if (_playerOnTop <= 0)
                {
                    _impactTween = this.tween("_impact", 5.0f, 0.5f).setEaseType(EaseType.Punch);
                    _impactTween.start();
                }
                playerComponent.ForcedGround = true;
                playerComponent.platformerObject.velocity.Y = 0.0f;
                _playerOnTop = 10;
            }
            else
            {
                _playerOnTop--;
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
