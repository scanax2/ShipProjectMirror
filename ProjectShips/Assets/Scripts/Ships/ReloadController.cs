using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadController : MonoBehaviour
{
    [SerializeField] private GameObject boundCircle;
    // Inner circles
    [SerializeField] private GameObject greenCircle;
    [SerializeField] private GameObject redCircle;

    [SerializeField] private float reduceValue;

    [SerializeField] private GameObject arrowClock;
    [SerializeField] private Transform rotationCenter;
    [SerializeField] private float clockSpeed;
    [SerializeField] private bool clockwise;

    private Image imageGreenCircle;
    private Image imageRedCircle;
    private Image imageBoundCircle;

    [SerializeField] private float timeToAppearModels; // 0.25f default

    private Vector3 defaultPosition;

    private float clockRadius;
    private float posX, posY;
    private float angle;

    private int clockwiseFactor = 1;
    private bool inMiniGame = false;

    private float greenRangeStartAngle = 0f;
    private float greenRangeEndAngle = 0f;

    private void Start()
    {
        if (!greenCircle || !redCircle || !boundCircle || !arrowClock) {
            return;
        }

        imageGreenCircle = greenCircle.GetComponent<Image>();
        imageRedCircle = redCircle.GetComponent<Image>();
        imageBoundCircle = boundCircle.GetComponent<Image>();

        imageGreenCircle.fillAmount = 0f;
        imageRedCircle.fillAmount = 0f;
        imageBoundCircle.fillAmount = 0f;
        arrowClock.GetComponent<Image>().fillAmount = 0f;

        imageGreenCircle.fillOrigin = 0;
        imageRedCircle.fillOrigin = 0;
        imageBoundCircle.fillOrigin = 0;
        arrowClock.GetComponent<Image>().fillOrigin = 0;

        defaultPosition = arrowClock.transform.transform.localPosition;
    }

    public void InitMiniGame()
    {
        StartMiniGame(timeToAppearModels);
    }

    public void StartMiniGame(float timeToAppear)
    {
        RectTransform rt = boundCircle.GetComponent<RectTransform>();
        clockRadius = (rt.rect.width / 2 - rt.rect.width / 4);


        CreateCircles(timeToAppear);
        StartCoroutine(SmoothImageAppear(0f, 1f, timeToAppear, arrowClock.GetComponent<Image>()));
        StartCoroutine(CreateArrow(timeToAppear*2));

        posX = 0;
        posY = 0;
        angle = Mathf.PI / 2;

        arrowClock.transform.rotation = new Quaternion(0, 0, 0, arrowClock.transform.rotation.w);
        arrowClock.transform.transform.localPosition = defaultPosition;
    }

    public void EndMiniGame()
    {
        inMiniGame = false;
        imageGreenCircle.fillAmount = 0f;
        imageRedCircle.fillAmount = 0f;
        imageBoundCircle.fillAmount = 0f;
        arrowClock.GetComponent<Image>().fillAmount = 0f;

    }

    private bool isGreenZoneClicked()
    {
        float angleInDegree = angle * (180 / Mathf.PI);
        if (angleInDegree >= greenRangeStartAngle && angleInDegree <= greenRangeEndAngle)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator CreateArrow(float delay)
    {
        yield return new WaitForSeconds(delay);
        angle = Mathf.PI / 2;
        inMiniGame = true;
    }

    private void Update()
    {
        if (inMiniGame)
        {
            // Arrow movement logic
            if (clockwise && clockwiseFactor == 1)
            {
                clockwiseFactor = -1;
                arrowClock.transform.Rotate(new Vector3(0, 0, 180));
            }
            else if (!clockwise && clockwiseFactor == -1)
            {
                clockwiseFactor = 1;
                arrowClock.transform.Rotate(new Vector3(0, 0, 180));
            }

            posX = rotationCenter.position.x + Mathf.Cos(angle) * clockRadius * clockwiseFactor;
            posY = rotationCenter.position.y + Mathf.Sin(angle) * clockRadius * clockwiseFactor;

            arrowClock.transform.Rotate(new Vector3(0, 0, Time.deltaTime * clockSpeed * (180 / Mathf.PI) * clockwiseFactor));
            angle += Time.deltaTime * clockSpeed * clockwiseFactor;

            arrowClock.transform.position = new Vector2(posX, posY);
            if (angle >= 2 * Mathf.PI)
            {
                angle = 0;
            }

            // Handle user input
            if (Input.GetKeyDown("space"))
            {
                if (isGreenZoneClicked())
                {
                    GetComponent<CannonController>().ReduceCooldown(reduceValue);
                }
                // fast reset to start new round
                StartMiniGame(0f);

                // Debug.Log("Arrow angle: " + (angle * (180 / Mathf.PI)).ToString()); 
            }
        }

    }

    IEnumerator SmoothImageAppear(float valueStart, float valueEnd, float duration, Image circle)
    {
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            circle.fillAmount = Mathf.Lerp(valueStart, valueEnd, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        circle.fillAmount = valueEnd;
    }
     
    private void CreateCircles(float timeToAppear)
    {
        if (!greenCircle || !redCircle || !boundCircle || !arrowClock)
        {
            return;
        }

        StartCoroutine(SmoothImageAppear(0.0f, 1.0f, timeToAppear, imageBoundCircle));

        float greenFillAmount = Random.Range(0.1f, 0.15f);
        float redFillAmount = 1 - greenFillAmount;
        int randAngle = Random.Range(0, 360);
        float rotation = randAngle;

        greenRangeStartAngle = randAngle - 90; // offset (-90) to synchronize with arrow clock
        if (greenRangeStartAngle < 0) { greenRangeStartAngle += 360;  }

        greenRangeEndAngle = greenRangeStartAngle + (float)(360.0 * greenFillAmount);
        if (greenRangeEndAngle >= 360) { greenRangeEndAngle -= 360; }

        // Debug.Log("Green zone range: " + fixedRandAngle.ToString() + "-" + greenZoneRange.ToString());

        StartCoroutine(SmoothImageAppear(0.0f, greenFillAmount, timeToAppear*2, imageGreenCircle));
        imageGreenCircle.fillClockwise = false;
        greenCircle.transform.eulerAngles = new Vector3(0, 0, rotation);

        StartCoroutine(SmoothImageAppear(0.0f, redFillAmount, timeToAppear*2, imageRedCircle));
        imageRedCircle.fillClockwise = true;
        redCircle.transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
