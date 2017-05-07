using Microsoft.Xna.Framework;
using Nez;
using Nez.Tiled;
using System.Collections.Generic;

namespace AdvJam2017.Managers
{
    public class SystemManager : IUpdatableManager
    {
        //--------------------------------------------------
        // Switches & Variables

        public Dictionary<string, bool> switches;
        public Dictionary<string, int> variables;

        //--------------------------------------------------
        // Postprocessors

        public float cinematicAmount;

        //--------------------------------------------------
        // Player

        public Entity playerEntity;

        //--------------------------------------------------
        // Map

        private int _mapId;
        public int MapId => _mapId;

        private TiledMap _tiledMapComponent;
        public TiledMap TiledMap => _tiledMapComponent;

        private Vector2? _spawnPosition;
        public Vector2? SpawnPosition => _spawnPosition;

        //----------------------//------------------------//

        public SystemManager()
        {
            switches = new Dictionary<string, bool>();
            variables = new Dictionary<string, int>();
        }

        public void setPlayer(Entity playerEntity)
        {
            this.playerEntity = playerEntity;
        }

        public void setMapId(int mapId)
        {
            _mapId = mapId;
        }
        
        public void setTiledMapComponent(TiledMap map)
        {
            _tiledMapComponent = map;
        }

        public void setSpawnPosition(Vector2 position)
        {
            _spawnPosition = position;
        }

        public void update()
        { }
    }
}
