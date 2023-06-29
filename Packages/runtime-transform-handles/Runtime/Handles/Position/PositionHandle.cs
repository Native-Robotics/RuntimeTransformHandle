using System.Collections.Generic;
using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    public class PositionHandle : MonoBehaviour
    {
        private RuntimeTransformHandle _parentTransformHandle;
        private List<PositionAxis> _axes;
        private List<PositionPlane> _planes;

        private static readonly Color DarkBlue = new(0, 0, 1, .2f);
        private static readonly Color DarkRed = new(1, 0, 0, .2f);
        private static readonly Color DarkGreen = new(0, 1, 0, .2f);

        private static readonly Color Blue = Color.blue;
        private static readonly Color Red = Color.red;
        private static readonly Color Green = Color.green;

        public PositionHandle Construct(Camera cam, RuntimeTransformHandle parentTransformHandle)
        {
            _parentTransformHandle = parentTransformHandle;
            transform.SetParent(_parentTransformHandle.transform, false);

            _axes = new List<PositionAxis>();

            if (_parentTransformHandle.Axes is HandleAxes.X or HandleAxes.XY or HandleAxes.XZ or HandleAxes.XYZ)
                _axes.Add(CreatePositionAxis()
                    .Construct(cam, _parentTransformHandle, Vector3.right, Red));

            if (_parentTransformHandle.Axes is HandleAxes.Y or HandleAxes.XY or HandleAxes.YZ or HandleAxes.XYZ)
                _axes.Add(CreatePositionAxis()
                    .Construct(cam, _parentTransformHandle, Vector3.up, Green));

            if (_parentTransformHandle.Axes is HandleAxes.Z or HandleAxes.XZ or HandleAxes.YZ or HandleAxes.XYZ)
                _axes.Add(CreatePositionAxis()
                    .Construct(cam, _parentTransformHandle, Vector3.forward, Blue));

            _planes = new List<PositionPlane>();

            if (_parentTransformHandle.Axes is HandleAxes.XY or HandleAxes.XYZ)
                _planes.Add(CreatePositionPlane()
                    .Construct(cam, _parentTransformHandle, Vector3.right, Vector3.up, -Vector3.forward, DarkBlue));

            if (_parentTransformHandle.Axes is HandleAxes.YZ or HandleAxes.XYZ)
                _planes.Add(CreatePositionPlane()
                    .Construct(cam, _parentTransformHandle, Vector3.up, Vector3.forward, Vector3.right, DarkRed));

            if (_parentTransformHandle.Axes is HandleAxes.XZ or HandleAxes.XYZ)
                _planes.Add(CreatePositionPlane()
                    .Construct(cam, _parentTransformHandle, Vector3.right, Vector3.forward, Vector3.up, DarkGreen));

            return this;
        }

        private PositionAxis CreatePositionAxis() => CreateGameObject().AddComponent<PositionAxis>();

        private PositionPlane CreatePositionPlane() => CreateGameObject().AddComponent<PositionPlane>();

        private GameObject CreateGameObject() => _parentTransformHandle.CreateGameObject();

        public void Destroy()
        {
            foreach (var axis in _axes)
                Destroy(axis.gameObject);

            foreach (var plane in _planes)
                Destroy(plane.gameObject);

            Destroy(this);
        }
    }
}