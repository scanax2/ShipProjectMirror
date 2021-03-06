using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    [SerializeField] private GameObject waterSplashParticleSystem;
    private Floater floaterObj;
    private bool gone;

    private void Start()
    {
        floaterObj = GetComponentInChildren<Floater>();
        gone = false;
    }

    private void FixedUpdate()
    {
        if (floaterObj.isFloaterUnderwater() && !gone && waterSplashParticleSystem)
        {
            Quaternion rotation = Quaternion.identity;
            rotation.y += 180;
            rotation.z += 180;
            Instantiate(waterSplashParticleSystem, transform.position, rotation);
            gone = true;
            Destroy(gameObject, 1f);
        }
    }
}
