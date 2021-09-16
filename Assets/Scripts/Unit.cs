using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // General variables
    GameObject unitObj;
    private string type;
    private int speed;

    // Selection indication
    private Material matA, matB;

    public bool selected
    {
        get; private set;
    }
    public void SetSelected(bool update)
    {
        // Change material to indicate selection
        if (update)
        {
            unitObj.GetComponent<Renderer>().material = matB;
        }
        else if (selected) // (!update) already checked with the if
        {
            unitObj.GetComponent<Renderer>().material = matA;
        }
        selected = update;
    }

    // Pathfinder variables
    public Tile currentTile;
    private List<PathNode> path;
    public void SetPath(List<Tile> input)
    {
        // Clear existing path
        if (path.Count > 0)
        {
            foreach (PathNode n in path)
            {
                Destroy(n.obj);
            }
            path.Clear();
        }

        // Create new path
        if (input != null)
        {
            for (int i = 0; i < input.Count; i++)
            {
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                sphere.transform.position = input[i].transform.position;
                sphere.transform.position += new Vector3(0, 1.15f, 0);
                sphere.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
                sphere.GetComponent<Renderer>().material.color = Color.magenta;
                sphere.transform.parent = transform.parent;
                path.Add(new PathNode(sphere, input[i]));
            }
        }
        else
        {
            Debug.LogError("Path input empty");
        }
    }

    public void Initialize(Material mA, Material mB, string _type, int _speed)
    {
        GameObject parent = new GameObject();
        parent.transform.position = new Vector3(0, 0, 0);
        unitObj = transform.gameObject;
        unitObj.transform.parent = parent.transform;

        matA = mA;
        matB = mB;
        
        type = _type;
        speed = _speed;

        path = new List<PathNode>();
    }

    public void PlaceOnTile(Tile tile)
    {
        currentTile = tile;
        tile.occupied = true;
        Vector3 pos = tile.transform.position;
        pos.y += 1.5f;
        unitObj.transform.position = pos;
    }

    #region turn cycle
    // Variables needed for movement function
    private int turnTicks;
    private int ticksPerMove, timesMoved;

    public void StartTurnProcess(int maxTurnTicks)
    {
        // Setup some variables to use when playing out the turn
        timesMoved = 0;
        turnTicks = maxTurnTicks;
        ticksPerMove = (maxTurnTicks - speed) / speed;
    }

    bool moving;
    int moveCycles;
    Vector3 moveTarget;
    float posXDiff, posZDiff;

    public void TurnUpdate()
    {
        if (!moving && path.Count > 0 && timesMoved < speed)
        {
            // Move based on the total turn cycles available and cycles allowed per move
            posXDiff = (path[0].obj.transform.position.x - unitObj.transform.position.x) / ticksPerMove;
            posZDiff = (path[0].obj.transform.position.z - unitObj.transform.position.z) / ticksPerMove;
            moveTarget = path[0].obj.transform.position;

            moveCycles = 0;
            moving = true;
        }
        // Update the units position
        else if (moving)
        {
            moveCycles++;

            // Move towards target
            Vector3 move = new Vector3(posXDiff, 0, posZDiff);
            unitObj.transform.Translate(move);

            // Check to see if the unit is close enough to the target position
            if (moveCycles >= ticksPerMove)
            {
                // Move from old tile
                currentTile.occupied = false;
                currentTile = path[0].tile;
                currentTile.occupied = true;

                Destroy(path[0].obj);
                path.RemoveAt(0);

                timesMoved++;
                moving = false;
            }
        }
    }
    #endregion
    
    struct PathNode
    {
        public GameObject obj;
        public Tile tile;

        public PathNode(GameObject _obj, Tile _tile)
        {
            obj = _obj;
            tile = _tile;
        }
    }
}
