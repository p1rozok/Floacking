using System.Collections.Generic;
using UnityEngine;

public class SpatialGrid
{
    public float cellSize { get; }
    public int gridWidth { get; }
    public int gridHeight { get; }
    public Vector2 originPosition { get; }

    private Dictionary<Vector2Int, List<FlockingFish>> grid;

    public SpatialGrid(float cellSize, int gridWidth, int gridHeight, Vector2 originPosition)
    {
        this.cellSize = cellSize;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.originPosition = originPosition;

        grid = new Dictionary<Vector2Int, List<FlockingFish>>();
    }

    public Vector2Int GetCellIndex(Vector2 position)
    {
        int x = Mathf.FloorToInt((position.x - originPosition.x) / cellSize);
        int y = Mathf.FloorToInt((position.y - originPosition.y) / cellSize);
        return new Vector2Int(x, y);
    }

    public void AddFish(FlockingFish fish)
    {
        Vector2Int cellIndex = GetCellIndex(fish.transform.position);
        if (!grid.ContainsKey(cellIndex))
        {
            grid[cellIndex] = new List<FlockingFish>();
        }
        grid[cellIndex].Add(fish);
    }

    public void UpdateFishCell(FlockingFish fish, Vector2 previousPosition)
    {
        Vector2Int oldCellIndex = GetCellIndex(previousPosition);
        Vector2Int newCellIndex = GetCellIndex(fish.transform.position);

        if (oldCellIndex != newCellIndex)
        {
            if (grid.ContainsKey(oldCellIndex))
            {
                grid[oldCellIndex].Remove(fish);
            }

            if (!grid.ContainsKey(newCellIndex))
            {
                grid[newCellIndex] = new List<FlockingFish>();
            }
            grid[newCellIndex].Add(fish);
        }
    }

    public List<FlockingFish> GetNeighbors(FlockingFish fish, float neighborRadius)
    {
        List<FlockingFish> neighbors = new List<FlockingFish>();
        Vector2Int cellIndex = GetCellIndex(fish.transform.position);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighborCellIndex = new Vector2Int(cellIndex.x + x, cellIndex.y + y);

                if (grid.TryGetValue(neighborCellIndex, out List<FlockingFish> cellFish))
                {
                    foreach (FlockingFish otherFish in cellFish)
                    {
                        if (otherFish != fish)
                        {
                            float distance = Vector2.Distance(fish.transform.position, otherFish.transform.position);
                            if (distance < neighborRadius)
                            {
                                neighbors.Add(otherFish);
                            }
                        }
                    }
                }
            }
        }
        return neighbors;
    }
}


/*using System.Collections.Generic;
using UnityEngine;

public class SpatialGrid
{
    public float cellSize { get; }
    public int gridWidth { get; }
    public int gridHeight { get; }
    public Vector2 originPosition { get; }

    private Dictionary<Vector2Int, List<FlockingFish>> grid;

    public SpatialGrid(float cellSize, int gridWidth, int gridHeight, Vector2 originPosition)
    {
        this.cellSize = cellSize;
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.originPosition = originPosition;
        grid = new Dictionary<Vector2Int, List<FlockingFish>>();
    }

    public Vector2Int GetCellIndex(Vector2 position)
    {
        int x = Mathf.FloorToInt((position.x - originPosition.x) / cellSize);
        int y = Mathf.FloorToInt((position.y - originPosition.y) / cellSize);
        return new Vector2Int(x, y);
    }

    public void AddFish(FlockingFish fish)
    {
        Vector2Int cellIndex = GetCellIndex(fish.transform.position);
        if (!grid.ContainsKey(cellIndex))
        {
            grid[cellIndex] = new List<FlockingFish>();
        }
        grid[cellIndex].Add(fish);
    }

    public void UpdateFishCell(FlockingFish fish, Vector2 previousPosition)
    {
        Vector2Int oldCellIndex = GetCellIndex(previousPosition);
        Vector2Int newCellIndex = GetCellIndex(fish.transform.position);

        if (oldCellIndex != newCellIndex)
        {
            if (grid.ContainsKey(oldCellIndex))
            {
                grid[oldCellIndex].Remove(fish);
            }

            if (!grid.ContainsKey(newCellIndex))
            {
                grid[newCellIndex] = new List<FlockingFish>();
            }

            grid[newCellIndex].Add(fish);
        }
    }

    public List<FlockingFish> GetNeighbors(FlockingFish fish, float neighborRadius)
    {
        List<FlockingFish> neighbors = new List<FlockingFish>();
        Vector2Int cellIndex = GetCellIndex(fish.transform.position);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighborCellIndex = new Vector2Int(cellIndex.x + x, cellIndex.y + y);
                if (grid.ContainsKey(neighborCellIndex))
                {
                    foreach (FlockingFish otherFish in grid[neighborCellIndex])
                    {
                        if (otherFish != fish)
                        {
                            float distance = Vector2.Distance(fish.transform.position, otherFish.transform.position);
                            if (distance < neighborRadius)
                            {
                                neighbors.Add(otherFish);
                            }
                        }
                    }
                }
            }
        }
        return neighbors;
    }
}
*/