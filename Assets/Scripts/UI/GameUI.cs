using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour {

    [SerializeField]
    private TextMeshProUGUI score;

    [SerializeField]
    private TextMeshProUGUI time;

    [SerializeField]
    private TextMeshProUGUI scaredTimer;

    // Start is called before the first frame update
    void Start() {
        score.text = "000";
        time.text = "00:00:00";
        scaredTimer.alpha = 0;
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ExitGame() {
        SceneManager.LoadScene(0);
    }

}
