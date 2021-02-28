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

    [SerializeField] private GameObject arrowClock;
    [SerializeField] private Transform rotationCenter;
    [SerializeField] private float clockSpeed;
    [SerializeField] private bool clockwise;

    private Image imageGreenCircle;
    private Image imageRedCircle;
    private Image imageBoundCircle;

    private Vector3 defaultPosition;

    private float clockRadius;
    private float posX, posY;
    private float angle;

    private int clockwiseFactor = 1;
    private bool inMiniGame = false;

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

        defaultPosition = arrowClock.transform.transform.localPosition;
    }

    public void StartMiniGame()
    {
        RectTransform rt = boundCircle.GetComponent<RectTransform>();
        clockRadius = (rt.rect.width / 2 - rt.rect.width / 4);
        Debug.Log(clockRadius);

        CreateCircles();
        StartCoroutine(SmoothCircleAppear(0f, 1f, 0.25f, arrowClock.GetComponent<Image>()));
        StartCoroutine(CreateArrow(0.5f));
    }

    public void EndMiniGame()
    {
        inMiniGame = false;
        imageGreenCircle.fillAmount = 0f;
        imageRedCircle.fillAmount = 0f;
        imageBoundCircle.fillAmount = 0f;
        arrowClock.GetComponent<Image>().fillAmount = 0f;
        posX = 0;
        posY = 0;
        angle = Mathf.PI / 2;
        arrowClock.transform.rotation = new Quaternion(0, 0, 0, arrowClock.transform.rotation.w);
        arrowClock.transform.transform.localPosition = defaultPosition;
    }

    private void DetectClickZone()
    {

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
        }
    }

    IEnumerator SmoothCircleAppear(float valueStart, float valueEnd, float duration, Image circle)
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
     
    private void CreateCircles()
    {
        if (!greenCircle || !redCircle || !boundCircle || !arrowClock)
        {
            return;
        }
        StartCoroutine(SmoothCircleAppear(0.0f, 1.0f, 0.25f, imageBoundCircle));

        float greenFillAmount = Random.Range(0.1f, 0.15f);
        float redFillAmount = 1 - greenFillAmount;
        int fillOrigin = Random.Range(0, 4); // <0,3>, 0-bottom, 1-right, 2-top, 3-left
        float rotation = Random.Range(0, 360) * (Mathf.PI/180);

        bool isClockwise = Random.value > 0.5f;

        StartCoroutine(SmoothCircleAppear(0.0f, greenFillAmount, 0.5f, imageGreenCircle));
        imageGreenCircle.fillOrigin = fillOrigin;
        imageGreenCircle.fillClockwise = isClockwise;
        greenCircle.transform.rotation = new Quaternion(0, 
                                                        0,
                                                        rotation,
                                                        greenCircle.transform.rotation.w);

        StartCoroutine(SmoothCircleAppear(0.0f, redFillAmount, 0.5f, imageRedCircle));
        imageRedCircle.fillOrigin = fillOrigin;
        imageRedCircle.fillClockwise = !isClockwise;
        redCircle.transform.rotation = new Quaternion(0,
                                                      0,
                                                      rotation,
                                                      redCircle.transform.rotation.w);
    }
}
