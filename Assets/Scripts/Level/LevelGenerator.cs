using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

//! This script is a mess!
public class LevelGenerator : MonoBehaviour {

    [SerializeField]
    private Color wallColour = new Color(1,1,1,1);

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

    [SerializeField]
    private GameObject player;

    private List<SpriteRenderer> wallSprites;

    //! Turns out this just records the first quarter, sort it out dumbass
    // TODO Use get all with tag after instantiation?
    // Do i even need a list of them all?
    private Dictionary<Vector2Int, GameObject> allPellets = new Dictionary<Vector2Int, GameObject>();
    public Dictionary<Vector2Int, GameObject> AllPellets { get { return allPellets; } }

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


        // Create the map
        GenMap();

        // for (int i = 1; i < 5; i++) {
        //     GenMap(i);
        // }

        Player playerController = Instantiate<GameObject>(player, allPellets[new Vector2Int(1,1)].transform.position, new Quaternion(0,0,0,0)).GetComponent<Player>();
        playerController.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        playerController.AddNavPoint(allPellets[new Vector2Int(1,1)]);
        playerController.AddNavPoint(allPellets[new Vector2Int(1,6)]);
        playerController.AddNavPoint(allPellets[new Vector2Int(5,6)]);
        playerController.AddNavPoint(allPellets[new Vector2Int(5,1)]);
        playerController.StartNavigation();

