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

        // Get all eligible grid cells that are not in the camera view
        private TileBase[] allTiles;
        private Camera mainCamera;

        private void Start()
        {
            BoundsInt tileBounds = tilemap.cellBounds;
            allTiles = tilemap.GetTilesBlock(tileBounds);
            mainCamera = Camera.main;
        
            Spawn();
        }

        private List<Vector3> GetCellsOutsideCameraView()
        {
            List<Vector3> spawnPositions = new List<Vector3>();

            foreach (Vector3Int cellPosition in tilemap.cellBounds.allPositionsWithin)
            {
                Vector3 worldPosition = tilemap.CellToWorld(cellPosition);
                Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
                if (screenPosition.x + pixelsPerUnit >= 0 && screenPosition.x - pixelsPerUnit <= Screen.width &&
                    screenPosition.y + pixelsPerUnit >= 0 && screenPosition.y - pixelsPerUnit <= Screen.height)
                {
                    continue;
                }

                spawnPositions.Add(worldPosition);
            }

            return spawnPositions;
        }
    
        private void Spawn()
        {
            foreach(Vector3 position in GetCellsOutsideCameraView())
            {
                var e = Instantiate(enemy, position, quaternion.identity);
                e.GetComponent<Enemy>().player = player;
            }
        }
    }
}