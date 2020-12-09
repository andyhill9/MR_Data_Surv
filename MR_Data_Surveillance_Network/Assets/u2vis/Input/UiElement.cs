using System.Collections.Generic;
using UnityEngine;

namespace u2vis.Input
{
    public abstract class UiElement : MonoBehaviour, IUiElement
    {


        public virtual void OnFingerPinch(float value)
        {
        }

        public virtual void OnMouseBtnDown(int button)
        {
        }
        public virtual void OnMouseBtnDown(int button, int order, RaycastHit hit)
        {
            OnMouseBtnDown(button);
        }

        public virtual void OnMouseMove(int button)
        {
        }
        public virtual void OnMouseMove(int button, int order, RaycastHit hit)
        {
            OnMouseMove(button);
        }

        public virtual void OnMouseBtnUp(int button)
        {
        }
        public virtual void OnMouseBtnUp(int button, int order, RaycastHit hit)
        {
            OnMouseBtnUp(button);
        }
    }
}
