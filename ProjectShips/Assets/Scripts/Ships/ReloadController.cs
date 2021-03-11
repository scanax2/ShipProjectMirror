using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* Control:
    -> SPACE - pick
    -> Right Mouse Click - switch direction of arrow rotation
 */

public class ReloadController : MonoBehaviour
{
    [SerializeField] private GameObject boundCircle;
    // Inner circles
    [SerializeField] private GameObject greenCircle;
    [SerializeField] private GameObject redCircle;

    [SerializeField] private float reduceValue;

    [SerializeField] private GameObject arrowClock;
    [SerializeField] private Transform rotationCenter;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private bool clockwise;

    private Image imageGreenCircle;
    private Image imageRedCircle;
    private Image imageBoundCircle;

    [SerializeField] private float timeToAppearModels; // 0.25f default

    private Vector3 defaultPosition;

    private float clockRadius;
    private Vector2 rotateCenter;
    private float angle;

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

        imageGreenCircle.fillOrigin = 2;
        imageRedCircle.fillOrigin = 2;
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

        arrowClock.transform.rotation = new Quaternion(0, 0, 0, arrowClock.transform.rotation.w);
        arrowClock.transform.transform.localPosition = defaultPosition;
    }

    private bool isGreenZoneClicked()
    {
        float angleInDegree = angle * (180 / Mathf.PI);

        // 1 case - startAngle < endAngle
        if (greenRangeStartAngle < greenRangeEndAngle)
        {
            if (angleInDegree >= greenRangeStartAngle && angleInDegree <= greenRangeEndAngle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // 2 case - startAngle < endAngle
        else
        {
            if (angleInDegree <= greenRangeStartAngle && angleInDegree <= greenRangeEndAngle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private IEnumerator CreateArrow(float delay)
    {
        yield return new WaitForSeconds(delay);
        angle = 0;
        inMiniGame = true;
    }

    private void Update()
    {
        if (inMiniGame)
        {
            // Arrow movement logic
            if (clockwise)
            {
                angle += rotateSpeed * Time.deltaTime;
            }
            else if (!clockwise)
            {
                angle -= rotateSpeed * Time.deltaTime;
            }
            var offset = new Vector2(Mathf.Sin(angle) * clockRadius, Mathf.Cos(angle) * clockRadius);

            arrowClock.transform.localPosition = rotateCenter + offset;
            arrowClock.transform.localEulerAngles = new Vector3(0, 0, -angle*(180/Mathf.PI));

            if (angle >= 2 * Mathf.PI)
            {
                angle -= 2 * Mathf.PI;
            }
            else if (angle < 0)
            {
                angle += 2 * Mathf.PI;
            }

            // Handle user input
            if (Input.GetKeyDown("space"))
            {
                if (isGreenZoneClicked())
                {
                    GetComponent<CannonController>().ReduceCooldown(reduceValue);
                }
                // fast reset to start new round
                EndMiniGame();
                StartMiniGame(0f);

                Debug.Log("Arrow angle: " + (angle * (180 / Mathf.PI)).ToString()); 
            }

            // Right mouse click
            if (Input.GetMouseButtonDown(1))
            {
                clockwise = !clockwise;
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

        greenRangeStartAngle = randAngle;
        greenRangeEndAngle = greenRangeStartAngle + 360*greenFillAmount;

        // Shift angles to match with arrow origin
        float tmpStart = greenRangeStartAngle;
        float tmpEnd = greenRangeEndAngle;

        greenRangeStartAngle = 360 - tmpEnd;
        if (greenRangeStartAngle < 0)
        {
            greenRangeStartAngle += 360;
        }
        greenRangeEndAngle = 360 - tmpStart;
        if (greenRangeEndAngle < 0)
        {
            greenRangeEndAngle += 360;
        }

        Debug.Log("Green zone range: " + greenRangeStartAngle.ToString() + "-" + greenRangeEndAngle.ToString());

        StartCoroutine(SmoothImageAppear(0.0f, greenFillAmount, timeToAppear*2, imageGreenCircle));
        imageGreenCircle.fillClockwise = false;
        greenCircle.transform.eulerAngles = new Vector3(0, 0, rotation);

        StartCoroutine(SmoothImageAppear(0.0f, redFillAmount, timeToAppear*2, imageRedCircle));
        imageRedCircle.fillClockwise = true;
        redCircle.transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}
