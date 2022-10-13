using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Enemies
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Enemy enemy;
        [SerializeField] private int pixelsPerUnit = 16;
        [SerializeField] private Transform player;
        [SerializeField] private int screenMargin = 1;
        [SerializeField] private int spawnDistanceOutsideOfCamera = 3;
        [SerializeField] private int numberOfEnemiesToSpawn;

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

            HashSet<Vector3> spawnPositions = new HashSet<Vector3>();

            for (int i = 0; i < numberOfEnemiesToSpawn; i++)
            {
                int randomNumber = Random.Range(0, availableSpawnPositions.Count);
                while (spawnPositions.Contains(availableSpawnPositions[randomNumber]))
                {
                    randomNumber = Random.Range(0, availableSpawnPositions.Count);
                }

                spawnPositions.Add(availableSpawnPositions[randomNumber]);
            }

            foreach (Vector3 worldPosition in spawnPositions)
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
                if (CellIsInsideScreenWithMargin(tileWorldPosition, spawnDistanceOutsideOfCamera))
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
            Vector3 cellScreenPosition = mainCamera.WorldToViewportPoint(cellWorldPosition);

            print(cellScreenPosition);

            // Check if the given point is within the screen position plus an additional margin.
            return cellScreenPosition.x + marginInCellDistance * 0.1f >= 0 &&
                   cellScreenPosition.x - marginInCellDistance * 0.1f <= 1.0 &&
                   cellScreenPosition.y + marginInCellDistance * 0.1f >= 0 &&
                   cellScreenPosition.y - marginInCellDistance * 0.1f <= 1.0;
        }

        private float PixelToViewportMagnitudeX(int pixelMagnitude)
        {
            return pixelMagnitude * (1f / Screen.width);
        }

        private float PixelToViewportMagnitudeY(int pixelMagnitude)
        {
            return pixelMagnitude * (1f / Screen.height);
        }
    }
}