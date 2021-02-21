using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ten skrypt odpowiada za fizyke wody z Rigibody

public class Floater : MonoBehaviour
{
    //public Rigidbody rigidBody;
    public Rigidbody rigidBody;

    public float depthBeforeSubmerged = 1f;
    public float displacementAmount = 3f;
    public float waterDrag = 0.99f;
    public float waterAngularDrag = 0.5f;

    public int floaterCount = 1;

    private float waveHeight;

    [SerializeField] private bool offWaterPhysics;

    private void Start()
    {
        rigidBody = transform.parent.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!offWaterPhysics)
        {
            rigidBody.AddForceAtPosition(Physics.gravity / floaterCount, transform.position, ForceMode.Acceleration);
            waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
            if (transform.position.y < waveHeight)
            {
                float displacementMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerged) * displacementAmount;
                rigidBody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * displacementMultiplier, 0f), transform.position, ForceMode.Acceleration);
                rigidBody.AddForce(displacementMultiplier * -rigidBody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
                rigidBody.AddTorque(displacementMultiplier * -rigidBody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            }
        }
    }

    public bool isFloaterUnderwater()
    {
        waveHeight = WaveManager.instance.GetWaveHeight(transform.position.x);
        if (waveHeight >= transform.position.y)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}
