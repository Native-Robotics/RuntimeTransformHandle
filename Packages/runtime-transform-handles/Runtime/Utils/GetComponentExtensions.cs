using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    public static class GetComponentExtensions
    {
        public static bool TryGetComponentInParent<T>(this GameObject p_gameObject, out T p_result)
        {
            p_result = p_gameObject.GetComponentInParent<T>();
            return p_result != null;
        }
    }
}