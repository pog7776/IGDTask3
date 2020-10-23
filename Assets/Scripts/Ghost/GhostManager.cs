using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour {

    [SerializeField]
    private bool startScared;

    private List<Ghost> allGhosts;

    private void Awake() {
        allGhosts = new List<Ghost>();
    }

    // Start is called before the first frame update
    void Start() {
        StartCoroutine(WaitForSetup());
    }

    private void AfterStart() {
        // Init all ghosts
        foreach(Ghost ghost in allGhosts) {
            ghost.SetScared(startScared);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    private IEnumerator WaitForSetup() {
        yield return new WaitForEndOfFrame();
        AfterStart();
    }

    public void RegisterGhost(Ghost ghost) {
        Debug.Log("Registering ghost: " + ghost.ghostName, ghost);
        allGhosts.Add(ghost);
    }
}
