using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGroundLayer : MonoBehaviour
{
    public int textureIndex = 0; // 페인팅할 텍스처의 인덱스
    public Vector3 position; // 텍스처를 페인팅할 월드 위치
    public float size = 10f; // 페인팅할 영역의 크기

    void Start()
    {
        Terrain[] terrains = FindObjectsOfType<Terrain>();
        foreach (Terrain terrain in terrains)
        {
            PaintTexture(terrain, position, size, textureIndex);
        }
    }

    void PaintTexture(Terrain terrain, Vector3 worldPos, float size, int textureIndex)
    {
        TerrainData terrainData = terrain.terrainData;
        int alphaMapWidth = terrainData.alphamapWidth;
        int alphaMapHeight = terrainData.alphamapHeight;
        float[,,] alphaMaps = terrainData.GetAlphamaps(0, 0, alphaMapWidth, alphaMapHeight);

        Vector3 terrainPosition = terrain.transform.position;
        Vector3 relativePosition = worldPos - terrainPosition;

        int xStart = Mathf.FloorToInt(relativePosition.x / terrainData.size.x * alphaMapWidth);
        int zStart = Mathf.FloorToInt(relativePosition.z / terrainData.size.z * alphaMapHeight);
        int xEnd = Mathf.FloorToInt((relativePosition.x + size) / terrainData.size.x * alphaMapWidth);
        int zEnd = Mathf.FloorToInt((relativePosition.z + size) / terrainData.size.z * alphaMapHeight);

        xStart = Mathf.Clamp(xStart, 0, alphaMapWidth - 1);
        zStart = Mathf.Clamp(zStart, 0, alphaMapHeight - 1);
        xEnd = Mathf.Clamp(xEnd, 0, alphaMapWidth - 1);
        zEnd = Mathf.Clamp(zEnd, 0, alphaMapHeight - 1);

        for (int y = zStart; y <= zEnd; y++)
        {
            for (int x = xStart; x <= xEnd; x++)
            {
                float xPos = (float)(x - xStart) / (xEnd - xStart);
                float yPos = (float)(y - zStart) / (zEnd - zStart);
                float distance = Mathf.Sqrt(xPos * xPos + yPos * yPos);
                float brushStrength = Mathf.Clamp01(1.0f - distance);

                for (int i = 0; i < terrainData.alphamapLayers; i++)
                {
                    if (i == textureIndex)
                    {
                        alphaMaps[y, x, i] = Mathf.Max(alphaMaps[y, x, i], brushStrength);
                    }
                    else
                    {
                        alphaMaps[y, x, i] *= 1.0f - brushStrength;
                    }
                }
            }
        }

        terrainData.SetAlphamaps(0, 0, alphaMaps);
    }
}
