using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    [Header("WaveA")]
    [Range(-1, 1)]
    public float dirX1 = 1;
    [Range(-1, 1)]
    public float dirY1 = 0;
    [Range(0, 1)]
    public float steepness1 = 0.5f;
    public float wavelength1 = 10f;

    [Header("WaveB")]
    [Range(-1, 1)]
    public float dirX2 = 1;
    [Range(-1, 1)]
    public float dirY2 = 0;
    [Range(0, 1)]
    public float steepness2 = 0.5f;
    public float wavelength2= 10f;

    [Header("WaveC")]
    [Range(-1, 1)]
    public float dirX3 = 1;
    [Range(-1, 1)]
    public float dirY3 = 0;
    [Range(0, 1)]
    public float steepness3 = 0.5f;
    public float wavelength3 = 10f;

    [Header("WaveD")]
    [Range(-1, 1)]
    public float dirX4 = 1;
    [Range(-1, 1)]
    public float dirY4 = 0;
    [Range(0, 1)]
    public float steepness4 = 0.5f;
    public float wavelength4 = 10f;

    private Vector4 waveA;
    private Vector4 waveB;
    private Vector4 waveC;
    private Vector4 waveD;

    private void Awake()
    {
        if (instance == null)
        {
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
        waveA = new Vector4(dirX1, dirY1, steepness1, wavelength1);
        waveB = new Vector4(dirX2, dirY2, steepness2, wavelength2);
        waveC = new Vector4(dirX3, dirY3, steepness3, wavelength3);
        waveD = new Vector4(dirX4, dirY4, steepness4, wavelength4);
        // offset += Time.deltaTime * speed;
    }

    public Vector3 TestWave(Vector3 p)
    {
        float k = 2 * Mathf.PI / wavelength1;
        float f = k * (p.x - Time.timeSinceLevelLoad);
        p.x += steepness1 * Mathf.Cos(f);
        p.y = steepness1 * Mathf.Sin(f);
        
        return p;
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
        for (int i = 1; i <= 4; i++)
        {
            p.y += GerstnerWave(gridPoint, i).y;
        }
        return p.y;
    }
}
