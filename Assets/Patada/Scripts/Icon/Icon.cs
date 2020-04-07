using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Icon {
    [SerializeField] private Sprite iconSmall = null;
    [SerializeField] private Sprite iconMedium = null;
    [SerializeField] private Sprite iconBig = null;
    public enum Size { Small, Medium, Big };

    public Sprite GetIcon(Icon.Size size = Icon.Size.Medium) {
        if (size == Icon.Size.Small) {
            return iconSmall;
        } else if (size == Icon.Size.Medium) {
            return iconMedium;
        } else if (size == Icon.Size.Big) {
            return iconBig;
        }

        throw new UnityException("Icon size " + size + " doesn't exist");
    }

    public Sprite this[Size size] {
        get { 
            if (size == Icon.Size.Small) {
            return iconSmall;
            } else if (size == Icon.Size.Medium) {
                return iconMedium;
            } else if (size == Icon.Size.Big) {
                return iconBig;
            }

            throw new UnityException("Icon size " + size + " doesn't exist");
        }
    }
}