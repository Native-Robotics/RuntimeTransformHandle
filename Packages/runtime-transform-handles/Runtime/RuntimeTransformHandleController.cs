// using System.Collections;
// using System.Collections.Generic;
// using NativeRobotics.InputHandling;
// using RuntimeHandle;
// using UnityEngine;
// using UnityEngine.Serialization;
// using Zenject;
//
//
// namespace RuntimeHandle
// {
//     /**
//      * Created by Peter @sHTiF Stefcek 21.10.2020
//      */
//     public class RuntimeTransformHandleController : MonoBehaviour, IInitializable
//     {
//         [SerializeField] private RuntimeTransformHandle handle;
//
//         public RuntimeTransformHandleController(IClickHandlerService clickHandlerService)
//         {
//             _clickHandlerService = clickHandlerService;
//         }
//
//         private Camera mainCamera;
//         private IClickHandlerService _clickHandlerService;
//
//         public void Initialize()
//         {
//             _clickHandlerService.Clicked += OnClicked;
//         }
//
//         private void OnClicked(GameObject obj)
//         {
//         
//         }
//
//         private void Start()
//         {
//             // Получаем ссылку на главную камеру
//             mainCamera = Camera.main;
//         }
//
//         // Update is called once per frame
//
//         void Update()
//         {
//             if (Input.GetMouseButtonDown(0))
//             {
//                 // Создаем луч из позиции нажатия мыши или экрана
//                 Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
//                 RaycastHit hit;
//
//                 // Проверяем столкновение луча с объектом
//                 if (Physics.Raycast(ray, out hit))
//                 {
//                     // Получаем ссылку на объект, с которым произошло столкновение
//                     GameObject clickedObject = hit.collider.gameObject;
//
//                     // Возвращаем объект
//                     if (clickedObject.TryGetComponent<ITransformHandleTarget>(out var target))
//                     {
//                         Select(target);
//                     }
//                 }
//             }
//         }
//
//         public void Select(ITransformHandleTarget target)
//         {
//             handle.target = target;
//             handle.gameObject.SetActive(true);
//         }
//
//         public void Deselect()
//         {
//             handle.gameObject.SetActive(false);
//         }
//     }
//
// }