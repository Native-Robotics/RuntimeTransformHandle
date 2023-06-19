using UnityEngine;

namespace NativeRobotics.RuntimeTransformHandle.Samples
{
    /**
     * Created by Peter @sHTiF Stefcek 21.10.2020
     */
    public class RuntimeTransformHandleController : MonoBehaviour
    {
        [SerializeField] private RuntimeTransformHandle handle;

        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;

            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit)) return;
            var clickedObject = hit.collider.gameObject;

            if (clickedObject.TryGetComponent<ITransformHandleTargetPosition>(out var target))
                Select(target);
        }

        public void Select(ITransformHandleTargetPosition p_targetPosition)
        {
            handle.TargetPosition = p_targetPosition;
            handle.gameObject.SetActive(true);
        }

        public void Deselect()
        {
            handle.gameObject.SetActive(false);
        }
    }
}