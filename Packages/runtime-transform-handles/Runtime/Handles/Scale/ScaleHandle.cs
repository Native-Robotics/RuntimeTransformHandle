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
        private RuntimeTransformHandle _parentTransformHandle;
        private List<ScaleAxis> _axes;
        private ScaleGlobal _globalAxis;
        
        public ScaleHandle Construct(Camera cam, RuntimeTransformHandle parentTransformHandle)
        {
            _parentTransformHandle = parentTransformHandle;
            transform.SetParent(_parentTransformHandle.transform, false);

            _axes = new List<ScaleAxis>();
            
            if (_parentTransformHandle.Axes is HandleAxes.X or HandleAxes.XY or HandleAxes.XZ or HandleAxes.XYZ)
                _axes.Add(CreateScaleAxis()
                    .Construct(cam, _parentTransformHandle, Vector3.right, Color.red));
            
            if (_parentTransformHandle.Axes is HandleAxes.Y or HandleAxes.XY or HandleAxes.YZ or HandleAxes.XYZ)
                _axes.Add(CreateScaleAxis()
                    .Construct(cam, _parentTransformHandle, Vector3.up, Color.green));

            if (_parentTransformHandle.Axes is HandleAxes.Z or HandleAxes.XZ or HandleAxes.YZ or HandleAxes.XYZ)
                _axes.Add(CreateScaleAxis()
                    .Construct(cam, _parentTransformHandle, Vector3.forward, Color.blue));

            if (_parentTransformHandle.Axes != HandleAxes.X && _parentTransformHandle.Axes != HandleAxes.Y && _parentTransformHandle.Axes != HandleAxes.Z)
            {
                _globalAxis = CreateGameObject().AddComponent<ScaleGlobal>()
                    .Construct(_parentTransformHandle, HandleBase.GetVectorFromAxes(_parentTransformHandle.Axes), Color.white);
                
                _globalAxis.InteractionStart += OnGlobalInteractionStart;
                _globalAxis.InteractionUpdate += OnGlobalInteractionUpdate;
                _globalAxis.InteractionEnd += OnGlobalInteractionEnd;
            }

            return this;
        }

        private ScaleAxis CreateScaleAxis() => CreateGameObject().AddComponent<ScaleAxis>();

        private GameObject CreateGameObject() => _parentTransformHandle.CreateGameObject();
        
        private void OnGlobalInteractionStart()
        {
            foreach (var axis in _axes)
            {
                axis.SetColor(Color.yellow);
            }
        }

        private void OnGlobalInteractionUpdate(float pDelta)
        {
            foreach (var axis in _axes)
            {
                axis.delta = pDelta;
            }
        }

        private void OnGlobalInteractionEnd()
        {
            foreach (var axis in _axes)
            {
                axis.SetDefaultColor();
                axis.delta = 0;
            }
        }

        public void Destroy()
        {
            foreach (var axis in _axes)
                Destroy(axis.gameObject);

            if (_globalAxis) Destroy(_globalAxis.gameObject);
            
            Destroy(this);
        }
    }
}