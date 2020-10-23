using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

    [SerializeField]
    public string ghostName;
    
    [SerializeField]
    private SpriteRenderer bodySprite;

    [SerializeField]
    private Color bodyColour;

    private bool killable = false;

    private Coroutine scaredRoutine;

    private void Start() {
        GameManager.Instance.ghostManager.RegisterGhost(this);
        bodySprite.color = bodyColour;
        
        // Set ghost name if not set
        if(ghostName == "") {
            ghostName = gameObject.name;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    /// <summary>
    /// Sets a ghosts scared status to the given value.
    /// </summary>
    /// <param name="value">Should the ghost be scared or not.</param>
    /// <param name="duration">The amount of time the ghost should be scared. -1 for indefinite.</param>
    /// <param name="frequency">How quickly the ghost should flash.</param>
    public void SetScared(bool value, float duration = -1, float frequency = 0.5f) {
        Debug.Log("Setting ghost: " + ghostName + " scared state to: " + value.ToString(), this);
        killable = value;

        // Set flash
        if(value) {
            scaredRoutine = StartCoroutine(VulnerableFlash(duration, frequency));
        } else {
            if(scaredRoutine != null) {
                StopCoroutine(scaredRoutine);
            }
        }
    }

    private IEnumerator VulnerableFlash(float duration, float frequency) {
        while (duration > 0 || duration == -1) {
            bodySprite.color = Color.blue;
            yield return new WaitForSeconds(frequency/2);
            bodySprite.color = Color.white;
            yield return new WaitForSeconds(frequency/2);
            yield return null;

            // Count down
            if(duration > 0) {
                duration -= Time.deltaTime + frequency;
            }
        }

        // Reset ghost
        bodySprite.color = bodyColour;
        SetScared(false);
    }
}
