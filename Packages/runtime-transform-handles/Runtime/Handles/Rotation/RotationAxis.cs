using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    public class RotationAxis : HandleBase
    {
        private static readonly int CameraPositionPropertyID = Shader.PropertyToID("_CameraPosition");
        private static readonly int CameraDistancePropertyID = Shader.PropertyToID("_CameraDistance");
        
        private Mesh _arcMesh;
        private Material _arcMaterial;
        private Vector3 _axis;
        private Vector3 _rotatedAxis;
        private Plane _axisPlane;
        private Vector3 _tangent;
        private Vector3 _biTangent;

        private Quaternion _startRotation;
        private Camera _cam;

        public RotationAxis Construct(Camera cam, RuntimeTransformHandle pRuntimeHandle, Vector3 pAxis, Color pColor)
        {
            _cam = cam;
            ParentTransformHandle = pRuntimeHandle;
            _axis = pAxis;
            DefaultColor = pColor;

            InitializeMaterial();

            transform.SetParent(pRuntimeHandle.transform, false);

            var o = ParentTransformHandle.CreateGameObject();
            o.transform.SetParent(transform, false);
            var mr = o.AddComponent<MeshRenderer>();
            mr.material = Material;
            var mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateTorus(2f, .02f, 32, 6);
            var mc = o.AddComponent<MeshCollider>();
            mc.sharedMesh = MeshUtils.CreateTorus(2f, .1f, 32, 6);
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, _axis);
            return this;
        }

        protected override void InitializeMaterial()
        {
            Material = new Material(Shader.Find("sHTiF/AdvancedHandleShader"));
            Material.color = DefaultColor;
        }

        public void Update()
        {
            var position = ParentTransformHandle.handleCamera.transform.position;
            Material.SetVector(CameraPositionPropertyID, position);
            Material.SetFloat(CameraDistancePropertyID,
                (position - ParentTransformHandle.transform.position)
                .magnitude);
        }

        public override void Interact(Vector3 previousPosition)
        {
            var cameraRay = _cam.ScreenPointToRay(Input.mousePosition);

            if (!_axisPlane.Raycast(cameraRay, out var hitT))
            {
                base.Interact(previousPosition);
                return;
            }

            var hitPoint = cameraRay.GetPoint(hitT);
            var hitDirection = (hitPoint - ParentTransformHandle.TargetPosition.Position).normalized;
            var x = Vector3.Dot(hitDirection, _tangent);
            var y = Vector3.Dot(hitDirection, _biTangent);
            var angleRadians = Mathf.Atan2(y, x);
            var angleDegrees = angleRadians * Mathf.Rad2Deg;

            if (ParentTransformHandle.rotationSnap != 0)
            {
                angleDegrees = Mathf.Round(angleDegrees / ParentTransformHandle.rotationSnap) *
                               ParentTransformHandle.rotationSnap;
                angleRadians = angleDegrees * Mathf.Deg2Rad;
            }

            if (ParentTransformHandle.Space == HandleSpace.Local)
            {
                ParentTransformHandle.TargetLocalRotation.LocalRotation =
                    _startRotation * Quaternion.AngleAxis(angleDegrees, _axis);
            }
            else
            {
                var invertedRotatedAxis = Quaternion.Inverse(_startRotation) * _axis;
                ParentTransformHandle.TargetRotation.Rotation =
                    _startRotation * Quaternion.AngleAxis(angleDegrees, invertedRotatedAxis);
            }

            _arcMesh = MeshUtils.CreateArc(transform.position, HitPoint, _rotatedAxis, 2, angleRadians,
                Mathf.Abs(Mathf.CeilToInt(angleDegrees)) + 1);
            DrawArc();

            base.Interact(previousPosition);
        }

        public override bool CanInteract(Vector3 hitPoint)
        {
            var position = ParentTransformHandle.handleCamera.transform.position;
            var cameraDistance = (ParentTransformHandle.transform.position -
                                  position).magnitude;
            var pointDistance = (hitPoint - position).magnitude;
            return pointDistance <= cameraDistance;
        }

        public override void StartInteraction(Vector3 hitPoint)
        {
            if (!CanInteract(hitPoint))
                return;


            base.StartInteraction(hitPoint);

            _startRotation = ParentTransformHandle.Space == HandleSpace.Local
                ? ParentTransformHandle.TargetLocalRotation.LocalRotation
                : ParentTransformHandle.TargetRotation.Rotation;

            _arcMaterial = new Material(Shader.Find("sHTiF/HandleShader"));
            _arcMaterial.color = new Color(1, 1, 0, .4f);
            _arcMaterial.renderQueue = 5000;

            if (ParentTransformHandle.Space == HandleSpace.Local)
            {
                _rotatedAxis = _startRotation * _axis;
            }
            else
            {
                _rotatedAxis = _axis;
            }

            _axisPlane = new Plane(_rotatedAxis, ParentTransformHandle.TargetPosition.Position);

            Vector3 startHitPoint;
            var cameraRay = _cam.ScreenPointToRay(Input.mousePosition);
            if (_axisPlane.Raycast(cameraRay, out var hitT))
            {
                startHitPoint = cameraRay.GetPoint(hitT);
            }
            else
            {
                startHitPoint = _axisPlane.ClosestPointOnPlane(hitPoint);
            }

            _tangent = (startHitPoint - ParentTransformHandle.TargetPosition.Position).normalized;
            _biTangent = Vector3.Cross(_rotatedAxis, _tangent);
        }

        public override void EndInteraction()
        {
            base.EndInteraction();
            delta = 0;
        }

        private void DrawArc()
        {
            Graphics.DrawMesh(_arcMesh, Matrix4x4.identity, _arcMaterial, 0);
        }
    }
}