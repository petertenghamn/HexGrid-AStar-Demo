using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Prefabs
    public GameObject playingUIPrefab;
    public GameObject unitPrefab;
    public Material matA, matB;
    
    // Main Components
    PlayerUI ui;
    HexGrid grid;
    Pathfinder pathfinder;
    
    // Map info
    private bool turnConcluding = false;
    private int maxTurnCycles = 1000;

    // Unit variables
    private List<Unit> units;
    private Unit unitSelected;

    void Awake()
    {
        // UI Initialization
        GameObject playingUI = Instantiate(playingUIPrefab, transform);
        ui = playingUI.GetComponent<PlayerUI>();

        // Grid
        GameObject gridObj = new GameObject();
        gridObj.transform.parent = transform;
        grid = gridObj.AddComponent<HexGrid>();

        // Pathfinder
        pathfinder = transform.gameObject.AddComponent<Pathfinder>();
        
        // Units
        units = new List<Unit>();
    }

    void Start()
    {
        // Populate grid on scene
        grid.GenerateGrid();

        // Place unit to move around
        GameObject unitObj = Instantiate(unitPrefab);
        Unit unit = unitObj.AddComponent<Unit>();
        unit.Initialize(matA, matB, "Sample", 5);
        unit.PlaceOnTile(grid.GetTile(new GridPosition(0, 0, 0)));
        units.Add(unit);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //left click
        {
            if (!ui.OverCheck())
            {
                LeftClick();
            }
        }

        if (Input.GetMouseButtonDown(1)) //right click
        {
            if (!ui.OverCheck())
            {
                RightClick();
            }
        }
    }

    private void LeftClick()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            Tile tile = hit.transform.GetComponent<Tile>();

            // Deselection override
            unitSelected = null;
            foreach (Unit u in units)
            {
                u.SetSelected(false);
            }

            // Check to see if a unit is on the tile and select the unit
            if (tile.occupied)
            {
                foreach (Unit u in units)
                {
                    if (u.currentTile.gridPos.x == tile.gridPos.x && u.currentTile.gridPos.y == tile.gridPos.y && u.currentTile.gridPos.z == tile.gridPos.z)
                    {
                        u.SetSelected(true);
                        unitSelected = u;
                    }
                }
            }
            // For Debug, print tile coordinate clicked
            Debug.Log("Clicked on - (" + tile.gridPos.x + ", " + tile.gridPos.y + ", " + tile.gridPos.z + ")");
        }
    }

    private void RightClick()
    {
        // Set the target location for currently selected unit
        if (unitSelected != null)
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                Tile tile = hit.transform.GetComponent<Tile>();

                if (!tile.occupied)
                {
                    FindUnitsPath(unitSelected, tile);
                }
            }
        }
    }
    
    void FindUnitsPath(Unit unit, Tile target)
    {
        Tile startTile = grid.GetTile(unit.currentTile.gridPos);
        unit.SetPath(pathfinder.FindPath(startTile, target));
    }

    /*
    public void RequestSpawningUnit(int id)
    {
        if (!turnConcluding)
        {
            DeselectUnits();
            //check if the player is already trying to place a unit and destroy that one first
            if (placingUnit.obj != null)
            {
                Destroy(placingUnit.obj);
            }

            //create a prefab of the unit requested if the player has the resources
            Debug.Log("temp cost assigned to unit.");
            bool request = player.CheckFunding(20);

            if (request)
            {
                //instantiate requested unit into a placeable template
                placingUnit.id = id;
                placingUnit.obj = Instantiate(unitPrefab);
                placingUnit.obj.transform.position = new Vector3(0, -10, 0);
                placingUnit.tile = null;
                placingUnit.active = true;
            }
            else
            {
                Debug.LogError("Not enough resources");
            }
        }
    }
    */
    
    public void RequestPlayTurn()
    {
        // Request from UI to play a cycle
        if (!turnConcluding)
        {
            PlayoutTurn();
        }
    }

    private void PlayoutTurn()
    {
        turnConcluding = true;
        Debug.Log("Moving units!");

        foreach (Unit u in units)
        {
            u.StartTurnProcess(maxTurnCycles);
        }
        StartCoroutine(ConcludeTurn());
    }

    private IEnumerator ConcludeTurn()
    {
        int cyclesPassed = 0;

        while (cyclesPassed < maxTurnCycles)
        {
            //loop each unit to move/shoot for the turn
            foreach (Unit u in units)
            {
                u.TurnUpdate();
            }

            cyclesPassed++;
            //Wait for a frame each cycle so the movement looks smooth
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Debug.Log("Turn complete!");
        turnConcluding = false;

        yield return null;
    }
}
