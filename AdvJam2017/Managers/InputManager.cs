using Microsoft.Xna.Framework.Input;
using Nez;

namespace AdvJam2017.Managers
{
    public class InputManager : IUpdatableManager
    {
        private VirtualButton _interactionButton;
        public VirtualButton InteractionButton => _interactionButton;

        private VirtualButton _jumpButton;
        public VirtualButton JumpButton => _jumpButton;

        private VirtualIntegerAxis _movementAxis;
        public VirtualIntegerAxis MovementAxis => _movementAxis;

        // Blocks all the interaction stuff
        public bool IsBusy { get; set; }

        // Blocks all the input
        public bool IsLocked { get; set; }

        public InputManager()
        {
            _interactionButton = new VirtualButton();
            _interactionButton.nodes.Add(new VirtualButton.KeyboardKey(Keys.F));

            _jumpButton = new VirtualButton();
            _jumpButton.nodes.Add(new VirtualButton.KeyboardKey(Keys.Z));

            _movementAxis = new VirtualIntegerAxis();
            _movementAxis.nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
        }

        public bool isMovementAvailable()
        {
            return !IsBusy && !IsLocked;
        }

        public void update()
        { }
    }
}
