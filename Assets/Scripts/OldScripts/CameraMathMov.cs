using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMathMov : MonoBehaviour {

    public GameObject center;
    public float r;
    public float y;
    public float p;

    public float secondsNeeded;

    float timer;

    Vector3 originalPosition;
    Vector3 targetPosition;

    public enum Rotations { stop, lLeft, lRight, dLeftUp, dLeftDown, dRightUp, dRightDown }
    Rotations actualRotation;

    // Use this for initialization
    void Start () {
        timer = 0;
        originalPosition = transform.position;

        

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.anyKey)
        {
            timer = 0;

            originalPosition = transform.position;
            targetPosition = transform.position;


            if (Input.GetKeyDown(KeyCode.A)) RotateTo(Rotations.lLeft);
            else if (Input.GetKeyDown(KeyCode.D)) RotateTo(Rotations.lRight);
        }

        if (actualRotation != Rotations.stop)
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            transform.position = Vector3.Slerp(originalPosition, targetPosition, timer / secondsNeeded);
            transform.LookAt(center.transform);
        }

        if (transform.position == targetPosition) actualRotation = Rotations.stop;
    }


    public void RotateTo(Rotations rotationDirection)
    {
        actualRotation = rotationDirection;

        if (rotationDirection == Rotations.lLeft)
        {
            y += 90;
        }
        else if (rotationDirection == Rotations.lRight)
        {
            y -= 90;
        }


        Vector3 cameraDistance = new Vector3(r * Mathf.Cos(y * Mathf.Deg2Rad) * Mathf.Cos(p * Mathf.Deg2Rad),
                                             r * Mathf.Sin(p * Mathf.Deg2Rad),
                                             r * Mathf.Sin(y * Mathf.Deg2Rad) * Mathf.Cos(p * Mathf.Deg2Rad));

        targetPosition = center.transform.position + cameraDistance;


    }
}
