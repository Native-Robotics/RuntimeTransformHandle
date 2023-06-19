using System;
using UnityEngine;

namespace NativeRobotics.RuntimeTransformHandle
{
    public class RuntimeTransformHandle : MonoBehaviour
    {
        public HandleSnappingType snappingType = HandleSnappingType.RELATIVE;

        public Vector3 positionSnap = Vector3.zero;
        public float rotationSnap = 0;
        public Vector3 scaleSnap = Vector3.zero;
        [SerializeField] private HandleAxes axes = HandleAxes.XYZ;
        [SerializeField] private HandleSpace space = HandleSpace.LOCAL;
        [SerializeField] private HandleType type = HandleType.POSITION;
        
        public bool autoScale = false;
        public float autoScaleFactor = 1;
        public Camera handleCamera;

        private Vector3 _previousMousePosition;
        private HandleBase _previousAxis;

        private HandleBase _draggingHandle;

        private PositionHandle _positionHandle;
        private RotationHandle _rotationHandle;
        private ScaleHandle _scaleHandle;

        public HandleAxes Axes
        {
            get => axes;
            private set
            {
                axes = value;
                Recreate();
            }
        }

        public HandleSpace Space
        {
            get => space;
            private set
            {
                space = value;
                Recreate();
            }
        }
        
        public HandleType Type
        {
            get => type;
            private set
            {
                type = value;
                Recreate();
            }
        }

        public bool IsEnabled { get; set; }
        public ITransformHandleTargetLocalRotation TargetLocalRotation { get; set; }
        public ITransformHandleTargetScale TargetScaleTarget { get; set; }
        public ITransformHandleTargetRotation TargetRotation { get; set; }
        public ITransformHandleTargetPosition TargetPosition { get; set; }
        
        private void Start()
        {
            if (handleCamera == null)
                handleCamera = Camera.main;

            CreateHandles();
        }

        private void CreateHandles()
        {
            switch (Type)
            {
                case HandleType.POSITION:
                    _positionHandle = gameObject.AddComponent<PositionHandle>().Initialize(this);
                    break;
                case HandleType.ROTATION:
                    _rotationHandle = gameObject.AddComponent<RotationHandle>().Initialize(this);
                    break;
                case HandleType.SCALE:
                    _scaleHandle = gameObject.AddComponent<ScaleHandle>().Initialize(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Clear()
        {
            _draggingHandle = null;

            if (_positionHandle) _positionHandle.Destroy();
            if (_rotationHandle) _rotationHandle.Destroy();
            if (_scaleHandle) _scaleHandle.Destroy();
        }

        private void Recreate()
        {
            Clear();
            CreateHandles();
        }
        
        private void Update()
        {
            if (!IsEnabled)
                return;

            if (autoScale)
                transform.localScale =
                    Vector3.one * (Vector3.Distance(handleCamera.transform.position, transform.position) *
                                   autoScaleFactor) / 15;

            HandleBase handle = null;
            Vector3 hitPoint = Vector3.zero;
            GetHandle(ref handle, ref hitPoint);

            HandleOverEffect(handle, hitPoint);

            if (Input.GetMouseButton(0) && _draggingHandle != null)
            {
                _draggingHandle.Interact(_previousMousePosition);
            }

            if (Input.GetMouseButtonDown(0) && handle != null)
            {
                _draggingHandle = handle;
                _draggingHandle.StartInteraction(hitPoint);
            }

            if (Input.GetMouseButtonUp(0) && _draggingHandle != null)
            {
                _draggingHandle.EndInteraction();
                _draggingHandle = null;
            }

            _previousMousePosition = Input.mousePosition;

            transform.position = TargetPosition.Position;
            if (Space == HandleSpace.LOCAL || Type == HandleType.SCALE)
            {
                transform.rotation = TargetRotation.Rotation;
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
        }

        private void HandleOverEffect(HandleBase p_axis, Vector3 p_hitPoint)
        {
            if (_draggingHandle == null && _previousAxis != null &&
                (_previousAxis != p_axis || !_previousAxis.CanInteract(p_hitPoint)))
            {
                _previousAxis.SetDefaultColor();
            }

            if (p_axis != null && _draggingHandle == null && p_axis.CanInteract(p_hitPoint))
            {
                p_axis.SetColor(Color.yellow);
            }

            _previousAxis = p_axis;
        }

        private void GetHandle(ref HandleBase p_handle, ref Vector3 p_hitPoint)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            if (hits.Length == 0)
                return;

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject.TryGetComponentInParent(out p_handle))
                {
                    p_hitPoint = hit.point;
                    return;
                }
            }
        }
    }
}