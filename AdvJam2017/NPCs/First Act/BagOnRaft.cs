using Microsoft.Xna.Framework;
using Nez;
using System.Collections.Generic;

namespace AdvJam2017.NPCs
{
    class BagOnRaft : NpcBase
    {
        public BagOnRaft(string name) : base(name)
        { }

        protected override void createActionList()
        {
            setGlobalSwitch("picked_up_bag", true);
            /*
            cinematicIn(30.0f, 1.0f);
            playerMessage("Oh, I found the bag!");
            closePlayerMessage();
            wait(0.5f);
            hideTexture();
            setGlobalSwitch("picked_up_bag", true);
            playerMessage("Wait, why is the raft moving?!");
            playerMessage("I don't know how to swim!");
            movePlayer(10.0f * Vector2.UnitX);
            wait(0.3f);
            movePlayer(Vector2.Zero);
            playerMessage("Where am I going?!");
            closePlayerMessage();
            */
        }

        protected override void createAnimations()
        {
            sprite.CreateAnimation(Animations.Stand);
            sprite.AddFrames(Animations.Stand, new List<Rectangle>()
            {
                new Rectangle(0, 0, 12, 11)
            }, new int[] { 0 }, new int[] { 0 });
        }

        protected override void createCollider()
        {
            entity.addComponent(new BoxCollider(-ObjectRect.Width / 2, -ObjectRect.Height / 2, ObjectRect.Width, ObjectRect.Height));
        }

        protected override void loadTexture()
        {
            TextureName = Content.Misc.bag;
        }
    }
}
