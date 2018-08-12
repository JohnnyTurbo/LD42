using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

    public float startLineSpeed;
    public float secondPhaseLineSpeed;
    public float yInputSensitivity;
    public float timeToMaxSensitivity;
    public float inputLerpMod;

    TrailRenderer mainLineTrailRenderer;
    EdgeCollider2D mainLineEdgeCollider;
    float verticalInput;
    float curInputLevel;
    float lineSpeed;

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
        if(Input.GetKey(KeyCode.UpArrow))
        {
            verticalInput = 1f;
        }
        else if(Input.GetKey(KeyCode.DownArrow))
        {
            verticalInput = -1f;
        }
        else
        {
            verticalInput = 0f;
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
}
