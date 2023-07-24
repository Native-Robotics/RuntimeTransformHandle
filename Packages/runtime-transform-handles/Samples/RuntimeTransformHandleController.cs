using UnityEngine;

namespace Shtif.RuntimeTransformHandle.Samples
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

            if (!Physics.Raycast(ray, out var hit))
            {
                Deselect();
                return;
            }

            var clickedObject = hit.collider.gameObject;

            var targetPositionExists =
                clickedObject.TryGetComponent<ITransformHandleTargetPosition>(out var targetPosition);
            if (targetPositionExists) handle.TargetPosition = targetPosition;

            var targetRotationExists =
                clickedObject.TryGetComponent<ITransformHandleTargetRotation>(out var targetRotation);
            if (targetRotationExists) handle.TargetRotation = targetRotation;

            var targetLocalRotationExists =
                clickedObject.TryGetComponent<ITransformHandleTargetLocalRotation>(out var targetLocalRotation);
            if (targetLocalRotationExists) handle.TargetLocalRotation = targetLocalRotation;

            var targetScaleExists =
                clickedObject.TryGetComponent<ITransformHandleTargetScale>(out var targetScale);
            if (targetScaleExists) handle.TargetScaleTarget = targetScale;

            if (targetPositionExists || targetRotationExists || targetLocalRotationExists || targetScaleExists)
                Select();
        }

        private void Select() => SetActiveHandle(true);

        private void Deselect() => SetActiveHandle(false);

        private void SetActiveHandle(bool pIsEnable)
        {
            handle.gameObject.SetActive(pIsEnable);
            handle.SetEnabled(pIsEnable);
        }
    }
}