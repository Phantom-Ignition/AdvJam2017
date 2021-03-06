﻿using Microsoft.Xna.Framework;
using Nez;
using System.Collections.Generic;

namespace AdvJam2017.NPCs
{
    class RaftStake : NpcBase
    {
        public RaftStake(string name) : base(name)
        { }

        protected override void createAnimations()
        {
            sprite.CreateAnimation(Animations.Stand);
            sprite.AddFrames(Animations.Stand, new List<Rectangle>()
            {
                new Rectangle(0, 0, 29, 35)
            }, new int[] { 0 }, new int[] { 8 });
        }

        protected override void createActionList()
        {
            playerMessage("This rope doesn't seem strong enough for a raft.");
            closePlayerMessage();
        }

        protected override void loadTexture()
        {
            TextureName = Content.Misc.stakeWithRope;
        }
    }
}
