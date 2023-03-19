using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClickerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI RacePointtext;

    public void UpdateUI(int amount)
    {
        RacePointtext.text = $"Race Point: {amount}";
    }
}
