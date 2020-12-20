using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* Realizuje wizualizacje fal wody, ale
   kosztem zuzycia CPU, dlatego ja zrobilem shader
   ktory robi to samo i nie uzywam tego skryptu
   shader jest wydajniej na okolo ~700 fps  

   z CPU - ~70-90 fps
   z Shaderem - ~700-900 fps                                */

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class WaterManager : MonoBehaviour
{
    private Vector3[] vertices;
    private Vector3[] dest;

    private MeshFilter meshFilter;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        vertices = meshFilter.mesh.vertices;
        dest = new Vector3[vertices.Length];
    }

    private void Update()
    {

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 gridPoint = vertices[i];
            Vector3 p = gridPoint;
            for (int wave = 1; wave <= 4; wave++)
            {
                p += WaveManager.instance.GerstnerWave(gridPoint, wave);
            }
            dest[i] = p;
        }

        meshFilter.mesh.vertices = dest;
        meshFilter.mesh.RecalculateNormals();
    }
}
