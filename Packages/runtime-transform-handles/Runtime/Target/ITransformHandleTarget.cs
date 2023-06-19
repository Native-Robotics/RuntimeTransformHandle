using UnityEngine;

namespace NativeRobotics.RuntimeTransformHandle
{
    public interface ITransformHandleTarget
    {
        Quaternion Rotation { get; set; }
        Vector3 Position { get; set; }
        Quaternion LocalRotation { get; set; }
        Vector3 LocalScale { get; set; }
    }
}