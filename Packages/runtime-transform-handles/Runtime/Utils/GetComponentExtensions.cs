using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    public static class GetComponentExtensions
    {
        public static bool TryGetComponentInParent<T>(this GameObject pGameObject, out T pResult)
        {
            pResult = pGameObject.GetComponentInParent<T>();
            return pResult != null;
        }
    }
}