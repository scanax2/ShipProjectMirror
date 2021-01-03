using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    [SerializeField] private float strength = 2500;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject sliderObj;
    [SerializeField] private GameObject particleShotePrefab;
    [SerializeField] private Vector2 distance;
    [SerializeField] private float cooldown;
    [SerializeField] private Vector2 angleFix;

    private float currentCooldown;

    private Slider slider;
    private Vector3 firePointPosition;
    private Camera cam;

    private bool isReload;

    private void Start()
    {
        //cam = transform.parent.GetComponentInChildren<Camera>();
        cam = transform.parent.parent.GetComponentInChildren<Camera>();
        currentCooldown = 0;
        if (sliderObj)
        {
            slider = sliderObj.GetComponent<Slider>();
        }
    }

    private void Update()
    {
        MoveCannon();
        if (sliderObj)
        {
            ChangeSlider();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isReload)
        {
            Shoot();
        }
        currentCooldown += Time.deltaTime;
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

    private void ChangeSlider()
    {
        float val;
        try
        {
            val = currentCooldown / cooldown;
        } catch (System.DivideByZeroException e)
        {
            val = 1;
        }
        if (val > 1) val = 1;

        slider.value = val;
    }

    private IEnumerator Reload()
    {
        currentCooldown = 0;

        isReload = true;
        yield return new WaitForSeconds(cooldown);
        isReload = false;
    }

    private void Shoot()
    {
        firePointPosition = transform.GetChild(0).position;

        if (particleShotePrefab)
        {
            Instantiate(particleShotePrefab, firePointPosition, Quaternion.identity);
        }

        var go = Instantiate(ballPrefab, firePointPosition, Quaternion.identity);
        var rigidbody = go.GetComponent<Rigidbody>();

        //Vector3 forwardVector = new Vector3(Mathf.Sin(transform.localRotation.y * angleFix.x), Mathf.Sin(transform.localRotation.x * angleFix.y), Mathf.Cos(transform.localRotation.y)) * strength;
        //Vector3 forwardVector = new Vector3(Mathf.Sin(transform.localRotation.y * angleFix.x), Mathf.Sin(-transform.localRotation.z * angleFix.y), Mathf.Cos(transform.localRotation.y)) * strength;
        Vector3 forwardVector = transform.GetChild(0).forward * strength;
        rigidbody.AddForce(forwardVector, ForceMode.Impulse);

        //parentRigidbody.AddForce(-forwardVector, ForceMode.Impulse);
        StartCoroutine(Reload());
    }
}

