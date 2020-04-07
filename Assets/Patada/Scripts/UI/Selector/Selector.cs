using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Patada.UI {
    public class Selector : MonoBehaviour {
        [SerializeField] private List<SelectionItem> selectedItems = null;
        [SerializeField] private bool mandatory = true;

        private System.Action<bool> onSelected = null;

        public bool IsSelected {
            get {
                if (mandatory) {
                    for (int i = 0; i < selectedItems.Count; i++) {
                        if (selectedItems[i] == null) {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        public List<string> SelectedItems {
            get {
                var items = new List<string>();
                for (int i = 0; i < selectedItems.Count; i++) {
                    if (selectedItems[i] != null) {
                        items.Add(selectedItems[i].name);
                    }
                }

                return items;
            }
        }

        public void SelectItem(SelectionItem item) {
            bool inserted = false;
            bool isSelectedBkp = IsSelected;

            for (int i = 0; i < selectedItems.Count && !inserted; i++) {
                if (selectedItems[i] == null) {
                    selectedItems[i] = item;
                    item.Selected();
                    inserted = true;

                } else if (i == selectedItems.Count - 1) {
                    selectedItems[i].Deselected();
                    selectedItems[i] = item;
                    item.Selected();
                    inserted = true;
                }
            }

            if(!isSelectedBkp && IsSelected && onSelected != null) {
                onSelected(true);
            }
        }

        public void DeselectItem(SelectionItem item) {
            bool removed = false;
            bool isSelectedBkp = IsSelected;

            for (int i = 0; i < selectedItems.Count && !removed; i++) {
                if (item.Equals(selectedItems[i])) {
                    selectedItems[i].Deselected();
                    selectedItems[i] = null;
                    removed = true;
                }
            }

            if(isSelectedBkp && !IsSelected && onSelected != null) {
                onSelected(false);
            }
        }

        public void RegisterOnSelectedCompletedListener(System.Action<bool> callback) {
            onSelected += callback;
        }

        public void RemoveOnSelectedCompletedListener(System.Action<bool> callback) {
            onSelected -= callback;
        }
    }
}