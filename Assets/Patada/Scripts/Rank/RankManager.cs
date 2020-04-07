using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Patada.Events;

public class RankManager : MonoBehaviour {
    [SerializeField] private List<SCOB_Rank_Entry> defaultRanks = new List<SCOB_Rank_Entry>();
    [SerializeField] private SCOB_BaseEvent levelledUp = null;
    [SerializeField] private SCOB_BaseEvent sameLevel = null;
    private int currentRankLevel = -1;
    private int currentVictories = -1;
    
    public int Victories{
        get{
            return currentVictories;
        }
    }

    void Awake(){
        currentRankLevel = Save.Instance.GetInt("rank_level", 2);
        currentVictories = Save.Instance.GetInt("rank_victories", 0);
    }

    private void IncreaseRankLevel() {
        if(currentRankLevel == -1) {
            currentRankLevel = Save.Instance.GetInt("rank_level", 2);
        }

        var newRankLevel = Mathf.Min(defaultRanks.Count-1, currentRankLevel+1);

        if(currentRankLevel < newRankLevel) {
            currentRankLevel = newRankLevel;
            Save.Instance.SetInt("rank_level", newRankLevel);

            // if(FB.IsInitialized) {
            //     FB.LogAppEvent("Rank_" + currentRankLevel);
            // }

            levelledUp.Raise();
        } else {
            sameLevel.Raise();
        }
    }
    
    public void IncreaseRank() {
        currentVictories = Save.Instance.GetInt("rank_victories", 0);
        currentVictories++;

        if(currentRankLevel < defaultRanks.Count-1) {
            if(currentVictories >= defaultRanks[currentRankLevel+1].VictoriesNeeded) {
                currentVictories = 0;
                IncreaseRankLevel();
            } else {
                sameLevel.Raise();
            }
        } else {
            sameLevel.Raise();
        }

        Save.Instance.SetInt("rank_victories", currentVictories);
    }

    public SCOB_Rank_Entry GetRank(int rankLevel = -1) {
        if(rankLevel == -1) {
            if(currentRankLevel == -1) {
                currentRankLevel = Save.Instance.GetInt("rank_level", 2);
            }

            return defaultRanks[currentRankLevel];

        } else {
            return defaultRanks[rankLevel];
        }
    }

    public SCOB_Rank_Entry GetNextRank() {
        if(currentRankLevel == -1) {
            currentRankLevel = Save.Instance.GetInt("rank_level", 2);
        }

        return defaultRanks[Mathf.Min(currentRankLevel+1, defaultRanks.Count -1)];
    }
}
