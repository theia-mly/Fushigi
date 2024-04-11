using System.Numerics;

namespace Fushigi.ui
{
    public class Transform
    {
        public Vector3 Position {  get; set; }
        public Vector3 RotationEuler { get; set; }
        public Vector3 Scale { get; set; } = Vector3.One;

        public event Action? Update;

        internal virtual void OnUpdate()
        {
            Update?.Invoke();
        }
    }
}
