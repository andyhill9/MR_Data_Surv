using u2vis.Input;
using UnityEngine;

namespace u2vis
{
    public class LinkGroupInteraction : UiElement
    {
        [SerializeField]
        protected LinkGroup _linkGroup = null;
        [SerializeField]
        protected float _doubleTapIntervall = 0.5f;
        
        protected float _doubleTapTimer = -1;

        protected void Update()
        {
            if (_doubleTapTimer > 0)
            {
                _doubleTapTimer -= Time.deltaTime;
                if (_doubleTapTimer <= 0)
                    _doubleTapTimer = -1;
            }
        }

        protected virtual void ClearLinks()
        {
            if (_linkGroup != null)
                _linkGroup.Clear();
        }
    }
}
