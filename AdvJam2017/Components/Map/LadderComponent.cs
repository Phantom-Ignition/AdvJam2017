﻿using Nez;
using Nez.Tiled;

namespace AdvJam2017.Components.Map
{
    public class LadderComponent : Component
    {
        private TiledObject _tiledObject;

        public LadderComponent(TiledObject tiledObject)
        {
            _tiledObject = tiledObject;
        }

        public override void onAddedToEntity()
        {
            entity.addComponent(new BoxCollider(0, 0, _tiledObject.width, _tiledObject.height));
            transform.position = _tiledObject.position;
        }
    }
}
