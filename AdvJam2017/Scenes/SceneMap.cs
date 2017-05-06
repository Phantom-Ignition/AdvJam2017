using AdvJam2017.Components;
using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using System.Linq;

namespace AdvJam2017.Scenes
{
    class SceneMap : Scene
    {
        //--------------------------------------------------
        // Render Layers Constants

        public const int BACKGROUND_RENDER_LAYER = 10;
        public const int TILED_MAP_RENDER_LAYER = 9;
        public const int WATER_RENDER_LAYER = 5;

        //--------------------------------------------------
        // Map

        public int mapId;
        private TiledMap _tiledMap;

        //----------------------//------------------------//

        public override void initialize()
        {
            addRenderer(new DefaultRenderer());
        }

        public override void onStart()
        {
            setupMap();
            setupPlayer();
        }

        private void setupMap()
        {
            _tiledMap = content.Load<TiledMap>(string.Format("maps/map{0}", mapId));

            var tiledEntity = createEntity("tiled-map");
            var collisionLayer = _tiledMap.properties["collisionLayer"];
            var defaultLayers = _tiledMap.properties["defaultLayers"].Split(',').Select(s => s.Trim()).ToArray();

            var tiledComp = tiledEntity.addComponent(new TiledMapComponent(_tiledMap, collisionLayer) { renderLayer = TILED_MAP_RENDER_LAYER });
            tiledComp.setLayersToRender(defaultLayers);
            
            if (_tiledMap.properties.ContainsKey("aboveWaterLayer"))
            {
                var aboveWaterLayer = _tiledMap.properties["aboveWaterLayer"];
                var tiledAboveWater = tiledEntity.addComponent(new TiledMapComponent(_tiledMap) { renderLayer = WATER_RENDER_LAYER });
                tiledAboveWater.setLayerToRender(aboveWaterLayer);
            }
        }

        private void setupPlayer()
        {
            var collisionLayer = _tiledMap.properties["collisionLayer"];
            var playerSpawn = _tiledMap.getObjectGroup("objects").objectWithName("playerSpawn");

            var player = createEntity("player");
            player.transform.position = playerSpawn.position;
            player.addComponent(new TiledMapMover(_tiledMap.getLayer<TiledTileLayer>(collisionLayer)));
            player.addComponent(new BoxCollider(-7f, -6, 15, 22));
            player.addComponent(new InteractionCollider(-20f, -6, 40, 22));
            player.addComponent<PlayerComponent>();
            player.addComponent<PlatformerObject>();
        }
    }
}
