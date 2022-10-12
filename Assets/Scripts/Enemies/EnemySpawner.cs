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

        private Camera mainCamera;
        [SerializeField] private int spawnMargin = 1;

        private void Start()
        {
            mainCamera = Camera.main;

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

        private List<Vector3> GetAvailableSpawnPositions()
        {
            List<Vector3> availableSpawnPositions = new List<Vector3>();

            // Gets all positions inside the bounds of tilemap.
            foreach (Vector3Int localPosition in tilemap.cellBounds.allPositionsWithin)
            {
                // Check if tile has a tile at the local position within the bounds.
                if (tilemap.HasTile(localPosition))
                {
                    Vector3 worldPosition = tilemap.CellToWorld(localPosition);

                    // If the cell has a collider in it or it is inside the camera -> continue.
                    if (CellHasCollider(worldPosition) || CellIsInsideCamera(worldPosition))
                    {
                        continue;
                    }

                    availableSpawnPositions.Add(worldPosition);
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

        private bool CellIsInsideCamera(Vector3 cellWordPosition)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(cellWordPosition);

            // Check if the given point is within the screen position plus an additional margin.
            return screenPosition.x + spawnMargin * pixelsPerUnit >= 0 &&
                   screenPosition.x - spawnMargin * pixelsPerUnit <= Screen.width &&
                   screenPosition.y + spawnMargin * pixelsPerUnit >= 0 &&
                   screenPosition.y - spawnMargin * pixelsPerUnit <= Screen.height;
        }
    }
}