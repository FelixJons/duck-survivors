using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Enemy enemy;
        [SerializeField] private int pixelsPerUnit = 16;
        [SerializeField] private Transform player;
        [SerializeField] private int screenMargin = 1;
        [SerializeField] private int spawnMargin = 3;

        private Camera mainCamera;
        private List<Vector3> cellsWithTileWorld;


        private void Start()
        {
            mainCamera = Camera.main;

            cellsWithTileWorld = GetCellsWithTiles();

            Spawn();
        }

        private void Spawn()
        {
            List<Vector3> availableSpawnPositions = GetAvailableSpawnPositions();

            foreach (Vector3 worldPosition in availableSpawnPositions)
            {
                var e = Instantiate(enemy, worldPosition, quaternion.identity);
                e.GetComponent<Enemy>().player = player;
            }
        }

        private List<Vector3> GetCellsWithTiles()
        {
            List<Vector3> cellsWithTiles = new List<Vector3>();

            // Gets all positions inside the bounds of tilemap.
            foreach (Vector3Int localPosition in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.HasTile(localPosition))
                {
                    Vector3 worldPosition = tilemap.CellToWorld(localPosition);
                    cellsWithTiles.Add(worldPosition);
                }
            }

            return cellsWithTiles;
        }

        private List<Vector3> GetAvailableSpawnPositions()
        {
            List<Vector3> availableSpawnPositions = new List<Vector3>();

            foreach (Vector3 tileWorldPosition in cellsWithTileWorld)
            {
                // If the cell has a collider in it or it is inside the camera + screenMargin -> continue.
                if (CellHasCollider(tileWorldPosition) ||
                    CellIsInsideScreenWithMargin(tileWorldPosition, screenMargin))
                {
                    continue;
                }
                
                // If the cell is outside of the camera + margin but inside camera + spawnMargin -> spawn.
                if (CellIsInsideScreenWithMargin(tileWorldPosition, spawnMargin))
                {
                    availableSpawnPositions.Add(tileWorldPosition);
                }
            }

            return availableSpawnPositions;
        }

        private bool CellHasCollider(Vector3 cellWorldPos)
        {
            // Projects a box with centre at the specified position. The size is for "both" sides. 
            Collider2D col = Physics2D.OverlapBox((Vector2) cellWorldPos + new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f), 0);
            return col != null;
        }


        private bool CellIsInsideScreenWithMargin(Vector3 cellWorldPosition, int marginInCellDistance)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(cellWorldPosition);

            // Check if the given point is within the screen position plus an additional margin.
            return screenPosition.x + marginInCellDistance * pixelsPerUnit >= 0 &&
                   screenPosition.x - marginInCellDistance * pixelsPerUnit <= Screen.width &&
                   screenPosition.y + marginInCellDistance * pixelsPerUnit >= 0 &&
                   screenPosition.y - marginInCellDistance * pixelsPerUnit <= Screen.height;
        }
    }
}