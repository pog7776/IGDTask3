using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacStudentController : MonoBehaviour {

    private KeyCode lastInput;
    private KeyCode currentInput;

    private Vector3 startPos;
    private Vector3 endPos;
    private float startTime;
    private float duration = 0.5f;
    private float rayLength = 1;
    private bool isNavigating = false;

    private Animator animator;
    private AudioSource audio;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        // This is a weird block of code
        // I wish it worked lol
        // Why can't Unity just give me anykey that has been pressed? Then I can switch over it
        /*
        Event e = Event.current;
        if (e != null && e.isKey && (
            e.keyCode == KeyCode.W || 
            e.keyCode == KeyCode.A || 
            e.keyCode == KeyCode.S || 
            e.keyCode == KeyCode.D ))
        {
            lastInput = e.keyCode;
            Debug.Log("Last Input: " + lastInput.ToString());
        }
        */

        if(Input.GetKey(KeyCode.W)) {
            lastInput = KeyCode.W;
        }
        if(Input.GetKey(KeyCode.A)) {
            lastInput = KeyCode.A;
        }
        if(Input.GetKey(KeyCode.S)) {
            lastInput = KeyCode.S;
        }
        if(Input.GetKey(KeyCode.D)) {
            lastInput = KeyCode.D;
        }
        
        if(lastInput != default && !isNavigating) {
            SetNext();
        } else if(isNavigating) {
            MoveToPoint(startPos, endPos, duration);
        }
    }

    private void SetNext() {
        // Init current input
        if(currentInput == default) {
            currentInput = lastInput;
        }

        Vector2 direction = Vector2.zero;

        switch (lastInput) {
            case KeyCode.W:
                direction = Vector2.up;
                animator.SetTrigger("Up");
                gameObject.transform.rotation = Quaternion.Euler(0,0,0);
            break;
            case KeyCode.A:
                direction = Vector2.left;
                animator.SetTrigger("Horizontal");
                gameObject.transform.rotation = Quaternion.Euler(0,180,0);
            break;
            case KeyCode.S:
                direction = Vector2.down;
                animator.SetTrigger("Down");
                gameObject.transform.rotation = Quaternion.Euler(0,0,0);
            break;
            case KeyCode.D:
                direction = Vector2.right;
                animator.SetTrigger("Horizontal");
                gameObject.transform.rotation = Quaternion.Euler(0,0,0);
            break;
            default:
                direction = Vector2.zero;
                Debug.LogWarning("shouldn't be here dumbass");
            break;
        }

        // Raycast and get close pellet
        ContactFilter2D contactFilter = new ContactFilter2D();
        RaycastHit2D[] results = new RaycastHit2D[2];
        Physics2D.Raycast(transform.localPosition, direction, contactFilter, results);

        // Get second result
        RaycastHit2D hit = results[1];

        //Debug.DrawLine(transform.localPosition, hit.point, Color.red, 5);

        if(hit.transform != null) {
            Debug.Log("Hit: " + hit.transform.name, hit.transform.gameObject);
        } else {
            Debug.Log("Didn't hit anything.");
        }
        
        if(hit.transform != null && hit.transform.gameObject.tag == "Pellet") {
            Debug.Log("Navigating to: " + hit.transform.name, hit.transform.gameObject);
            currentInput = lastInput;
            StartNavigation(hit.transform.position);
        } else {
            lastInput = currentInput;
            audio.Pause();
        }
    }

    public void StartNavigation(Vector2 newEndPos) {
        startTime = Time.time;
        startPos = transform.position;
        endPos = newEndPos;
        isNavigating = true;

        if(endPos != null) {
            MoveToPoint(startPos, endPos, duration);
        }
    }

    private void MoveToPoint(Vector3 startPos, Vector3 endPos, float duration) {
        if(!audio.isPlaying) {
            audio.Play();
        }

        if(Vector3.Distance(gameObject.transform.localPosition, endPos) > 0.1f) {
            float timeFraction = (Time.time - startTime) / duration;

            gameObject.transform.localPosition = Vector3.Lerp(startPos, endPos, timeFraction);
        } else {
            gameObject.transform.localPosition = endPos;
            isNavigating = false;
            SetNext();
        }
    }
}
