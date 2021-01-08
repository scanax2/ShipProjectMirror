using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    private Vector4 waveA;
    private Vector4 waveB;
    private Vector4 waveC;
    private Vector4 waveD;

    private MeshRenderer rend;


    private void Awake()
    {
        if (instance == null)
        {
            rend = GetComponent<MeshRenderer>();
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object !");
            Destroy(this);
        }
    }

    private void Update()
    {
        //Pobieranie zmiennych z shadera
        waveA = rend.material.GetVector("Vector4_EBAA9535");
        waveB = rend.material.GetVector("Vector4_EB7AD3C1");
        waveC = rend.material.GetVector("Vector4_F2108763");
        waveD = rend.material.GetVector("Vector4_C2031578");


        //waveA.x *= -1;
        //waveA.y *= -1;
        //waveB.x *= -1;
        //waveB.y *= -1;
        //waveC.x *= -1;
        //waveC.y *= -1;
        //waveD.x *= -1;
        //waveD.y *= -1;
    }

    
    public Vector3 GerstnerWave(Vector3 p, int waveIndex)
    {
        Vector4 wave = new Vector4(0,0,0,0);
        switch (waveIndex)
        {
            case 1:
                wave = waveA;
                break;
            case 2:
                wave = waveB;
                break;
            case 3:
                wave = waveC;
                break;
            case 4:
                wave = waveD;
                break;
        }
    
        float steepness = wave.z;
        float wavelength = wave.w;
        float k = 2 * Mathf.PI / wavelength;
        float c = Mathf.Sqrt(9.8f / k);
        Vector2 d = new Vector2(wave.x, wave.y).normalized;
        float f = k * (Vector2.Dot(d, new Vector2(p.x,p.z)) - c * Time.timeSinceLevelLoad);
        float a = steepness / k;
        return new Vector3(d.x * (a * Mathf.Cos(f)),
                           a * Mathf.Sin(f),
                           d.y * (a * Mathf.Cos(f))); 
    }

    public float GetWaveHeight(float x)
    {
        Vector3 p = new Vector3(x, 0, 0);
        Vector3 gridPoint = p;
        float tmp = 0;
        for (int i = 1; i <= 4; i++)
        {
            p.y -= GerstnerWave(gridPoint, i).y;
        }
        return p.y;
    }

}
