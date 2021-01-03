using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private float strength = 500;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Vector2 distance;

    [SerializeField] private Vector2 angleFix;

    private Vector3 firePointPosition;
    private Camera cam;

   

    private void Start()
    {
        //cam = transform.parent.GetComponentInChildren<Camera>();
        cam = transform.parent.parent.GetComponentInChildren<Camera>();
    }

    private void Update()
    {
        MoveCannon();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        }
    }

    private void MoveCannon()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;

        float alphaX = Mathf.Atan2(mousePos.y - screenPos.y, distance.y);
        float alphaY = Mathf.Atan2(mousePos.x - screenPos.x, distance.x);

        //transform.localRotation = new Quaternion(alphaX, alphaY, 0, 1);
        transform.localRotation = new Quaternion(0, alphaY, -alphaX, 1);
    }

    private void Shoot()
    {
        firePointPosition = transform.GetChild(0).position;

        var go = Instantiate(ballPrefab, firePointPosition, Quaternion.identity);
        var rigidbody = go.GetComponent<Rigidbody>();

        //Vector3 forwardVector = new Vector3(Mathf.Sin(transform.localRotation.y * angleFix.x), Mathf.Sin(transform.localRotation.x * angleFix.y), Mathf.Cos(transform.localRotation.y)) * strength;
        //Vector3 forwardVector = new Vector3(Mathf.Sin(transform.localRotation.y * angleFix.x), Mathf.Sin(-transform.localRotation.z * angleFix.y), Mathf.Cos(transform.localRotation.y)) * strength;
        Vector3 forwardVector = transform.GetChild(0).forward * strength;
        rigidbody.AddForce(forwardVector, ForceMode.Impulse);
    }
}

