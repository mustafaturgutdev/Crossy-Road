using System.Collections.Generic;
using System.Numerics;

namespace Blink.KEK.SpaceSystem
{
    public class Space
    {
        private const float TWO_PI_RADIAN = 3.141592f * 2;
        public Vector2 GlobalPosition
        {
            get => _parent == null ? RelativePosition : _parent.GlobalPosition + Vector2.Transform(RelativePosition, Matrix3x2.CreateRotation(_parent.GlobalRotation)) * _parent.GlobalScale;
            set => RelativePosition = _parent == null ? value : Vector2.Transform(value - _parent.GlobalPosition, Matrix3x2.CreateScale(_parent.GlobalScale / 1.0f) * Matrix3x2.CreateRotation(-_parent.GlobalRotation));
        }
        public float GlobalRotation
        {
            get => (_parent == null ? RelativeRotation : RelativeRotation + _parent.GlobalRotation) % TWO_PI_RADIAN;
            set => RelativeRotation = (_parent == null ? value : value - _parent.GlobalRotation) % TWO_PI_RADIAN;
        }
        public Vector2 GlobalScale
        {
            get => _parent == null ? RelativeScale : RelativeScale * _parent.GlobalScale;
            set => RelativeScale = _parent == null ? value : value / _parent.GlobalScale;
        }

        public Vector2 RelativePosition { get; set; } = Vector2.Zero;
        public float RelativeRotation { get; set; } = 0;
        public Vector2 RelativeScale { get; set; } = Vector2.One;


        private static readonly Space _root = new();
        private readonly List<Space> _children = new();
        private Space _parent;

        private Space()
        {
            _parent = null;
        }

        public Space(Space parent = null)
        {
            _parent = parent ?? _root;
            _parent._children.Add(this);
        }

        public void SetParent(Space parent=null)
        {
            if (this == _root || _parent == parent) return;

            Vector2 previousSpacePosition = GlobalPosition;
            float previousSpaceRotation = GlobalRotation;
            Vector2 previousSpaceScale = GlobalScale;

            _parent._children.Remove(this);
            _parent = parent??_root;
            _parent._children.Add(this);

            GlobalPosition = previousSpacePosition;
            GlobalRotation = previousSpaceRotation;
            GlobalScale = previousSpaceScale;
        }

    }
}
