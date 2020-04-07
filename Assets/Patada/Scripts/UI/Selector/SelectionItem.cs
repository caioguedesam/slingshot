using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Patada.UI {
    public class SelectionItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        [SerializeField] private Selector selector = null;
        private bool selected = false;

        public virtual void Selected() {
            selected = true;
        }

        public virtual void Deselected() {
            selected = false;
        }

        public void OnPointerUp(PointerEventData eventData) {
            if(!selected) {
                selector.SelectItem(this);
            } else {
                selector.DeselectItem(this);
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            
        }
    }
}

