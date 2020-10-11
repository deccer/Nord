using Microsoft.Xna.Framework;

namespace Nord.Client.Engine
{
    public abstract class Camera
    {
        private int _width;
        private int _height;
        private float _nearPlane;
        private float _farPlane;
        private Vector3 _direction;
        private Vector3 _position;
        private float _zoom;

        protected Camera(int width, int height, float nearPlane, float farPlane)
        {
            _width = width;
            _height = height;
            _nearPlane = nearPlane;
            _farPlane = farPlane;
        }

        public int Width
        {
            get => _width;
            set
            {
                if (_width != value)
                {
                    _width = value;
                    InvalidateProjectionMatrix();
                }
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (_height != value)
                {
                    _height = value;
                    InvalidateProjectionMatrix();
                }
            }
        }

        public float NearPlane
        {
            get => _nearPlane;
            set
            {
                if (!_nearPlane.Equals(value))
                {
                    _nearPlane = value;
                    InvalidateProjectionMatrix();
                }
            }
        }

        public float FarPlane
        {
            get => _farPlane;
            set
            {
                if (!_farPlane.Equals(value))
                {
                    _farPlane = value;
                    InvalidateProjectionMatrix();
                }
            }
        }


        public Vector3 Direction
        {
            get => _direction;
            set
            {
                if (!_direction.Equals(value))
                {
                    _direction = value;
                    InvalidateViewMatrix();
                }
            }
        }

        public Vector3 Position
        {
            get => _position;
            set
            {
                if (_position != value)
                {
                    _position = value;
                    InvalidateViewMatrix();
                }
            }
        }

        public Matrix ProjectionMatrix { get; protected set; }

        public Matrix ViewMatrix { get; set; }

        public float Zoom
        {
            get => _zoom;
            set
            {
                if (!_zoom.Equals(value))
                {
                    _zoom = value;
                    InvalidateProjectionMatrix();
                }
            }
        }


        protected Camera()
        {
            Position = new Vector3(0, 30, 22);
            Direction = new Vector3(0, 0, -1);
        }

        protected abstract void InvalidateProjectionMatrix();

        private void InvalidateViewMatrix()
        {
            ViewMatrix = Matrix.CreateLookAt(Position, Position + Direction, Vector3.Up);
        }
    }
}
