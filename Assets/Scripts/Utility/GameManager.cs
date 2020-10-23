using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField]
    public GhostManager ghostManager;

    [SerializeField]
    public GameUI gameUI;


    // Set instance
	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }
    private void SetSingleton() {
		if (_instance != null && _instance != this) {
			Destroy(this.gameObject);
		} else {
			_instance = this;
		}
	}

    private void Awake() {
        SetSingleton();
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
