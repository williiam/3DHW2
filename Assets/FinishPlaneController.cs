using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishPlaneController : MonoBehaviour
{
    int newWinCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Player Win");
            if(PlayerPrefs.HasKey("winCount")) {
                newWinCount = PlayerPrefs.GetInt("winCount");
            }
            newWinCount+=1;
            PlayerPrefs.SetInt("winCount", newWinCount);
            SceneManager.LoadScene(0);
        }
    }
}
