using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateTextLabel : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI label = null;
    [SerializeField] private SCOB_TextVariable variable = null;
    [SerializeField] private string key = "";

    public void Update () {
        label.text = variable.StringValue(key);
    }
}
