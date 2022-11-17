using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Text = TMPro.TextMeshProUGUI;

public class WinCount : MonoBehaviour {
   public Text scoreText;
   int winCount = 0;

   void Start() {
        if(PlayerPrefs.HasKey("winCount")) {
           winCount = PlayerPrefs.GetInt("winCount");
        }
       scoreText = this.GetComponentInChildren<Text>();
       scoreText.text = $"WIN COUNT: {winCount.ToString()}";
   }

   void Update() {
        if(PlayerPrefs.HasKey("winCount")) {
           winCount = PlayerPrefs.GetInt("winCount");
        }
        scoreText.text = $"WIN COUNT: {winCount.ToString()}";
   }

}

