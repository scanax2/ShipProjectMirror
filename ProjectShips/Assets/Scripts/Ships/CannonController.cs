using ProjectShips.Ships;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CannonController : MonoBehaviour
{
    public bool UseUserInput = true;

    [SerializeField] private float strength = 2500;
    [SerializeField] private float recoilStrength = 1000;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject sliderObj;
    [SerializeField] private GameObject particleShotePrefab;
    [SerializeField] private float cooldown;

    [SerializeField] private Vector2 distance;

    private ReloadController reloadController;
    private Slider slider;
    private Vector3 firePointPosition;
    private Camera cam;

    private float cooldownElapsed;
    private bool isReload;

    private void Start()
    {
        cam = transform.parent.parent.GetComponentInChildren<Camera>();
        cooldownElapsed = 0;
        if (sliderObj)
        {
            slider = sliderObj.GetComponent<Slider>();
        }
        reloadController = GetComponent<ReloadController>();
    }

    private void Update()
    {
        if (!UseUserInput)
            return;

        MoveCannon();
        if (sliderObj)
        {
            ChangeSlider();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isReload)
        {
            Shoot();
        }
        cooldownElapsed += Time.deltaTime;
    }

    private void MoveCannon()
    {
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);
        Vector3 mousePos = Input.mousePosition;

        float alphaX = Mathf.Atan2(mousePos.y - screenPos.y, distance.y);
        float alphaY = Mathf.Atan2(mousePos.x - screenPos.x, distance.x);

        //transform.localRotation = new Quaternion(alphaX, alphaY, 0, 1);
        transform.localRotation = Quaternion.Euler(0, 90, 0) * new Quaternion(0, alphaY, -alphaX, 1);
    }

    private void ChangeSlider()
    {
        float val;
        try
        {
            val = cooldownElapsed / cooldown;
        } catch (System.DivideByZeroException e)
        {
            val = 1;
        }
        if (val > 1) val = 1;

        slider.value = val;
    }

    public void ReduceCooldown(float value)
    {
        cooldownElapsed += value;
    }

    private IEnumerator Reload()
    {
        cooldownElapsed = 0f;

        isReload = true;
        // 0.1f - 1 tick
        for (;cooldownElapsed < cooldown; cooldownElapsed += 0.1f/2)
        {
            // Debug.Log(cooldownElapsed);
            yield return new WaitForSeconds(0.1f);
        }
        // yield return new WaitForSeconds(cooldown);
        if (reloadController)
            reloadController.EndMiniGame();
        isReload = false;
    }

    public void Shoot()
    {
        firePointPosition = transform.GetChild(0).position;

        if (particleShotePrefab)
        {
            Instantiate(particleShotePrefab, firePointPosition, Quaternion.identity);
        }

        var go = Instantiate(ballPrefab, firePointPosition, Quaternion.identity);

        var rigidbody = go.GetComponent<Rigidbody>();

        Vector3 forwardVector = transform.GetChild(0).forward * strength;
        rigidbody.AddForce(forwardVector, ForceMode.Impulse);

        Vector3 recoilVector = new Vector3(0,0,-recoilStrength);
        Quaternion recoilVectorQuat = transform.parent.parent.rotation;
        recoilVectorQuat.z = -recoilStrength;
        // transform.parent.parent.GetComponent<Rigidbody>().AddTorque(recoilVector, ForceMode.Impulse);
        // transform.parent.parent.rotation = Quaternion.Lerp(transform.parent.parent.rotation, recoilVectorQuat, Time.time * 0.1f);
        Debug.Log(transform.parent.parent.GetComponent<Rigidbody>());

        if (UseUserInput)
        {
            if (reloadController)
            {
                reloadController.InitMiniGame();
            }
            StartCoroutine(Reload());
        }
    }
}

