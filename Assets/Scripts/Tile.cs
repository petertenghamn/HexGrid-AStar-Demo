using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public bool occupied;
    
    // Coordinates
    public static float HEXTILE_RADIUS = 1f;
    public static float HEXTILE_WIDTH = Mathf.Sqrt(3) / 2;
    public GridPosition gridPos
    {
        get; private set;
    }

    // What the tile is (mainly has to do with map and unit interactivity)
    public enum State { EMPTY, UNWALKABLE };
    public State state
    {
        get; private set;
    }

    public void SetTileState(State tileState)
    {
        if (tileState == State.UNWALKABLE)
        {
            // Spawn a box basic obj to indicate blocker
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = transform.position;
            cube.transform.position += new Vector3(0, 1.15f, 0);
            cube.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            cube.GetComponent<Renderer>().material.color = Color.red;
            cube.transform.parent = transform.parent;
        }
        state = tileState;
    }
    public bool GetTileTraversable()
    {
        if (state != State.UNWALKABLE)
            return true;
        return false;
    }

    // Variables for pathfinder
    public int gCost;
    public int hCost;
    public int fCost
    {
        get { return gCost + hCost; }
    }
    public Tile parent;
    
    public void Initialize(GridPosition pos)
    {
        gridPos = pos;

        // World position
        Vector3 position;

        float height = HEXTILE_RADIUS * 2;
        float width = HEXTILE_WIDTH * height;

        float vert = height * 0.75f;
        float horiz = width;

        position.x = horiz * (pos.x + pos.y / 2f);
        position.y = 0;
        position.z = vert * pos.y;

        transform.localPosition = position;

        // Initialize some variables
        SetTileState(State.EMPTY);
        occupied = false;
    }
}

// 3 directional positioning using Hex coordinates
public struct GridPosition
{
    public int x
    {
        get; private set;
    }
    public int y
    {
        get; private set;
    }
    public int z
    {
        get; private set;
    }

    public GridPosition(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }

    public override string ToString()
    {
        return "[" + x + ", " + y + ", " + z + "]";
    }
}
