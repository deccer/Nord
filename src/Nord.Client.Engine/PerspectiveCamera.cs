using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    public sealed class PerspectiveCamera : Camera
    {

        private float _fieldOfView;

        public PerspectiveCamera(int width, int height, float nearPlane, float farPlane, float fieldOfView)
            : base(width, height, nearPlane, farPlane)
        {
            FieldOfView = fieldOfView;
        }

        public float FieldOfView
        {
            get => _fieldOfView;
            set
            {
                if (!_fieldOfView.Equals(value))
                {
                    _fieldOfView = value;
                    InvalidateProjectionMatrix();
                }
            }
        }

        protected override void InvalidateProjectionMatrix()
        {
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(_fieldOfView, Width / (float)Height, NearPlane, FarPlane) *
                               Matrix.CreateScale(Zoom);
        }
    }
}
