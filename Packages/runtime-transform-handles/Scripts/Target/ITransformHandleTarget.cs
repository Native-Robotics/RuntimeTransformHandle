using UnityEngine;

namespace RuntimeHandle
{
    public interface ITransformHandleTarget
    {
        Quaternion rotation { get; set; }
        Vector3 position { get; set; }
        Quaternion localRotation { get; set; }
        Vector3 localScale { get; set; }
    }
}