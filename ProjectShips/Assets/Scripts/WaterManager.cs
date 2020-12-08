using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private MeshFilter meshFilter;

    //public float noiseWalk = 1f;
    //public float noiseStrength = 1f;

    //public float speed = 2f;
    //public float scale = 1f;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        Vector3[] vertices = meshFilter.mesh.vertices;
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i].y = WaveManager.instance.GetWaveHeight(transform.position.x + vertices[i].x);
            //vertices[i].y += Mathf.Sin(Time.time * speed + vertices[i].x + vertices[i].y + vertices[i].z) * scale;
            //vertices[i].y += Mathf.PerlinNoise(vertices[i].x + noiseWalk, vertices[i].x + Mathf.Sin(Time.time * 0.1f)) * noiseStrength;

        }

        meshFilter.mesh.vertices = vertices;
        meshFilter.mesh.RecalculateNormals();
    }
}
