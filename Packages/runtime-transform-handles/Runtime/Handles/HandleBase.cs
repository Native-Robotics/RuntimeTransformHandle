using System;
using System.IO;
using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public abstract class HandleBase : MonoBehaviour
    {
        public event Action InteractionStart;
        public event Action InteractionEnd;
        public event Action<float> InteractionUpdate;
        
        protected RuntimeTransformHandle ParentTransformHandle;
        protected Color DefaultColor;
        protected Material Material;
        protected Vector3 HitPoint;

        public float delta;

        protected virtual void InitializeMaterial()
        {
            Material = new Material(Shader.Find("sHTiF/HandleShader"));
            Material.color = DefaultColor;
        }
        
        public void SetDefaultColor()
        {
            Material.color = DefaultColor;
        }
        
        public void SetColor(Color pColor)
        {
            Material.color = pColor;
        }
        
        public virtual void StartInteraction(Vector3 hitPoint)
        {
            HitPoint = hitPoint;
            InteractionStart?.Invoke();
        }

        public virtual bool CanInteract(Vector3 hitPoint)
        {
            return true;
        }
        
        public virtual void Interact(Vector3 previousPosition)
        {
            InteractionUpdate?.Invoke(delta);
        }

        public virtual void EndInteraction()
        {
            InteractionEnd?.Invoke();
            delta = 0;
            SetDefaultColor();
        }

        public static Vector3 GetVectorFromAxes(HandleAxes axes)
        {
            switch (axes)
            {
                case HandleAxes.X:
                    return new Vector3(1,0,0);
                case HandleAxes.Y:
                    return new Vector3(0,1,0);
                case HandleAxes.Z:
                    return new Vector3(0,0,1);
                case HandleAxes.XY:
                    return new Vector3(1,1,0);
                case HandleAxes.XZ:
                    return new Vector3(1,0,1);
                case HandleAxes.YZ:
                    return new Vector3(0,1,1);
                default:
                    return new Vector3(1,1,1);
            }
        }
    }
}