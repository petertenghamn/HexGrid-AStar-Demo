using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public GameObject tilePF;
    
    private int size;
    private Dictionary<GridPosition, Tile> grid;
    public Tile GetTile(GridPosition pos)
    {
        return grid[pos];
    }

    public void GenerateGrid()
    {
        size = 10;
        grid = GenerateHexagon(size, tilePF);
        PopulateGrid();
    }

    private Dictionary<GridPosition, Tile> GenerateHexagon(int size, GameObject tilePF)
    {
        // Generate a hex shaped grid using the plate position as offset
        Dictionary<GridPosition, Tile> tiles = new Dictionary<GridPosition, Tile>();

        for (int i = 0; i < size; i++)
        {
            for (int x = -i; x <= i; x++)
                for (int y = -i; y <= i; y++)
                    for (int z = -i; z <= i; z++)
                        if (Mathf.Abs(x) + Mathf.Abs(y) + Mathf.Abs(z) == i * 2 && x + y + z == 0)
                        {
                            GameObject tileObj = Instantiate(tilePF);
                            Tile newTile = tileObj.GetComponent<Tile>();

                            newTile.Initialize(new GridPosition(x, y, z));
                            //newTile.RequestTileTypeChange(HexTile.TileType.EMPTY);
                            tileObj.transform.parent = transform;

                            // Add the tile to the returned grid
                            tiles.Add(newTile.gridPos, newTile);
                        }
        }

        return tiles;
    }

    private void PopulateGrid()
    {
        // Add random blockers to random tiles, besides the middle
        List<Tile> tiles = new List<Tile>();
        tiles.AddRange(grid.Values);
        
        for (int i = 0; i < size * 3; i++)
        {
            Tile randTile = GetRandomTile(tiles, false);
            randTile.SetTileState(Tile.State.UNWALKABLE);
        }
    }

    private Tile GetRandomTile(List<Tile> tiles, bool recursiveCall)
    {
        Tile randTile = tiles[Random.Range(0, tiles.Count)];
        GridPosition pos = new GridPosition(0, 0, 0);
        if (randTile.gridPos.Equals(pos) || !randTile.GetTileTraversable())
        {
            if (!recursiveCall)
            {
                return GetRandomTile(tiles, true);
            }
            else
            {
                Debug.Log("Failed blocker twice, aborting!");
                return null;
            }
        }
        return randTile;
    }

    public List<Tile> GetNeighbors(Tile node)
    {
        List<Tile> neighbors = new List<Tile>();
        GridPosition pos = node.gridPos;
        // Add the neighbors which are present in the grid
        if (grid.ContainsKey(new GridPosition(pos.x - 1, pos.y, pos.z + 1)))
            neighbors.Add(grid[new GridPosition(pos.x - 1, pos.y, pos.z + 1)]);
        if (grid.ContainsKey(new GridPosition(pos.x + 1, pos.y - 1, pos.z)))
            neighbors.Add(grid[new GridPosition(pos.x + 1, pos.y - 1, pos.z)]);
        if (grid.ContainsKey(new GridPosition(pos.x, pos.y + 1, pos.z - 1)))
            neighbors.Add(grid[new GridPosition(pos.x, pos.y + 1, pos.z - 1)]);
        if (grid.ContainsKey(new GridPosition(pos.x + 1, pos.y, pos.z - 1)))
            neighbors.Add(grid[new GridPosition(pos.x + 1, pos.y, pos.z - 1)]);
        if (grid.ContainsKey(new GridPosition(pos.x - 1, pos.y + 1, pos.z)))
            neighbors.Add(grid[new GridPosition(pos.x - 1, pos.y + 1, pos.z)]);
        if (grid.ContainsKey(new GridPosition(pos.x, pos.y - 1, pos.z + 1)))
            neighbors.Add(grid[new GridPosition(pos.x, pos.y - 1, pos.z + 1)]);
        return neighbors;
    }
}
