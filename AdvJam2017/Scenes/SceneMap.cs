﻿using AdvJam2017.Components;
using AdvJam2017.Components.Map;
using AdvJam2017.Components.Player;
using AdvJam2017.Components.Windows;
using AdvJam2017.Extensions;
using AdvJam2017.Managers;
using AdvJam2017.NPCs;
using AdvJam2017.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Particles;
using Nez.Tiled;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public const int MISC_RENDER_LAYER = 4; // NPCs, Text, etc.
        public const int PLAYER_RENDER_LAYER = 3;
        public const int PARTICLES_RENDER_LAYER = 2;

        //--------------------------------------------------
        // PostProcessors

        private CinematicLetterboxPostProcessor _cinematicPostProcessor;

        //--------------------------------------------------
        // Map
        
        private TiledMap _tiledMap;

        //----------------------//------------------------//

        public override void initialize()
        {
            addRenderer(new DefaultRenderer());
            setupMap();
            setupPlayer();
            setupEntityProcessors();
            setupNpcs();
            setupParticles();
            setupMapTexts();
            setupTransfers();
            setupPostProcessors();
        }

        public override void onStart()
        {
            getEntityProcessor<NpcInteractionSystem>().mapStart();
        }

        private void setupMap()
        {
            var sysManager = Core.getGlobalManager<SystemManager>();
            var mapId = sysManager.MapId;
            _tiledMap = content.Load<TiledMap>(string.Format("maps/map{0}", mapId));
            sysManager.setTiledMapComponent(_tiledMap);

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
            var sysManager = Core.getGlobalManager<SystemManager>();

            var collisionLayer = _tiledMap.properties["collisionLayer"];
            Vector2? playerSpawn = null;

            if (sysManager.SpawnPosition.HasValue)
            {
                playerSpawn = sysManager.SpawnPosition;
            }
            else
            {
                playerSpawn = _tiledMap.getObjectGroup("objects").objectWithName("playerSpawn").position;
            }

            var player = createEntity("player");
            player.transform.position = playerSpawn.Value;
            player.addComponent(new TiledMapMover(_tiledMap.getLayer<TiledTileLayer>(collisionLayer)));
            player.addComponent(new BoxCollider(-10f, -20f, 20f, 40f));
            player.addComponent(new InteractionCollider(-20f, -6, 40, 22));
            player.addComponent<PlatformerObject>();
            var playerComponent = player.addComponent<PlayerComponent>();
            playerComponent.sprite.renderLayer = PLAYER_RENDER_LAYER;

            Core.getGlobalManager<SystemManager>().setPlayer(player);
        }

        private void setupNpcs()
        {
            var npcObjects = _tiledMap.getObjectGroup("npcs");
            if (npcObjects == null) return;

            var collisionLayer = _tiledMap.properties["collisionLayer"];
            var names = new Dictionary<string, int>();
            foreach (var npc in npcObjects.objects)
            {
                names[npc.name] = names.ContainsKey(npc.name) ? ++names[npc.name] : 0;

                var npcEntity = createEntity(string.Format("iu", npc.name, names[npc.name]));
                var npcComponent = (NpcBase)Activator.CreateInstance(Type.GetType("AdvJam2017.NPCs." + npc.type), npc.name);
                npcComponent.setRenderLayer(MISC_RENDER_LAYER);
                npcEntity.addComponent(npcComponent);
                npcEntity.addComponent<PlatformerObject>();
                npcEntity.addComponent<TextWindowComponent>();
                npcEntity.addComponent(new BoxCollider(-14f, -24f, 32f, 44f));
                npcEntity.addComponent(new TiledMapMover(_tiledMap.getLayer<TiledTileLayer>(collisionLayer)));
                npcEntity.position = npc.position + new Vector2(npc.width, npc.height) / 2; ;

                // Props
                if (npc.properties.ContainsKey("flipX") && npc.properties["flipX"] == "true")
                {
                    npcComponent.FlipX = true;
                }
                if (npc.properties.ContainsKey("autorun") && npc.properties["flipX"] == "true")
                {
                    getEntityProcessor<NpcInteractionSystem>().addAutorun(npcComponent);
                }
            }
        }

        private void setupParticles()
        {
            var particles = _tiledMap.getObjectGroup("particles");
            if (particles == null) return;

            var names = new Dictionary<string, int>();
            foreach (var particleObj in particles.objects)
            {
                names[particleObj.name] = names.ContainsKey(particleObj.name) ? ++names[particleObj.name] : 0;

                var particle = createEntity(string.Format("{0}:{1}", particleObj.name, names[particleObj.name]));
                var emitterConf = content.Load<ParticleEmitterConfig>("particles/" + particleObj.type);
                var emitter = particle.addComponent(new ParticleEmitter(emitterConf));
                emitter.renderLayer = PARTICLES_RENDER_LAYER;
                particle.position = particleObj.position + new Vector2(particleObj.width, particleObj.height) / 2;
            }
        }

        private void setupTransfers()
        {
            var transfers = _tiledMap.getObjectGroup("transfers");
            if (transfers == null) return;

            var names = new Dictionary<string, int>();
            foreach (var transferObj in transfers.objects)
            { 
                names[transferObj.name] = names.ContainsKey(transferObj.name) ? ++names[transferObj.name] : 0;

                var entity = createEntity(string.Format("{0}:{1}", transferObj.name, names[transferObj.name]));
                entity.addComponent(new TransferComponent(transferObj));
            }
        }

        private void setupEntityProcessors()
        {
            var player = findEntity("player");
            camera.addComponent(new CameraShake());
            var mapSize = new Vector2(_tiledMap.width * _tiledMap.tileWidth, _tiledMap.height / _tiledMap.tileHeight);
            addEntityProcessor(new CameraSystem(player) { mapLockEnabled = true, mapSize = mapSize, followLerp = 0.07f, deadzoneSize = new Vector2(20, 10) });
            addEntityProcessor(new NpcInteractionSystem(player.getComponent<PlayerComponent>()));
            addEntityProcessor(new TransferSystem(new Matcher().all(typeof(TransferComponent)), player));
        }

        private void setupMapTexts()
        {
            var textObjects = _tiledMap.getObjectGroup("texts");
            if (textObjects == null) return;
            
            var names = new Dictionary<string, int>();
            foreach (var textObj in textObjects.objects)
            {
                names[textObj.name] = names.ContainsKey(textObj.name) ? ++names[textObj.name] : 0;
                var font = Graphics.instance.bitmapFont;
                var position = textObj.position.round();
                var text = textObj.properties["text"];

                if (textObj.properties.ContainsKey("shadowColor"))
                {
                    var shadowColor = textObj.properties["shadowColor"].FromHex();
                    var shadowEntity = createEntity(string.Format("[Shadow] {0}:{1}", textObj.name, names[textObj.name]));
                    shadowEntity.position = textObj.position.round() + Vector2.One;
                    var shadowTextComponent = shadowEntity.addComponent(new Text(font, text, Vector2.Zero, shadowColor));
                    shadowTextComponent.setRenderLayer(MISC_RENDER_LAYER);
                }

                var color = textObj.properties["color"].FromHex();
                var entity = createEntity(string.Format("{0}:{1}", textObj.name, names[textObj.name]));
                entity.position = textObj.position.round();
                var textComponent = entity.addComponent(new Text(font, text, Vector2.Zero, color));
                textComponent.setRenderLayer(MISC_RENDER_LAYER);
            }
        }

        private void setupPostProcessors()
        {
            _cinematicPostProcessor = addPostProcessor(new CinematicLetterboxPostProcessor(1));
        }

        public void reserveTransfer(TransferComponent transferComponent)
        {
            Core.getGlobalManager<SystemManager>().setMapId(transferComponent.destinyId);
            Core.getGlobalManager<SystemManager>().setSpawnPosition(transferComponent.destinyPosition);
            Core.startSceneTransition(new FadeTransition(() => new SceneMap()));
        }

        public override void update()
        {
            base.update();

            if (Input.isKeyPressed(Keys.C))
            {
                var sys = Core.getGlobalManager<SystemManager>();
                sys.tween("cinematicAmount", 50.0f, 2.0f).start();
            }

            var cinematicAmount = Core.getGlobalManager<SystemManager>().cinematicAmount;
            if (cinematicAmount > 0)
            {
                _cinematicPostProcessor.enabled = true;
                _cinematicPostProcessor.letterboxSize = cinematicAmount;
            }
            else
            {
                _cinematicPostProcessor.enabled = false;
            }
        }
    }
}
