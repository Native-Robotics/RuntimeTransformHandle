using System.Collections.Generic;
using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    public class RotationHandle : MonoBehaviour
    {
        private RuntimeTransformHandle _parentTransformHandle;
        private List<RotationAxis> _axes;
        private Camera _cam;

        public RotationHandle Construct(Camera cam, RuntimeTransformHandle parentTransformHandle)
        {
            _cam = cam;
            _parentTransformHandle = parentTransformHandle;
            transform.SetParent(_parentTransformHandle.transform, false);

            _axes = new List<RotationAxis>();
            
            if (_parentTransformHandle.Axes is HandleAxes.X or HandleAxes.XY or HandleAxes.XZ or HandleAxes.XYZ)
                _axes.Add(CreateRotationAxis()
                    .Construct(_cam, _parentTransformHandle, Vector3.right, Color.red));
            
            if (_parentTransformHandle.Axes is HandleAxes.Y or HandleAxes.XY or HandleAxes.YZ or HandleAxes.XYZ)
                _axes.Add(CreateRotationAxis()
                    .Construct(_cam, _parentTransformHandle, Vector3.up, Color.green));

            if (_parentTransformHandle.Axes is HandleAxes.Z or HandleAxes.YZ or HandleAxes.XZ or HandleAxes.XYZ)
                _axes.Add(CreateRotationAxis()
                    .Construct(_cam, _parentTransformHandle, Vector3.forward, Color.blue));

            return this;
        }

        private RotationAxis CreateRotationAxis() => CreateGameObject().AddComponent<RotationAxis>();
      
        private GameObject CreateGameObject() => _parentTransformHandle.CreateGameObject();

        public void Destroy()
        {
            foreach (var axis in _axes)
                Destroy(axis.gameObject);
            
            Destroy(this);
        }
    }
}