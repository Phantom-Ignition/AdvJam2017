using AdvJam2017.Managers;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;

namespace AdvJam2017.Components
{
    class PlatformerObject : Component, IUpdatable
    {
        //--------------------------------------------------
        // Physics

        public float moveSpeed = 1000;
        public float maxMoveSpeed = 150;
        public float gravity = 1200;
        public float jumpHeight = 32 * 0.9f;

        //--------------------------------------------------
        // Tiled Mover

        TiledMapMover _mover;

        //--------------------------------------------------
        // Box Collider

        BoxCollider _boxCollider;
        
        //--------------------------------------------------
        // Velocity

        public Vector2 velocity;
        
        //--------------------------------------------------
        // Collision State

        public TiledMapMover.CollisionState collisionState = new TiledMapMover.CollisionState();

        //----------------------//------------------------//

        public override void onAddedToEntity()
        {
            _mover = this.getComponent<TiledMapMover>();
            _boxCollider = entity.getComponent<BoxCollider>();
        }

        public void update()
        {
            // apply gravity
            velocity.Y += gravity * Time.deltaTime;

            // apply movement
            _mover.move(velocity * Time.deltaTime, _boxCollider, collisionState);

            // handle map bounds
            var map = Core.getGlobalManager<SystemManager>().TiledMap;
            var x = MathHelper.Clamp(_mover.transform.position.X, 0, map.widthInPixels);
            _mover.transform.position = new Vector2(x, _mover.transform.position.Y);

            // update velocity
            if (collisionState.right || collisionState.left)
                velocity.X = 0;
            if (collisionState.above || collisionState.below)
                velocity.Y = 0;
        }

        public void jump()
        {
            velocity.Y = -Mathf.sqrt(2 * jumpHeight * gravity);
        }
    }
}
