using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour {

    [SerializeField]
    private Animator animator;

    private bool isPowerPellet;

    // Start is called before the first frame update
    void Start() {
        if(isPowerPellet) {
            animator.SetBool("IsPower", true);
        }
    }

    public void ConvertToPowerPellet() {
        isPowerPellet = true;
        animator.SetBool("IsPower", true);
    }
}
