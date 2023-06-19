using System.Collections.Generic;
using UnityEngine;

namespace NativeRobotics.RuntimeTransformHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class PositionHandle : MonoBehaviour
    {
        protected RuntimeTransformHandle _parentTransformHandle;
        protected List<PositionAxis> _axes;
        protected List<PositionPlane> _planes;

        public PositionHandle Initialize(RuntimeTransformHandle p_runtimeHandle)
        {
            _parentTransformHandle = p_runtimeHandle;
            transform.SetParent(_parentTransformHandle.transform, false);

            _axes = new List<PositionAxis>();

            if (_parentTransformHandle.Axes == HandleAxes.X || _parentTransformHandle.Axes == HandleAxes.XY || _parentTransformHandle.Axes == HandleAxes.XZ || _parentTransformHandle.Axes == HandleAxes.XYZ)
                _axes.Add(new GameObject().AddComponent<PositionAxis>()
                    .Initialize(_parentTransformHandle, Vector3.right, Color.red));
            
            if (_parentTransformHandle.Axes == HandleAxes.Y || _parentTransformHandle.Axes == HandleAxes.XY || _parentTransformHandle.Axes == HandleAxes.YZ || _parentTransformHandle.Axes == HandleAxes.XYZ)
                _axes.Add(new GameObject().AddComponent<PositionAxis>()
                    .Initialize(_parentTransformHandle, Vector3.up, Color.green));

            if (_parentTransformHandle.Axes == HandleAxes.Z || _parentTransformHandle.Axes == HandleAxes.XZ || _parentTransformHandle.Axes == HandleAxes.YZ || _parentTransformHandle.Axes == HandleAxes.XYZ)
                _axes.Add(new GameObject().AddComponent<PositionAxis>()
                    .Initialize(_parentTransformHandle, Vector3.forward, Color.blue));

            _planes = new List<PositionPlane>();
            
            if (_parentTransformHandle.Axes == HandleAxes.XY || _parentTransformHandle.Axes == HandleAxes.XYZ)
                _planes.Add(new GameObject().AddComponent<PositionPlane>()
                    .Initialize(_parentTransformHandle, Vector3.right, Vector3.up, -Vector3.forward, new Color(0,0,1,.2f)));

            if (_parentTransformHandle.Axes == HandleAxes.YZ || _parentTransformHandle.Axes == HandleAxes.XYZ)
                _planes.Add(new GameObject().AddComponent<PositionPlane>()
                    .Initialize(_parentTransformHandle, Vector3.up, Vector3.forward, Vector3.right, new Color(1, 0, 0, .2f)));

            if (_parentTransformHandle.Axes == HandleAxes.XZ || _parentTransformHandle.Axes == HandleAxes.XYZ)
                _planes.Add(new GameObject().AddComponent<PositionPlane>()
                    .Initialize(_parentTransformHandle, Vector3.right, Vector3.forward, Vector3.up, new Color(0, 1, 0, .2f)));

            return this;
        }

        public void Destroy()
        {
            foreach (PositionAxis axis in _axes)
                Destroy(axis.gameObject);
            
            foreach (PositionPlane plane in _planes)
                Destroy(plane.gameObject);
            
            Destroy(this);
        }
    }
}