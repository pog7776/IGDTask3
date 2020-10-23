using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Animator animator;
    private List<GameObject> navPoints = new List<GameObject>();
    private float startTime;
    private int currentTarget = 0;

    private Vector3 startPos;
    private Vector3 endPos;
    private float duration = 1.5f;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if(navPoints.Count != 0 && startPos != null && endPos != null) {
            //MoveToPoint(startPos, endPos, duration);
        }
    }

    public void AddNavPoint(GameObject point) {
        navPoints.Add(point);
    }

    public void StartNavigation() {
        startTime = Time.time;
        startPos = navPoints[currentTarget].transform.position;
        currentTarget = currentTarget < navPoints.Count-1 ? currentTarget+1 : 0;
        endPos = navPoints[currentTarget].transform.position;
        ChangeDirection();
        StartSound();
    }

    private void MoveToPoint(Vector3 startPos, Vector3 endPos, float duration) {
        if(Vector3.Distance(gameObject.transform.localPosition, endPos) > 0.1f) {
            float timeFraction = (Time.time - startTime) / duration;

            gameObject.transform.localPosition = Vector3.Lerp(startPos, endPos, timeFraction);
        } else {
            gameObject.transform.localPosition = endPos;
            //StartNavigation();
        }
    }

    private void ChangeDirection() {
        switch (currentTarget) {
            case 0:
                animator.SetTrigger("Up");
                gameObject.transform.rotation = Quaternion.Euler(0,0,0);
            break;
            case 1:
                animator.SetTrigger("Horizontal");
                gameObject.transform.rotation = Quaternion.Euler(0,0,0);
            break;
            case 2:
                animator.SetTrigger("Down");
                gameObject.transform.rotation = Quaternion.Euler(0,0,0);
            break;
            case 3:
                animator.SetTrigger("Horizontal");
                gameObject.transform.rotation = Quaternion.Euler(0,180,0);
            break;
            default:
            break;
        }
    }

    private void StartSound() {
        
    }
}
