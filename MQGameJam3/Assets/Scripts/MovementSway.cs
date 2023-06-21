using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSway : MonoBehaviour
{
    [SerializeField] private float swayAmount;
    [SerializeField] private int frameDelay = 60;

    private Vector3[] prevFramePositions;
    private int currFrame = 0;

    private Vector3 baseOffset;

    private void Start()
    {
        baseOffset = transform.eulerAngles;
        prevFramePositions = new Vector3[frameDelay];
    }

    // Update is called once per frame
    void Update()
    {
        if (currFrame - frameDelay >= 0)
        {
            Vector3 dir = (transform.position - prevFramePositions[(currFrame - frameDelay) % frameDelay]).normalized;
            Vector3 sway = dir * swayAmount;

            transform.eulerAngles = baseOffset + new Vector3(sway.x, sway.x, sway.y);
        }

        prevFramePositions[currFrame % frameDelay] = transform.position;
        currFrame++;
    }
}
