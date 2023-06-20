using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    public interface ITransformHandleTargetLocalRotation
    {
        Quaternion LocalRotation { get; set; }
    }
}