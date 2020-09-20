using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    private GameObject wallBlockPrefab;

    [SerializeField]
    private Transform levelParent;

    [SerializeField]
    private GameObject pellet;

    [SerializeField]
    private Sprite outWall;
    [SerializeField]
    private Sprite outCorner;
    [SerializeField]
    private Sprite inWall;
    [SerializeField]
    private Sprite inCorner;
    [SerializeField]
    private Sprite tJunction;
    [SerializeField]
    private Sprite square;
    [SerializeField]
    private bool useSquare;

    private List<SpriteRenderer> wallSprites;

    int[,] levelMap = {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}
    };

    int[,] levelMapClosed = {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,4},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0}
    };

    // Start is called before the first frame update
    void Start() {
        wallSprites = new List<SpriteRenderer>();

        if(useSquare) {
            outWall = square;
            outCorner = square;
            inCorner = square;
            inWall = square;
            tJunction = square;
        }

        for (int i = 1; i < 5; i++) {
            GenMap(i);
        }

        // for (int x = 0; x < allElements.GetLength(0); x++) {
        //     string line = "";
        //     for (int y = 0; y < allElements.GetLength(1); y++) {
        //         line += allElements[x,y] + " ";
        //     }
        //     debug.text += line + "\n";
        // }
    }

    void Update() {
        // Color newColour = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f), 1);
        // foreach(SpriteRenderer sprite in wallSprites) {
        //     sprite.color = newColour;
        // }
    }

    private void GenMap(int quarter) {
        GameObject parent = new GameObject("Quarter " + quarter);
        switch (quarter) {
            case 1:
                // Top Left
                for (int x = 0; x < levelMap.GetLength(0); x++) {
                    for (int y = 0; y < levelMap.GetLength(1); y++) {
                        Vector3 position = new Vector3(y, -x, 0);
                        PlaceBlock(x, y, position, parent.transform, levelMap);

                        //allElements[x,y] = levelMap[x,y];
                    }
                }
            break;
            case 2:
            // Top Right
                for (int x = levelMap.GetLength(0)-1; x >= 0; x--) {
                    for (int y = levelMap.GetLength(1)-1; y >= 0; y--) {
                        Vector3 position = new Vector3((levelMap.GetLength(1) + (levelMap.GetLength(1) - y))-1, -x, 0);
                        PlaceBlock(x, y, position, parent.transform, levelMap);
                        
                        //allElements[levelMap.GetLength(0)+(levelMap.GetLength(0)-x),y] = levelMap[x,y];
                    }
                }
            break;
            case 3:
            // Bottom Left
                for (int x = 0; x < levelMap.GetLength(0); x++) {
                    for (int y = levelMap.GetLength(1)-1; y >= 0; y--) {
                        Vector3 position = new Vector3(y, -levelMap.GetLength(1) + (-levelMap.GetLength(0)) + x, 0);
                        PlaceBlock(x, y, position, parent.transform, levelMapClosed);
                    }
                }
            break;
            case 4:
            // Bottom Right
                for (int x = levelMap.GetLength(0)-1; x >= 0; x--) {
                    for (int y = levelMap.GetLength(1)-1; y >= 0; y--) {
                        Vector3 position = new Vector3((levelMap.GetLength(1) + (levelMap.GetLength(1) - y))-1, -(levelMap.GetLength(0) + (levelMap.GetLength(0) - x))+1, 0);
                        PlaceBlock(x, y, position, parent.transform, levelMapClosed);
                    }
                }
            break;
            default:
            break;
        }
    }

    private void PlaceBlock(int x, int y, Vector3 position, Transform parent, int[,] mapArray) {
        switch (mapArray[x,y]) {
            case 1:
                // Outside Corner
                HandleCorner(CreateWall(outCorner, position, parent, new Vector2Int(x, y)), new Vector2Int(x, y));
            break;
            case 2:
                // Outside Wall
                HandleStraight(CreateWall(outWall, position, parent, new Vector2Int(x, y)), new Vector2Int(x, y));
            break;
            case 3:
                // Inside Corner
                HandleCorner(CreateWall(inCorner, position, parent, new Vector2Int(x, y)), new Vector2Int(x, y));
            break;
            case 4:
                // Inside Wall
                HandleStraight(CreateWall(inWall, position, parent, new Vector2Int(x, y)), new Vector2Int(x, y));
            break;
            case 5:
                // Normal Pellet
                GameObject newPellet = Instantiate(pellet, position, new Quaternion(), levelParent);
                newPellet.transform.localScale = new Vector3(0.1f, 0.1f, 1);
            break;
            case 6:
                // Power Pellet
                GameObject newPPellet = Instantiate(pellet, position, new Quaternion(), levelParent);
                newPPellet.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                Pellet pelletScript = newPPellet.GetComponent<Pellet>();
                pelletScript.ConvertToPowerPellet();
            break;
            case 7:
                // T Junction
                CreateWall(tJunction, position, parent, new Vector2Int(x, y));
            break;
            default:
                //Nothing
            break;
        }
    }

    private GameObject CreateWall(Sprite sprite, Vector3 position, Transform parent, Vector2Int gridPos) {
        // Create block
        GameObject block = new GameObject();
        block.transform.parent = parent;
        // Create Sprite
        SpriteRenderer sr = block.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        wallSprites.Add(sr);
        // Scale and position
        block.transform.localScale = useSquare ? new Vector3(0.2f, 0.2f, 1) : new Vector3(0.1f, 0.1f, 1);
        block.transform.position = new Vector3(position.x, position.y, 0);

        block.AddComponent<BoxCollider2D>();
        BlockDebug debug = block.AddComponent<BlockDebug>();
        debug.SetPos(gridPos);

        return block;
    }

    private void HandleStraight(GameObject block, Vector2Int pos) {
        if(pos.x > 0 && pos.x < levelMap.GetLength(0)-1) {
            if(CheckForEmpty(pos.x-1, pos.y) || CheckForEmpty(pos.x+1, pos.y)) {
                Quaternion newRot = block.transform.rotation;
                newRot.eulerAngles = new Vector3(0,0,90);
                block.transform.rotation = newRot;
            }
        } else if(pos.x == levelMap.GetLength(0)-1) {
            if(CheckForEmpty(pos.x-1, pos.y)) {
                Quaternion newRot = block.transform.rotation;
                newRot.eulerAngles = new Vector3(0,0,90);
                block.transform.rotation = newRot;
            }
        } else if(pos.x == 0) {
            if(CheckForEmpty(pos.x+1, pos.y)) {
                Quaternion newRot = block.transform.rotation;
                newRot.eulerAngles = new Vector3(0,0,90);
                block.transform.rotation = newRot;
            }
        }
    }

    private void HandleCorner(GameObject block, Vector2Int pos) {
        float rotation = 0;

        // Add 90 to rotation if pos below is empty
        // Is the position on the right edge, if not check pos to right
        if(pos.y == levelMap.GetLength(1)-1) {
            if(!CheckForEmpty(pos.x, pos.y-1)) {
                rotation += 90;
            }
            
        } else if(CheckForEmpty(pos.x, pos.y+1)) {
            rotation += 90;
        }

        // Add 90 to rotation if pos to right is empty
        // Is the position on the bottom edge, if not check pos below
        if(pos.x == levelMap.GetLength(0)-1) {
            rotation += 90;
        } else if(CheckForEmpty(pos.x+1, pos.y)) {
            rotation += 90;
        }

        // Multiply rotation by -1
        // Is the position at top edge, if not check pos above
        if(pos.x == 0) {
            rotation = rotation * -1;
            //Debug.Log(pos.x + " " + pos.y);
        } else if(CheckForEmpty(pos.x-1, pos.y)) {
            rotation = rotation * -1;
            Debug.Log((pos.x-1) + " " + (pos.y));
        }

        // Apply new Rotation
        Quaternion newRot = block.transform.rotation;
        newRot.eulerAngles = new Vector3(0,0,rotation);
        block.transform.rotation = newRot;
    }


    private bool CheckForEmpty(Vector2Int pos) => (levelMap[pos.x, pos.y] == 5 || levelMap[pos.x, pos.y] == 6 || levelMap[pos.x, pos.y] == 0);
    private bool CheckForEmpty(int x, int y) => (levelMap[x, y] == 5 || levelMap[x, y] == 6 || levelMap[x, y] == 0);

}
