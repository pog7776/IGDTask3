using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

    [SerializeField]
    private Image background;

    [SerializeField]
    private Gradient backgroundRange;
    [SerializeField]
    private float speed = 1;

    // Start is called before the first frame update
    void Start() {
        //StartCoroutine(BGFade(0.01f));
    }

    // Update is called once per frame
    void Update() {
        background.color = Color.Lerp(backgroundRange.Evaluate(0), backgroundRange.Evaluate(1), Mathf.PingPong(Time.time, speed));// * speed);
    }

    // LMAO this sucked
    private IEnumerator BGFade(float speed) {
        bool colour = false;
        while(true) {
            while (colour == true) {
                background.color = Color.Lerp(background.color, backgroundRange.Evaluate(1), speed);
                Debug.Log("red " + background.color);
                if(background.color == backgroundRange.Evaluate(1)) {
                    colour = false;
                }
                yield return null;
            }
            while (colour == false) {
                background.color = Color.Lerp(background.color, backgroundRange.Evaluate(0), speed);
                Debug.Log("black " + background.color);
                if(background.color == backgroundRange.Evaluate(0)) {
                    colour = true;
                }
                yield return null;
            }
        }
    }

    public void LoadLevel1() {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel2() {
        SceneManager.LoadScene(2);
    }
}