        // for (int x = 0; x < allElements.GetLength(0); x++) {
        //     string line = "";
        //     for (int y = 0; y < allElements.GetLength(1); y++) {
        //         line += allElements[x,y] + " ";
        //     }
        //     debug.text += line + "\n";
        // }
    }

    void Update() {
        //Color newColour = new Color(Random.Range(0f,1f),Random.Range(0f,1f), Random.Range(0f,1f), 1);
        foreach(SpriteRenderer sprite in wallSprites) {
            if(sprite) sprite.color = wallColour;
        }
    }

    private void GenMap() {
        GameObject parent = new GameObject("Quarter 1");
        parent.transform.parent = levelParent;

        // Create pelletParent
        GameObject pelletParent = new GameObject("PelletParent");
        pelletParent.transform.parent = parent.transform;

        // First quarter
        for (int x = 0; x < levelMap.GetLength(0); x++) {
            for (int y = 0; y < levelMap.GetLength(1); y++) {
                Vector3 position = new Vector3(y, -x, 0);
                PlaceBlock(x, y, position, parent.transform, pelletParent.transform, levelMap);

                //allElements[x,y] = levelMap[x,y];
            }
        }

        // Duplicate and position the first quarter
        for (int i = 2; i < 5; i++) {
            GameObject newQuarter = Object.Instantiate(parent, levelParent);
            newQuarter.transform.name = "Quarter " + i;
            Quaternion newRot = newQuarter.transform.rotation;
            
            // Position and rotate
            switch (i) {
                case 2:
                    newRot.eulerAngles += new Vector3(0,180,0);
                    newQuarter.transform.position += new Vector3(27,0,0);

                    // Remove the two top spawn doors
                    foreach(GameObject wall in GameObject.FindGameObjectsWithTag("SpawnDoor")) {
                        //Debug.Log("Destroying spawn door", wall);
                        Destroy(wall);
                    }
                break;
                case 3:
                    newRot.eulerAngles += new Vector3(180,0,0);
                    newQuarter.transform.position += new Vector3(0,-29,0);
                break;
                case 4:
                    newRot.eulerAngles += new Vector3(180,180,0);
                    newQuarter.transform.position += new Vector3(27,-29,0);
                break;
                default:
                    Debug.LogError("You shouldn't be here");
                break;
            }
            newQuarter.transform.rotation = newRot;
        }

        // Get all wall sprites
        foreach(GameObject wall in GameObject.FindGameObjectsWithTag("Wall")) {
            wallSprites.Add(wall.GetComponent<SpriteRenderer>());
        }

        foreach(GameObject wall in GameObject.FindGameObjectsWithTag("SpawnDoor")) {
            if(wall) wallSprites.Add(wall.GetComponent<SpriteRenderer>());
        }

        // Make sure all pellets are upright
        foreach (GameObject pellet in GameObject.FindGameObjectsWithTag("Pellet")) {
            //Debug.Log(pellet.transform.rotation);
            pellet.transform.rotation = Quaternion.identity;
        }
    }

    private void PlaceBlock(int x, int y, Vector3 position, Transform parent, Transform pelletParent, int[,] mapArray) {
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
                GameObject newPellet = Instantiate(pellet, position, new Quaternion(), pelletParent.transform);
                newPellet.transform.localScale = new Vector3(0.1f, 0.1f, 1);

                // Debug
                newPellet.AddComponent<BoxCollider2D>();
                BlockDebug debug = newPellet.AddComponent<BlockDebug>();
                debug.SetPos(new Vector2Int(x, y));

                // Add pellets to list
                if(!allPellets.ContainsKey(new Vector2Int(x,y))) allPellets.Add(new Vector2Int(x, y), newPellet);
            break;
            case 6:
                // Power Pellet
                GameObject newPPellet = Instantiate(pellet, position, new Quaternion(), pelletParent.transform);
                newPPellet.transform.localScale = new Vector3(0.1f, 0.1f, 1);
                Pellet pelletScript = newPPellet.GetComponent<Pellet>();
                pelletScript.ConvertToPowerPellet();

                // Debug
                newPPellet.AddComponent<BoxCollider2D>();
                BlockDebug pdebug = newPPellet.AddComponent<BlockDebug>();
                pdebug.SetPos(new Vector2Int(x, y));
            break;
            case 7:
                // T Junction
                CreateWall(tJunction, position, parent, new Vector2Int(x, y));
            break;
            default:
                //Nothing

                // Check if it's the underside of the spawn room
                if(x == 12 && y == 13) {
                    GameObject spawnDoor = HandleStraight(CreateWall(inWall, position, parent, new Vector2Int(x, y)), new Vector2Int(x, y));
                    spawnDoor.tag = "SpawnDoor";
                }
            break;
        }
    }

    private GameObject CreateWall(Sprite sprite, Vector3 position, Transform parent, Vector2Int gridPos) {
        // Create block
        GameObject block = new GameObject();
        block.transform.parent = parent;
        block.tag = "Wall";
        // Create Sprite
        SpriteRenderer sr = block.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        //wallSprites.Add(sr);
        sr.color = wallColour;
        // Scale and position
        block.transform.localScale = useSquare ? new Vector3(0.2f, 0.2f, 1) : new Vector3(0.1f, 0.1f, 1);
        block.transform.position = new Vector3(position.x, position.y, 0);

        block.AddComponent<BoxCollider2D>();
        BlockDebug debug = block.AddComponent<BlockDebug>();
        debug.SetPos(gridPos);

        // Hacky handle weird block
        if(gridPos == new Vector2Int(0, 13)) {
            block.transform.Rotate(new Vector3(0,0,-90));
        }

        return block;
    }

    private GameObject HandleStraight(GameObject block, Vector2Int pos) {
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
        return block;
    }

    private GameObject HandleCorner(GameObject block, Vector2Int pos) {
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
            //Debug.Log((pos.x-1) + " " + (pos.y));
        }


        // Handle weird blocks <3
        if (pos == new Vector2Int(7, 13)) {
            rotation = -90;
        }

        if (pos == new Vector2Int(9, 8)) {
            rotation = 90;
        }

        // Apply new Rotation
        Quaternion newRot = block.transform.rotation;
        newRot.eulerAngles = new Vector3(0,0,rotation);
        block.transform.rotation = newRot;

        return block;
    }


    private bool CheckForEmpty(Vector2Int pos) => (levelMap[pos.x, pos.y] == 5 || levelMap[pos.x, pos.y] == 6 || levelMap[pos.x, pos.y] == 0);
    private bool CheckForEmpty(int x, int y) => (levelMap[x, y] == 5 || levelMap[x, y] == 6 || levelMap[x, y] == 0);

}
