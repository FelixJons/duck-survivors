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

        private void Start()
        {
            mainCamera = Camera.main;
            
            Spawn();
        }

        private bool CellHasCollider(Vector3 cellWorldPos)
        {
            var c = Physics2D.OverlapBox((Vector2) cellWorldPos + new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), 0);
            return c != null;
        }

        private bool CellIsInsideCamera(Vector3 cellWordPos)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(cellWordPos);
            return screenPosition.x + pixelsPerUnit >= 0 && screenPosition.x - pixelsPerUnit <= Screen.width &&
                   screenPosition.y + pixelsPerUnit >= 0 && screenPosition.y - pixelsPerUnit <= Screen.height;
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

            foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3Int localPosition = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 worldPosition = tilemap.CellToWorld(localPosition);
                if (tilemap.HasTile(localPosition))
                {
                    if (CellHasCollider(worldPosition) || CellIsInsideCamera(worldPosition))
                    {
                        continue;
                    }

                    availableSpawnPositions.Add(worldPosition);
                }
            }

            return availableSpawnPositions;
        }
    }
}