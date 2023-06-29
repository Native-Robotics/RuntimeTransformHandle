using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class PositionPlane : HandleBase
    {
        private Vector3 _startPosition;
        private Vector3 _axis1;
        private Vector3 _axis2;
        private Vector3 _perp;
        private Plane _plane;
        private Vector3 _interactionOffset;
        private GameObject _handle;
        private Camera _cam;

        public PositionPlane Construct(Camera cam, RuntimeTransformHandle runtimeHandle, Vector3 axis1, Vector3 axis2, Vector3 perp, Color color)
        {
            _cam = cam;
            ParentTransformHandle = runtimeHandle;
            DefaultColor = color;
            _axis1 = axis1;
            _axis2 = axis2;
            _perp = perp;

            InitializeMaterial();

            transform.SetParent(runtimeHandle.transform, false);

            _handle = ParentTransformHandle.CreateGameObject();
            _handle.transform.SetParent(transform, false);
            var mr = _handle.AddComponent<MeshRenderer>();
            mr.material = Material;
            var mf = _handle.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateBox(.02f, .5f, 0.5f);
            var unused = _handle.AddComponent<MeshCollider>();
            _handle.transform.localRotation = Quaternion.FromToRotation(Vector3.up, _perp);
            _handle.transform.localPosition = (_axis1 + _axis2) * .25f;

            return this;
        }

        public override void Interact(Vector3 previousPosition)
        {
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            _plane.Raycast(ray, out var d);
            
            var hitPoint = ray.GetPoint(d);

            var offset = hitPoint + _interactionOffset - _startPosition;

            var axis = _axis1 + _axis2;
            var snapping = ParentTransformHandle.positionSnap;
            var snap = Vector3.Scale(snapping, axis).magnitude;
            if (snap != 0 && ParentTransformHandle.snappingType == HandleSnappingType.Relative)
            {
                if (snapping.x != 0) offset.x = Mathf.Round(offset.x / snapping.x) * snapping.x;
                if (snapping.y != 0) offset.y = Mathf.Round(offset.y / snapping.y) * snapping.y;
                if (snapping.z != 0) offset.z = Mathf.Round(offset.z / snapping.z) * snapping.z;
            }

            var position = _startPosition + offset;
            
            if (snap != 0 && ParentTransformHandle.snappingType == HandleSnappingType.Absolute)
            {
                if (snapping.x != 0) position.x = Mathf.Round(position.x / snapping.x) * snapping.x;
                if (snapping.y != 0) position.y = Mathf.Round(position.y / snapping.y) * snapping.y;
                if (snapping.x != 0) position.z = Mathf.Round(position.z / snapping.z) * snapping.z;
            }

            ParentTransformHandle.TargetPosition.Position = position;

            base.Interact(previousPosition);
        }

        public override void StartInteraction(Vector3 _)
        {
            var rperp = ParentTransformHandle.Space == HandleSpace.Local
                ? ParentTransformHandle.TargetRotation.Rotation * _perp
                : _perp;
            
            _plane = new Plane(rperp, ParentTransformHandle.TargetPosition.Position);
            
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            _plane.Raycast(ray, out var d);
            
            var hitPoint = ray.GetPoint(d);
            _startPosition = ParentTransformHandle.TargetPosition.Position;
            _interactionOffset = _startPosition - hitPoint;
        }

        private void Update()
        {
            var axis1 = _axis1;
            var raxis1 = ParentTransformHandle.Space == HandleSpace.Local
                ? ParentTransformHandle.TargetRotation.Rotation * axis1
                : axis1;
            var angle1 = Vector3.Angle(ParentTransformHandle.handleCamera.transform.forward, raxis1);
            if (angle1 < 90)
                axis1 = -axis1;

            var axis2 = _axis2;
            var raxis2 = ParentTransformHandle.Space == HandleSpace.Local
                ? ParentTransformHandle.TargetRotation.Rotation * axis2
                : axis2;
            var angle2 = Vector3.Angle(ParentTransformHandle.handleCamera.transform.forward, raxis2);
            if (angle2 < 90)
                axis2 = -axis2;

            _handle.transform.localPosition = (axis1 + axis2) * .25f;
        }
    }
}