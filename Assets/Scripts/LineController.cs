using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

    public float lineSpeed;
    public float yInputSensitivity;

    TrailRenderer mainLineTrailRenderer;
    EdgeCollider2D mainLineEdgeCollider;
    float verticalInput;

    private void Awake()
    {
        mainLineTrailRenderer = GetComponent<TrailRenderer>();
        mainLineEdgeCollider = GetComponent<EdgeCollider2D>();
    }

    private void Update()
    {
        verticalInput = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        transform.position += ((lineSpeed * Time.deltaTime) * Vector3.right);
        transform.position += ((verticalInput * yInputSensitivity * Time.deltaTime) * Vector3.up);
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

}
