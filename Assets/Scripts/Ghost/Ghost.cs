using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {
    
    [SerializeField]
    private SpriteRenderer bodySprite;

    [SerializeField]
    private bool useCustomColour;

    [SerializeField]
    private Color bodyColour = Color.blue;


    // Update is called once per frame
    void Update() {
        if(useCustomColour) {
            bodySprite.color = bodyColour;
        }
    }
}
