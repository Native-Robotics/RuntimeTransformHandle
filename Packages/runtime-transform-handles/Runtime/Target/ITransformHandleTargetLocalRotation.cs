using UnityEngine;

namespace NativeRobotics.RuntimeTransformHandle
{
    public interface ITransformHandleTargetLocalRotation
    {
        Quaternion LocalRotation { get; set; }
    }
}