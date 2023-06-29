using System.Collections.Generic;
using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class RotationHandle : MonoBehaviour
    {
        protected RuntimeTransformHandle _parentTransformHandle;
        protected List<RotationAxis> _axes;

        public RotationHandle Initialize(RuntimeTransformHandle p_parentTransformHandle)
        {
            _parentTransformHandle = p_parentTransformHandle;
            transform.SetParent(_parentTransformHandle.transform, false);

            _axes = new List<RotationAxis>();
            
            if (_parentTransformHandle.Axes is HandleAxes.X or HandleAxes.XY or HandleAxes.XZ or HandleAxes.XYZ)
                _axes.Add(CreateGameObject().AddComponent<RotationAxis>()
                    .Initialize(_parentTransformHandle, Vector3.right, Color.red));
            
            if (_parentTransformHandle.Axes is HandleAxes.Y or HandleAxes.XY or HandleAxes.YZ or HandleAxes.XYZ)
                _axes.Add(CreateGameObject().AddComponent<RotationAxis>()
                    .Initialize(_parentTransformHandle, Vector3.up, Color.green));

            if (_parentTransformHandle.Axes is HandleAxes.Z or HandleAxes.YZ or HandleAxes.XZ or HandleAxes.XYZ)
                _axes.Add(CreateGameObject().AddComponent<RotationAxis>()
                    .Initialize(_parentTransformHandle, Vector3.forward, Color.blue));

            return this;
        }

        private GameObject CreateGameObject() => _parentTransformHandle.CreateGameObject();

        public void Destroy()
        {
            foreach (var axis in _axes)
                Destroy(axis.gameObject);
            
            Destroy(this);
        }
    }
}