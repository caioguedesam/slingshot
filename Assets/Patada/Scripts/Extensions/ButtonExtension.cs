using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonExtension : MonoBehaviour {
    private Button button = null;

    void Awake() {
        button = GetComponent<Button>();
    }

    public void Click() {
        button.onClick.Invoke();
    }
}
