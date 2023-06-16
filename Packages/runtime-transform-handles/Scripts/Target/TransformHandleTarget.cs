using UnityEngine;

namespace RuntimeHandle
{
    public class TransformHandleTarget : MonoBehaviour, ITransformHandleTarget
    {
        public Quaternion localRotation
        {
            get => transform.localRotation;
            set => transform.localRotation = value;
        }

        public Vector3 localScale
        {
            get => transform.localScale;
            set => transform.localScale = value;
        }

        public Quaternion rotation
        {
            get => transform.rotation;
            set => transform.rotation = value;
        }

        public Vector3 position
        {
            get => transform.position;
            set => transform.position = value;
        }
    }
}