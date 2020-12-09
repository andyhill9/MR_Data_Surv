using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace u2vis.Input
{
    public class ButtonEx : UiElement
    {
        [SerializeField]
        private UnityEvent Clicked = null;

        public override void OnMouseBtnUp(int button)
        {
            if (Clicked != null)
                Clicked.Invoke();
        }

    }
}
