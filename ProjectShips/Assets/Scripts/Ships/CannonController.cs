using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    [SerializeField] private float strength = 500;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float distance = 2500;

    private Vector2 angleFix;

    private Vector3 firePointPosition;
    private Camera cam;

   

    private void Start()
    {
        cam = transform.parent.GetComponentInChildren<Camera>();
        angleFix = new Vector2(10, 5);
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

        float alphaX = Mathf.Atan2(mousePos.y - screenPos.y, distance);
        float alphaY = Mathf.Atan2(mousePos.x - screenPos.x, distance);
 
        transform.localRotation = new Quaternion(alphaX, alphaY, 0, 1);
    }

    private void Shoot()
    {
        firePointPosition = transform.GetChild(0).position;

        var go = Instantiate(ballPrefab, firePointPosition, Quaternion.identity);
        var rigidbody = go.GetComponent<Rigidbody>();

        Vector3 forwardVector = new Vector3(Mathf.Sin(transform.localRotation.y * angleFix.x), Mathf.Sin(transform.localRotation.x * angleFix.y), Mathf.Cos(transform.localRotation.y)) * strength;
        rigidbody.AddForce(forwardVector, ForceMode.Impulse);
    }
}

