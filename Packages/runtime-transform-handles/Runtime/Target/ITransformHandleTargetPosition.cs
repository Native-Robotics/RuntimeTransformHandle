using UnityEngine;

namespace NativeRobotics.RuntimeTransformHandle
{
    public interface ITransformHandleTargetScale
    {
        Vector3 LocalScale { get; set; }
    }

    public interface ITransformHandleTargetLocalRotation
    {
        Quaternion Rotation { get; set; }
    }

    public interface ITransformHandleTargetRotation
    {
        Quaternion Rotation { get; set; }
    }
    
    public interface ITransformHandleTargetPosition
    {
        Vector3 Position { get; set; }
    }
}