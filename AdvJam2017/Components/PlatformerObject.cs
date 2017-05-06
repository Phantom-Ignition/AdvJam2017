﻿using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;

namespace AdvJam2017.Components
{
    class PlatformerObject : Component, IUpdatable
    {
        //--------------------------------------------------
        // Physics

        public float moveSpeed = 10;
        public float maxMoveSpeed = 100;
        public float gravity = 1000;
        public float jumpHeight = 16 * 2;

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