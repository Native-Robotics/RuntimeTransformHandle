using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    public class PositionAxis : HandleBase
    {
        private Vector3 _startPosition;
        private Vector3 _axis;

        private Vector3 _interactionOffset;
        private Ray _raxisRay;
        private Camera _cam;

        public PositionAxis Construct(Camera cam, RuntimeTransformHandle parentTransformHandle, Vector3 axis, Color color)
        {
            _cam = cam;
            ParentTransformHandle = parentTransformHandle;
            _axis = axis;
            DefaultColor = color;

            InitializeMaterial();

            transform.SetParent(parentTransformHandle.transform, false);

            var o = ParentTransformHandle.CreateGameObject();
            o.transform.SetParent(transform, false);
            var mr = o.AddComponent<MeshRenderer>();
            mr.material = Material;
            var mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateCone(2f, .02f, .02f, 8, 1);
            var mc = o.AddComponent<MeshCollider>();
            mc.sharedMesh = MeshUtils.CreateCone(2f, .1f, .02f, 8, 1);
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, axis);

            o = ParentTransformHandle.CreateGameObject();
            o.transform.SetParent(transform, false);
            mr = o.AddComponent<MeshRenderer>();
            mr.material = Material;
            mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateCone(.4f, .2f, .0f, 8, 1);
            mc = o.AddComponent<MeshCollider>();
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, _axis);
            o.transform.localPosition = axis * 2;

            return this;
        }

        public override void Interact(Vector3 previousPosition)
        {
            var cameraRay = _cam.ScreenPointToRay(Input.mousePosition);

            var closestT = HandleMathUtils.ClosestPointOnRay(_raxisRay, cameraRay);
            var hitPoint = _raxisRay.GetPoint(closestT);

            var offset = hitPoint + _interactionOffset - _startPosition;

            var snapping = ParentTransformHandle.positionSnap;
            var snap = Vector3.Scale(snapping, _axis).magnitude;
            if (snap != 0 && ParentTransformHandle.snappingType == HandleSnappingType.Relative)
            {
                offset = (Mathf.Round(offset.magnitude / snap) * snap) * offset.normalized;
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

        public override void StartInteraction(Vector3 hitPoint)
        {
            base.StartInteraction(hitPoint);

            _startPosition = ParentTransformHandle.TargetPosition.Position;

            var raxis = ParentTransformHandle.Space == HandleSpace.Local
                ? ParentTransformHandle.TargetRotation.Rotation * _axis
                : _axis;

            _raxisRay = new Ray(_startPosition, raxis);

            var cameraRay = _cam.ScreenPointToRay(Input.mousePosition);

            var closestT = HandleMathUtils.ClosestPointOnRay(_raxisRay, cameraRay);
            hitPoint = _raxisRay.GetPoint(closestT);
            _interactionOffset = _startPosition - hitPoint;
        }
    }
}