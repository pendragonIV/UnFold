using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridCellManager : MonoBehaviour
{
    public static GridCellManager instance;

    [SerializeField]
    private Tilemap tileMap;
    [SerializeField]
    private List<Vector3Int> tileLocations = new List<Vector3Int>();
    [SerializeField]
    private List<Vector3Int> placedCell = new List<Vector3Int>();

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void GetMoveAbleCell()
    {
        for (int x = tileMap.cellBounds.xMin; x < tileMap.cellBounds.xMax; x++)
        {
            for (int y = tileMap.cellBounds.yMin; y < tileMap.cellBounds.yMax; y++)
            {
                Vector3Int localLocation = new Vector3Int(
                    x: x,
                    y: y,
                    z: 0);
                if (tileMap.HasTile(localLocation))
                {
                    tileLocations.Add(localLocation);
                }
            }
        }
    }
    public void SetTileMap(Tilemap tilemap)
    {
        this.tileMap = tilemap;
        GetMoveAbleCell();
    }
    public bool IsPlaceableArea(Vector3Int mouseCellPos)
    {
        if (tileMap.GetTile(mouseCellPos) == null)
        {
            return false;
        }
        return true;
    }

    public List<Vector3Int> GetCellsPosition()
    {
        return tileLocations;
    }

    public Vector3Int GetObjCell(Vector3 position)
    {
        Vector3Int cellPosition = tileMap.WorldToCell(position);
        return cellPosition;
    }

    public Vector3 PositonToMove(Vector3Int cellPosition)
    {
        return tileMap.GetCellCenterWorld(cellPosition);
    }

    public Vector3 GetCellGap()
    {
        return tileMap.cellGap;
    }

    public void PlacedCell(Vector3Int cell)
    {
        if(!placedCell.Contains(cell))
        {
            placedCell.Add(cell);
        }
    }

    public void RemovePlacedCell(Vector3Int cell)
    {
        if (placedCell.Contains(cell))
        {
            placedCell.Remove(cell);
        }
    }

    public bool IsCellPaced(Vector3Int cell)
    {
        if(placedCell.Contains(cell))
        {
            Debug.Log("Can't place");
        }
        return placedCell.Contains(cell);
    }
}
