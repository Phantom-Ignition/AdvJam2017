using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Sprites;
using Nez.Textures;
using System.Collections.Generic;

namespace AdvJam2017.Components.Sprites
{
    public class AnimatedSprite<T> : Sprite, IUpdatable
    {
        //--------------------------------------------------
        // Frames stuff

        private int _currentFrame;
        public int CurrentFrame => _currentFrame;

        private T _currentFrameList;
        public T CurrentAnimation => _currentFrameList;

        private bool _looped;
        public bool Looped => _looped;

        //--------------------------------------------------
        // Animations

        private Dictionary<T, FramesList> _animations;
        private float _delayTick;

        //----------------------//------------------------//

        public AnimatedSprite(Texture2D texture, T initialFrame) : base(texture)
        {
            _currentFrame = 0;
            _currentFrameList = initialFrame;
            _delayTick = 0;
            _animations = new Dictionary<T, FramesList>();
            _looped = false;
        }
        
        public void CreateAnimation(T animation, float delay)
        {
            _animations[animation] = new FramesList(delay);
        }

        public void CreateAnimation(T animation, float delay, bool reset)
        {
            _animations[animation] = new FramesList(delay);
            _animations[animation].Reset = reset;
        }

        public void CreateAnimation(T animation)
        {
            _animations[animation] = new FramesList(0);
        }

        public void ResetCurrentAnimation()
        {
            _currentFrame = 0;
            _looped = false;
            _delayTick = 0;
        }

        public void AddFrames(T animation, List<Rectangle> frames, int[] offsetX, int[] offsetY)
        {
            for (var i = 0; i < frames.Count; i++)
            {
                var frameSubtexture = new Subtexture(subtexture.texture2D, frames[i]);
                _animations[animation].Frames.Add(new FrameInfo(frameSubtexture, offsetX[i], offsetY[i]));
            }
        }

        public void AddFrames(T animation, List<Rectangle> frames)
        {
            var offsetX = new int[frames.Count];
            var offsetY = new int[frames.Count];
            AddFrames(animation, frames, offsetX, offsetY);
        }

        void IUpdatable.update()
        {
            if (_animations[_currentFrameList].Loop)
            {
                _delayTick += Time.deltaTime;
                if (_delayTick > _animations[_currentFrameList].Delay)
                {
                    _delayTick -= _animations[_currentFrameList].Delay;
                    _currentFrame++;
                    if (_currentFrame == _animations[_currentFrameList].Frames.Count)
                    {
                        if (!_animations[_currentFrameList].Reset)
                        {
                            _currentFrame--;
                            _animations[_currentFrameList].Loop = false;
                        }
                        else _currentFrame = 0;
                        if (!_looped) _looped = true;
                    }
                }
                var rsubtexture = _animations[_currentFrameList].Frames[_currentFrame].Subtexture;
                setSubtexture(rsubtexture);
            }
        }

        public void play(T animation)
        {
            _currentFrame = 0;
            _delayTick = 0;
            _currentFrameList = animation;
            _looped = false;
            if (!_animations[_currentFrameList].Reset)
            {
                _animations[_currentFrameList].Loop = true;
            }
        }
    }
}
