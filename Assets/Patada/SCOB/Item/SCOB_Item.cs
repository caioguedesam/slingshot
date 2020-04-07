using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SCOB_Item : SCOB_TextVariable {

    [Space]
    [SerializeField] protected int id = 0;
    public int ID {
        get {
            return id;
        }
    }
    
    [SerializeField] protected string shownName = "";
    public string Name {
        get {
            return shownName;
        }
    }

    [Space]
    [SerializeField] protected Icon icons;
    public Sprite GetIcon(Icon.Size size) {
        return icons[size];
    }

    

    public override string StringValue(string key) {
        if (key.ToLower().Equals("name")) {
            return shownName;
        } else {
            throw new UnityException("Key " + key + " not implemented");
        }
    }
}
