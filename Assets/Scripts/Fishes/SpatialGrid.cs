using UnityEngine;
using System.Collections.Generic;

public class SpatialGrid
{
    private float _cellSize;
    private Vector2 _originPosition;
    private Dictionary<Vector2Int, List<FlockingFish>> _grid = new Dictionary<Vector2Int, List<FlockingFish>>();

    public SpatialGrid(float cellSize, Vector2 originPosition)
    {
        _cellSize = cellSize;
        _originPosition = originPosition;
    }

    public void AddFish(FlockingFish fish)
    {
        if (fish == null || fish.transform == null)
        {
            return;
        }

        Vector2Int cellIndex = GetCellIndex(fish.transform.position);

        if (!_grid.TryGetValue(cellIndex, out var fishList))
        {
            fishList = new List<FlockingFish>();
            _grid[cellIndex] = fishList;
        }

        fishList.Add(fish);
    }

    public void RemoveFish(FlockingFish fish)
    {
        if (fish == null || fish.transform == null)
        {
            return;
        }

        Vector2Int cellIndex = GetCellIndex(fish.transform.position);

        if (_grid.TryGetValue(cellIndex, out var fishList))
        {
            fishList.Remove(fish);

            if (fishList.Count == 0)
            {
                _grid.Remove(cellIndex);
            }
        }
    }

    public void UpdateFishCell(FlockingFish fish, Vector2 previousPosition)
    {
        if (fish == null || fish.transform == null)
        {
            return;
        }

        Vector2Int oldCellIndex = GetCellIndex(previousPosition);
        Vector2Int newCellIndex = GetCellIndex(fish.transform.position);

        if (oldCellIndex != newCellIndex)
        {
            if (_grid.TryGetValue(oldCellIndex, out var oldCellFishes))
            {
                oldCellFishes.Remove(fish);

                if (oldCellFishes.Count == 0)
                {
                    _grid.Remove(oldCellIndex);
                }
            }

            if (!_grid.TryGetValue(newCellIndex, out var newCellFishes))
            {
                newCellFishes = new List<FlockingFish>();
                _grid[newCellIndex] = newCellFishes;
            }

            newCellFishes.Add(fish);
        }
    }

    public List<FlockingFish> GetNeighbors(FlockingFish fish, float neighborRadius)
    {
        var neighbors = new List<FlockingFish>();

        if (fish == null || fish.transform == null)
        {
            return neighbors;
        }

        Vector2Int cellIndex = GetCellIndex(fish.transform.position);

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector2Int neighborCellIndex = new Vector2Int(cellIndex.x + x, cellIndex.y + y);
                if (_grid.TryGetValue(neighborCellIndex, out var cellFishes))
                {
                    foreach (FlockingFish otherFish in cellFishes)
                    {
                        if (otherFish == null || otherFish == fish || otherFish.transform == null)
                        {
                            continue;
                        }

                        float distance = Vector2.Distance(fish.transform.position, otherFish.transform.position);
                        if (distance < neighborRadius)
                        {
                            neighbors.Add(otherFish);
                        }
                    }
                }
            }
        }
        return neighbors;
    }

    private Vector2Int GetCellIndex(Vector2 position)
    {
        int xIndex = Mathf.FloorToInt((position.x - _originPosition.x) / _cellSize);
        int yIndex = Mathf.FloorToInt((position.y - _originPosition.y) / _cellSize);
        return new Vector2Int(xIndex, yIndex);
    }
}
