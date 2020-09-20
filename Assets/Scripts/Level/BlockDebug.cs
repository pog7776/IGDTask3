using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockDebug : MonoBehaviour{

    public int x = 0;
    public int y = 0;

    private void OnMouseDown() {
        Debug.LogWarning("x: " + x + " y: " + y);
    }

    public void SetPos(Vector2Int pos) {
        x = pos.x;
        y = pos.y;
    }
}
