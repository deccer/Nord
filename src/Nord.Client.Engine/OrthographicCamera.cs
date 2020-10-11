using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    public sealed class OrthographicCamera : Camera
    {
        public OrthographicCamera(int width, int height, float nearPlane, float farPlane)
            : base(width, height, nearPlane, farPlane)
        {
        }

        protected override void InvalidateProjectionMatrix()
        {
            ProjectionMatrix = Matrix.CreateOrthographic(Width, Height, NearPlane, FarPlane) *
                               Matrix.CreateScale(Zoom);
        }
    }
}
