using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

    public float startLineSpeed;
    public float secondPhaseLineSpeed;
    public float powerupSpeed;
    public float yInputSensitivity;
    public float timeToMaxSensitivity;
    public float inputLerpMod;
    public float minLineY, maxLineY;
    public Gradient powerupColor;

    TrailRenderer mainLineTrailRenderer;
    EdgeCollider2D mainLineEdgeCollider;
    float verticalInput;
    float curInputLevel;
    float lineSpeed;
    bool canControlLine;
    
    private void Awake()
    {
        mainLineTrailRenderer = GetComponent<TrailRenderer>();
        mainLineEdgeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Start()
    {
        curInputLevel = 0f;
        lineSpeed = startLineSpeed;
    }

    private void Update()
    {
        //verticalInput = Input.GetAxis("Vertical");
        if (canControlLine)
        {
            if (Input.GetKey(KeyCode.UpArrow) && canControlLine && transform.position.y < maxLineY)
            {
                verticalInput = 1f;
            }
            else if (Input.GetKey(KeyCode.DownArrow) && canControlLine && transform.position.y > minLineY)
            {
                verticalInput = -1f;
            }
            else
            {
                verticalInput = 0f;
            }
        }
        curInputLevel = Mathf.Lerp(curInputLevel, verticalInput, Time.deltaTime * inputLerpMod);
    }

    private void FixedUpdate()
    {
        transform.position += ((lineSpeed * Time.deltaTime) * Vector3.right);
        transform.position += ((yInputSensitivity * curInputLevel * Time.deltaTime) * Vector3.up);
        Vector3[] linePositions = new Vector3[mainLineTrailRenderer.positionCount];
        int numPositionsOnLine = mainLineTrailRenderer.GetPositions(linePositions);
        if (numPositionsOnLine >= 2)
        {
            /*
            Debug.Log("LR[end]: " + linePositions[numPositionsOnLine - 1].ToString() + 
                      " LR[end - 1]: " + linePositions[numPositionsOnLine - 2], gameObject);
            */
            Vector2[] edgeColliderPositions = new Vector2[numPositionsOnLine];
            for(int i = 0; i < numPositionsOnLine; i++)
            {
                Vector2 localPos = transform.InverseTransformPoint(linePositions[i]);
                edgeColliderPositions[i] = localPos;
            }
            mainLineEdgeCollider.points = edgeColliderPositions;
        }
    }

    public void StopLine()
    {
        lineSpeed = 0f;
    }

    public void StartLine(int phaseNumber)
    {
        lineSpeed = (phaseNumber == 1) ? startLineSpeed : secondPhaseLineSpeed;
    }

    public void UsePowerup(float powerupTime)
    {
        StartCoroutine(ActivatePowerup(powerupTime));
    }

    IEnumerator ActivatePowerup(float powerupTime)
    {
        float initLineSpeed = lineSpeed;
        Gradient initColor = mainLineTrailRenderer.colorGradient;

        lineSpeed = powerupSpeed;
        mainLineTrailRenderer.colorGradient = powerupColor;

        yield return new WaitForSeconds(powerupTime);

        lineSpeed = initLineSpeed;
        mainLineTrailRenderer.colorGradient = initColor;
        Debug.Log("End Powerup Speed");
    }

    public void MoveLineTo(float lineHeight)
    {
        canControlLine = false;
        StartCoroutine(MoveLine(lineHeight));
        verticalInput = 0f;
    }

    IEnumerator MoveLine(float lineHeight)
    {
        float startTime = Time.time;
        float moveDuration = 1.5f;
        float endTime = startTime + moveDuration;
        float startHeight = transform.position.y;

        while(Time.time <= endTime)
        {
            float completionPct = (Time.time - startTime) / moveDuration;
            float curHeight = Mathf.Lerp(startHeight, lineHeight, completionPct);
            Vector3 newLineLoc = new Vector3(transform.position.x, curHeight, transform.position.z);
            transform.position = newLineLoc;
            yield return new WaitForFixedUpdate();
        }
        verticalInput = 0f;
    }

    public void GivePlayerControl()
    {
        canControlLine = true;
    }
}
