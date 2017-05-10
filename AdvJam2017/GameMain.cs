using AdvJam2017.Scenes;
using AdvJam2017.Managers;
using Microsoft.Xna.Framework;
using Nez;

namespace AdvJam2017
{
    public class GameMain : Core
    {
        public GameMain() : base(width: 852, height: 480, isFullScreen: false, enableEntitySystems: true)
        {
            IsMouseVisible = true;
            Window.AllowUserResizing = true;
            debugRenderEnabled = true;

            IsFixedTimeStep = true;

            // Register Global Managers
            registerGlobalManager(new InputManager());
            registerGlobalManager(new SystemManager());
            registerGlobalManager(new PlayerManager());
        }

        protected override void Initialize()
        {
            base.Initialize();
            Scene.setDefaultDesignResolution(427, 240, Scene.SceneResolutionPolicy.FixedHeight);

            // PP Fix
            scene = Scene.createWithDefaultRenderer();
            base.Update(new GameTime());
            base.Draw(new GameTime());

            // Set first scene
            getGlobalManager<SystemManager>().setMapId(4);
            scene = new SceneMap();
        }
    }
}
