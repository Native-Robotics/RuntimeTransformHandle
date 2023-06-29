using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class ScaleGlobal : HandleBase
    {
        protected Vector3 _axis;
        protected Vector3 _startScale;
        
        public ScaleGlobal Initialize(RuntimeTransformHandle p_parentTransformHandle, Vector3 p_axis, Color p_color)
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
            mf.mesh = MeshUtils.CreateBox(.35f, .35f, .35f);
            var mc = o.AddComponent<MeshCollider>();

            return this;
        }

        public override void Interact(Vector3 p_previousPosition)
        {
            var mouseVector = (Input.mousePosition - p_previousPosition);
            var d = (mouseVector.x + mouseVector.y) * Time.deltaTime * 2;
            delta += d;
            _parentTransformHandle.TargetScaleTarget.LocalScale = _startScale + Vector3.Scale(_startScale,_axis) * delta;
            
            base.Interact(p_previousPosition);
        }

        public override void StartInteraction(Vector3 p_hitPoint)
        {
            base.StartInteraction(p_hitPoint);
            _startScale = _parentTransformHandle.TargetScaleTarget.LocalScale;
        }
    }
}