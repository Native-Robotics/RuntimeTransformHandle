using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shtif.RuntimeTransformHandle
{
    /**
     * Created by Peter @sHTiF Stefcek 20.10.2020
     */
    public class ScaleHandle : MonoBehaviour
    {
        protected RuntimeTransformHandle _parentTransformHandle;
        protected List<ScaleAxis> _axes;
        protected ScaleGlobal _globalAxis;
        
        public ScaleHandle Initialize(RuntimeTransformHandle p_parentTransformHandle)
        {
            _parentTransformHandle = p_parentTransformHandle;
            transform.SetParent(_parentTransformHandle.transform, false);

            _axes = new List<ScaleAxis>();
            
            if (_parentTransformHandle.Axes == HandleAxes.X || _parentTransformHandle.Axes == HandleAxes.XY || _parentTransformHandle.Axes == HandleAxes.XZ || _parentTransformHandle.Axes == HandleAxes.XYZ)
                _axes.Add(CreateGameObject().AddComponent<ScaleAxis>()
                    .Initialize(_parentTransformHandle, Vector3.right, Color.red));
            
            if (_parentTransformHandle.Axes == HandleAxes.Y || _parentTransformHandle.Axes == HandleAxes.XY || _parentTransformHandle.Axes == HandleAxes.YZ || _parentTransformHandle.Axes == HandleAxes.XYZ)
                _axes.Add(CreateGameObject().AddComponent<ScaleAxis>()
                    .Initialize(_parentTransformHandle, Vector3.up, Color.green));

            if (_parentTransformHandle.Axes == HandleAxes.Z || _parentTransformHandle.Axes == HandleAxes.XZ || _parentTransformHandle.Axes == HandleAxes.YZ || _parentTransformHandle.Axes == HandleAxes.XYZ)
                _axes.Add(CreateGameObject().AddComponent<ScaleAxis>()
                    .Initialize(_parentTransformHandle, Vector3.forward, Color.blue));

            if (_parentTransformHandle.Axes != HandleAxes.X && _parentTransformHandle.Axes != HandleAxes.Y && _parentTransformHandle.Axes != HandleAxes.Z)
            {
                _globalAxis = CreateGameObject().AddComponent<ScaleGlobal>()
                    .Initialize(_parentTransformHandle, HandleBase.GetVectorFromAxes(_parentTransformHandle.Axes), Color.white);
                
                _globalAxis.InteractionStart += OnGlobalInteractionStart;
                _globalAxis.InteractionUpdate += OnGlobalInteractionUpdate;
                _globalAxis.InteractionEnd += OnGlobalInteractionEnd;
            }

            return this;
        }
        
        private GameObject CreateGameObject() => _parentTransformHandle.CreateGameObject();
        
        private void OnGlobalInteractionStart()
        {
            foreach (ScaleAxis axis in _axes)
            {
                axis.SetColor(Color.yellow);
            }
        }

        private void OnGlobalInteractionUpdate(float p_delta)
        {
            foreach (ScaleAxis axis in _axes)
            {
                axis.delta = p_delta;
            }
        }

        private void OnGlobalInteractionEnd()
        {
            foreach (ScaleAxis axis in _axes)
            {
                axis.SetDefaultColor();
                axis.delta = 0;
            }
        }

        public void Destroy()
        {
            foreach (ScaleAxis axis in _axes)
                Destroy(axis.gameObject);

            if (_globalAxis) Destroy(_globalAxis.gameObject);
            
            Destroy(this);
        }
    }
}