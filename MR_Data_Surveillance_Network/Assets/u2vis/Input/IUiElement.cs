
using UnityEngine;

namespace u2vis.Input
{
    public interface IUiElement
    {

        void OnFingerPinch(float value);
        void OnMouseBtnDown(int button, int order, RaycastHit hit);
        void OnMouseMove(int button, int order, RaycastHit hit);
        void OnMouseBtnUp(int button, int order, RaycastHit hit);
    }
}
