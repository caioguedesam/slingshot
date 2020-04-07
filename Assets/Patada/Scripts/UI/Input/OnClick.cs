using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Patada.UI {
    public class OnClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
        [SerializeField] private UnityEvent events = null;

        public void OnPointerDown(PointerEventData eventData) {

        }

        public void OnPointerUp(PointerEventData eventData) {
            if (!eventData.dragging) {
                Debug.Log("clicked :" + this.name);
                events.Invoke();
            }
        }
    }
}