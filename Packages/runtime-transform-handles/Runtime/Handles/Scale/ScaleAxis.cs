using System.IO;
using System.Security.Permissions;
using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class ScaleAxis : HandleBase
    {
        private const float SIZE = 2;
        
        private Vector3 _axis;
        private Vector3 _startScale;

        private float _interactionDistance;
        private Ray     _raxisRay;
        
        public ScaleAxis Initialize(RuntimeTransformHandle p_parentTransformHandle, Vector3 p_axis, Color p_color)
        {
            _parentTransformHandle = p_parentTransformHandle;
            _axis = p_axis;
            _defaultColor = p_color;

            InitializeMaterial();

            transform.SetParent(p_parentTransformHandle.transform, false);

            var o = _parentTransformHandle.CreateGameObject();
            o.transform.SetParent(transform, false);
            var mr = o.AddComponent<MeshRenderer>();
            mr.material = _material;
            var mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateCone(p_axis.magnitude * SIZE, .02f, .02f, 8, 1);
            var mc = o.AddComponent<MeshCollider>();
            mc.sharedMesh = MeshUtils.CreateCone(p_axis.magnitude * SIZE, .1f, .02f, 8, 1);
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, p_axis);

            o = _parentTransformHandle.CreateGameObject();
            o.transform.SetParent(transform, false);
            mr = o.AddComponent<MeshRenderer>();
            mr.material = _material;
            mf = o.AddComponent<MeshFilter>();
            mf.mesh = MeshUtils.CreateBox(.25f, .25f, .25f);
            mc = o.AddComponent<MeshCollider>();
            o.transform.localRotation = Quaternion.FromToRotation(Vector3.up, p_axis);
            o.transform.localPosition = p_axis * SIZE;

            return this;
        }

        protected void Update()
        {
            transform.GetChild(0).localScale = new Vector3(1, 1+delta, 1);
            transform.GetChild(1).localPosition = _axis * (SIZE * (1 + delta));
        }

        public override void Interact(Vector3 p_previousPosition)
        {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            var   closestT = HandleMathUtils.ClosestPointOnRay(_raxisRay, cameraRay);
            var hitPoint = _raxisRay.GetPoint(closestT);
            
            var distance = Vector3.Distance(_parentTransformHandle.TargetPosition.Position, hitPoint);
            var axisScaleDelta    = distance / _interactionDistance - 1f;

            var snapping = _parentTransformHandle.scaleSnap;
            var   snap     = Mathf.Abs(Vector3.Dot(snapping, _axis));
            if (snap != 0)
            {
                if (_parentTransformHandle.snappingType == HandleSnappingType.RELATIVE)
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

            _parentTransformHandle.TargetScaleTarget.LocalScale = scale;

            base.Interact(p_previousPosition);
        }

        public override void StartInteraction(Vector3 p_hitPoint)
        {
            base.StartInteraction(p_hitPoint);
            _startScale = _parentTransformHandle.TargetScaleTarget.LocalScale;

            var raxis = _parentTransformHandle.Space == HandleSpace.LOCAL
                ? _parentTransformHandle.TargetRotation.Rotation * _axis
                : _axis;
            
            _raxisRay = new Ray(_parentTransformHandle.TargetPosition.Position, raxis);
            
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            var   closestT = HandleMathUtils.ClosestPointOnRay(_raxisRay, cameraRay);
            var hitPoint = _raxisRay.GetPoint(closestT);
            
            _interactionDistance = Vector3.Distance(_parentTransformHandle.TargetPosition.Position, hitPoint);
        }
    }
}