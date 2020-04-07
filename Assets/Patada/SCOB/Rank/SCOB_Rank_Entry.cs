using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SCOBI_Rank_Entry", menuName = "Patada/Rank/Entry", order = 1)]
public class SCOB_Rank_Entry : ScriptableObject {
    [SerializeField] private string rankName = "";
    public string Name{
        get{
            return rankName;
        }
    }

    [SerializeField] private int level = 0;
    public int Level {
        get{
            return level;
        }
    }

    [SerializeField] private int victoriesNeeded = 0;
    public int VictoriesNeeded {
        get{
            return victoriesNeeded;
        }
    }
    
    [Space]
    [SerializeField] private Icon icon = new Icon();
    public Icon Icons {
        get{
            return icon;
        }
    }
}
