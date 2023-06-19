using UnityEngine;

namespace NativeRobotics.RuntimeTransformHandle
{
    public class TransformHandleTargetPosition : MonoBehaviour, ITransformHandleTargetPosition
    {
        public Quaternion LocalRotation
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
        }

        public Vector3 LocalScale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }

        public Quaternion Rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public Vector3 Position
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}