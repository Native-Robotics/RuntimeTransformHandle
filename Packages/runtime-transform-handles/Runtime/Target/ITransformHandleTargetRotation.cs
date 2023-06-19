using UnityEngine;

namespace NativeRobotics.RuntimeTransformHandle
{
    public interface ITransformHandleTargetRotation
    {
        Quaternion Rotation { get; set; }
    }
}