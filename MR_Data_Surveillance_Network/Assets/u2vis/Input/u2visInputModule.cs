using System.Collections.Generic;
using UnityEngine;

namespace u2vis.Input
{
    public sealed class u2visInputModule : MonoBehaviour
    {

        [SerializeField]
        private bool _simulateMouseWithTouches = false;



        private void Start()
        {
            UnityEngine.Input.simulateMouseWithTouches = _simulateMouseWithTouches;
        }

        private void Update()
        {
            for (int i = 0; i < 3; i++)
            {
                if (UnityEngine.Input.GetMouseButtonDown(i))
                    OnMouseDown(i);
                if (UnityEngine.Input.GetMouseButton(i))
                    OnMouseMove(i);
                if (UnityEngine.Input.GetMouseButtonUp(i))
                    OnMouseUp(i);
            }


        }


        private void OnMouseDown(int button)
        {
            if (GetUiElements(UnityEngine.Input.mousePosition, out List<UiElemHitResult> results))
                for (int i = 0; i < results.Count; i++)
                {
                    var res = results[i];
                    res.UiElement.OnMouseBtnDown(button, i, res.HitResult);
                }
        }

        private void OnMouseMove(int button)
        {
            if (GetUiElements(UnityEngine.Input.mousePosition, out List<UiElemHitResult> results))
                for (int i = 0; i < results.Count; i++)
                {
                    var res = results[i];
                    res.UiElement.OnMouseMove(button, i, res.HitResult);
                }
        }

        private void OnMouseUp(int button)
        {
            if (GetUiElements(UnityEngine.Input.mousePosition, out List<UiElemHitResult> results))
                for (int i = 0; i < results.Count; i++)
                {
                    var res = results[i];
                    res.UiElement.OnMouseBtnUp(button, i, res.HitResult);
                }
        }

        private bool GetUiElement(Vector3 screenPosition, out IUiElement uiElement, out RaycastHit hit)
        {
            uiElement = null;
            var ray = Camera.main.ScreenPointToRay(screenPosition);
            if (!Physics.Raycast(ray.origin, ray.direction, out hit))
                return false;
            uiElement = hit.transform.GetComponent<IUiElement>();
            if (uiElement == null)
                return false;
            return true;
        }

        private bool GetUiElements(Vector3 screenPosition, out List<UiElemHitResult> uiElements)
        {
            uiElements = new List<UiElemHitResult>();
            var ray = Camera.main.ScreenPointToRay(screenPosition);
            var hits = Physics.RaycastAll(ray);
            foreach (var hit in hits)
            {
                var uiElem = hit.transform.GetComponent<IUiElement>();
                if (uiElem != null)
                    uiElements.Add(new UiElemHitResult(uiElem, hit));
            }
            if (uiElements.Count == 0)
                return false;
            uiElements.Sort(UiElemHitResult.Compare);
            return true;
        }

        private class UiElemHitResult
        {
            public readonly IUiElement UiElement;
            public readonly RaycastHit HitResult;

            public UiElemHitResult(IUiElement uiElement, RaycastHit hitResult)
            {
                UiElement = uiElement;
                HitResult = hitResult;
            }

            public static int Compare(UiElemHitResult x, UiElemHitResult y)
            {
                return Comparer<float>.Default.Compare(x.HitResult.distance, y.HitResult.distance);
            }
        }
    }
}