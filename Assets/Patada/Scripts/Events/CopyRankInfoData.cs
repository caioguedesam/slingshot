using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CopyRankInfoData : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI rankNameLabel = null;
    [SerializeField] private Image rankIcon = null;
    [SerializeField] private Icon.Size iconSize = Icon.Size.Medium;
    [SerializeField] private TextMeshProUGUI rankProgressionLabel = null;
    [SerializeField] private Image rankProgression = null;

    public void Copy() {
        var rankManager = FindObjectOfType<RankManager>();
        var currentRank = rankManager.GetRank();
        var nextRank = rankManager.GetNextRank();

        if(rankNameLabel != null) {
            rankNameLabel.text = currentRank.Name;
        }
        if(rankIcon != null) {
            rankIcon.sprite = currentRank.Icons[iconSize];
        }

        var percentage = (float)System.Math.Round(Save.Instance.GetInt("rank_victories", 0) / (float)nextRank.VictoriesNeeded, 2);
        if(rankProgressionLabel != null) {
            rankProgressionLabel.text = (percentage * 100f).ToString();
        }
        if(rankProgression != null) {
            rankProgression.fillAmount = percentage;
        }
    }
}
