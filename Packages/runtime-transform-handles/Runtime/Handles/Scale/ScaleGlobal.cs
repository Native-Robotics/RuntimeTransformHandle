using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    public class ScaleGlobal : HandleBase
    {
        private Vector3 _axis;
        private Vector3 _startScale;
        
        public ScaleGlobal Construct(RuntimeTransformHandle parentTransformHandle, Vector3 axis, Color color)
        {
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
            mf.mesh = MeshUtils.CreateBox(.35f, .35f, .35f);
            o.AddComponent<MeshCollider>();

            return this;
        }

        public override void Interact(Vector3 previousPosition)
        {
            var mouseVector = (Input.mousePosition - previousPosition);
            var d = (mouseVector.x + mouseVector.y) * Time.deltaTime * 2;
            delta += d;
            ParentTransformHandle.TargetScaleTarget.LocalScale = _startScale + Vector3.Scale(_startScale,_axis) * delta;
            
            base.Interact(previousPosition);
        }

        public override void StartInteraction(Vector3 hitPoint)
        {
            base.StartInteraction(hitPoint);
            _startScale = ParentTransformHandle.TargetScaleTarget.LocalScale;
        }
    }
}