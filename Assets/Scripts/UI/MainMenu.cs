using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private Image background;

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(BGFade(1));
    }

    // Update is called once per frame
    void Update() {
        
    }

    private IEnumerator BGFade(float frequency) {
        while (true) {
            background.color = Color.Lerp(background.color, Color.red, frequency);
            yield return null;
        }
    }

    public void LoadLevel1() {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2() {
        SceneManager.LoadScene(2);
    }
}
