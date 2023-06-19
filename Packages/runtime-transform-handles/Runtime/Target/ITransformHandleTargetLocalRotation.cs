using UnityEngine;

namespace NativeRobotics.RuntimeTransformHandle
{
    public interface ITransformHandleTargetLocalRotation
    {
        Quaternion Rotation { get; set; }
    }
}