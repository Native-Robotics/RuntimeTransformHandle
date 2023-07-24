using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    public class ScaleAxis : HandleBase
    {
        private const float Size = 2;

        private Vector3 _axis;
        private Vector3 _startScale;

        private float _interactionDistance;
        private Ray _raxisRay;
        private Camera _cam;

        public ScaleAxis Construct(Camera cam, RuntimeTransformHandle parentTransformHandle, Vector3 axis,
            Color pColor, Shader shader)
        {
            _cam = cam;
            ParentTransformHandle = parentTransformHandle;
            _axis = axis;
            DefaultColor = pColor;

            InitializeMaterial(shader);

            transform.SetParent(parentTransformHandle.transform, false);

            var o = ParentTransformHandle.CreateGameObject();
            o.transform.SetParent(transform, false);
            var mr = o.AddComponent<MeshRenderer>();
            mr.material = Material;
            var mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateCone(axis.magnitude * Size, .02f, .02f, 8, 1);
            var mc = o.AddComponent<MeshCollider>();
            mc.sharedMesh = MeshUtils.CreateCone(axis.magnitude * Size, .1f, .02f, 8, 1);
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, axis);

            o = ParentTransformHandle.CreateGameObject();
            o.transform.SetParent(transform, false);
            mr = o.AddComponent<MeshRenderer>();
            mr.material = Material;
            mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateBox(.25f, .25f, .25f);
            o.AddComponent<MeshCollider>();
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, axis);
            o.transform.localPosition = axis * Size;

            return this;
        }

        protected void Update()
        {
            transform.GetChild(0).localScale = new Vector3(1, 1 + delta, 1);
            transform.GetChild(1).localPosition = _axis * (Size * (1 + delta));
        }

        public override void Interact(Vector3 previousPosition)
        {
            var cameraRay = _cam.ScreenPointToRay(Input.mousePosition);

            var closestT = HandleMathUtils.ClosestPointOnRay(_raxisRay, cameraRay);
            var hitPoint = _raxisRay.GetPoint(closestT);

            var distance = Vector3.Distance(ParentTransformHandle.TargetPosition.Position, hitPoint);
            var axisScaleDelta = distance / _interactionDistance - 1f;

            var snapping = ParentTransformHandle.scaleSnap;
            var snap = Mathf.Abs(Vector3.Dot(snapping, _axis));
            if (snap != 0)
            {
                if (ParentTransformHandle.snappingType == HandleSnappingType.Relative)
                {
                    axisScaleDelta = Mathf.Round(axisScaleDelta / snap) * snap;
                }
                else
                {
                    var axisStartScale = Mathf.Abs(Vector3.Dot(_startScale, _axis));
                    axisScaleDelta = Mathf.Round((axisScaleDelta + axisStartScale) / snap) * snap - axisStartScale;
                }
            }

            delta = axisScaleDelta;
            var scale = Vector3.Scale(_startScale, _axis * axisScaleDelta + Vector3.one);

            ParentTransformHandle.TargetScaleTarget.LocalScale = scale;

            base.Interact(previousPosition);
        }

        public override void StartInteraction(Vector3 hitPoint)
        {
            base.StartInteraction(hitPoint);
            _startScale = ParentTransformHandle.TargetScaleTarget.LocalScale;

            var raxis = ParentTransformHandle.Space == HandleSpace.Local
                ? ParentTransformHandle.TargetRotation.Rotation * _axis
                : _axis;

            _raxisRay = new Ray(ParentTransformHandle.TargetPosition.Position, raxis);

            var cameraRay = _cam.ScreenPointToRay(Input.mousePosition);

            var closestT = HandleMathUtils.ClosestPointOnRay(_raxisRay, cameraRay);
            hitPoint = _raxisRay.GetPoint(closestT);

            _interactionDistance = Vector3.Distance(ParentTransformHandle.TargetPosition.Position, hitPoint);
        }
    }
}