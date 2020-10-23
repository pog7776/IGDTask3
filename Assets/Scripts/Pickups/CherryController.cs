using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CherryController : MonoBehaviour {

    [SerializeField]
    private GameObject cherryPrefab;
    private Coroutine timer;

    private GameObject cherry;
    private float startTime;
    private Vector3 startPos;
    private Vector3 endPos;
    private float duration = 5f;
    private bool isNavigating = false;

    // Start is called before the first frame update
    void Start() {
        timer = StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update() {
        if(isNavigating) {
            MoveToPoint(startPos, endPos, duration);
        }
    }

    private IEnumerator CountDown() {
        yield return new WaitForSeconds(30);
        SpawnCherry();
        // Loop
        timer = StartCoroutine(CountDown());
    }

    private void SpawnCherry() {
        cherry = Instantiate<GameObject>(cherryPrefab, new Vector3(-15, -16, 0), Quaternion.identity);
        cherry.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        startTime = Time.time;
        startPos = cherry.transform.localPosition;
        endPos = new Vector3(45*4, -16, 0);
        isNavigating = true;
        MoveToPoint(startPos, endPos, duration);
    }

    private void MoveToPoint(Vector3 startPos, Vector3 endPos, float duration) {
        if(Vector3.Distance(cherry.transform.localPosition, endPos) > 0.1f) {
            float timeFraction = (Time.time - startTime) / duration;

            cherry.transform.localPosition = Vector3.Lerp(startPos, endPos, timeFraction);
        } else {
            cherry.transform.localPosition = endPos;
            isNavigating = false;
            Destroy(cherry);
        }
    }
}
