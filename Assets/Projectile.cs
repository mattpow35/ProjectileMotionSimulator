using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    // launch variables
    [Range(0.0f, 10.0f)] public float TargetHeightOffsetFromGround;
    public Slider angleSlider;
    public Slider velSlider;
    public Camera camera;

    // state
    private bool bTargetReady;
    private bool bTouchingGround;

    // cache
    private Rigidbody rigid;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public Text distanceText;
    public Text expectedDistanceText;
    public Text marginOfErrorText;
    public Text avgMarginOfErrorText;
    private float distanceTravelled;
    private bool listeningForCollision;
    private float avgMarginOfError;
    private float numberOfLaunches;
    private float currAvgMarginOfError;
    private float expectedDist;
    

   
    // Use this for initialization
    void Start()
    {   
        rigid = GetComponent<Rigidbody>();
        bTargetReady = true;
        bTouchingGround = true;
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        distanceTravelled = 0f;
        listeningForCollision = false;
        numberOfLaunches = 0;
        avgMarginOfError = 0;
        currAvgMarginOfError = 0;
        expectedDist = 0;

        //transform.LookAt(new Vector3(45, 0,0));
    }


    // resets the projectile to its initial position
    void ResetToInitialState()
    {
        rigid.velocity = Vector3.zero;
        this.transform.SetPositionAndRotation(initialPosition, initialRotation);
        distanceTravelled = 0f;
        bTouchingGround = true;
        bTargetReady = false;
    }

    

    // Update is called once per frame
    void Update ()
    {
        var v2 = (float)Math.Pow(velSlider.value, 2);
        var angleRad = angleSlider.value * (Mathf.PI / 180);
        var sin2Theta = (float)Math.Sin(angleRad * 2);
        expectedDist = v2*sin2Theta / 9.81f;
        expectedDistanceText.text = $"Expected Distance Travelled In Simulator: {expectedDist.ToString("0.00")} m";
        //distanceTravelled = (transform.position.z - initialPosition.z);
        distanceText.text = $"Actual Distance Travelled In Simulator: {distanceTravelled.ToString("0.00")} m";
        
        marginOfErrorText.text = $"Margin of Error: {currAvgMarginOfError} m";
        avgMarginOfErrorText.text = $"Average Margin of Error: {avgMarginOfError} m";
        camera.transform.SetPositionAndRotation(new Vector3(camera.transform.position.x, camera.transform.position.y,
            transform.position.z), camera.transform.rotation);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (bTargetReady)
            {
                Launch();
                numberOfLaunches += 1;
                listeningForCollision = true;
            }
            else
            {
                ResetToInitialState();
                listeningForCollision = false;
                bTargetReady = true;
               
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetToInitialState();
        }

        //if (!bTouchingGround && !bTargetReady)
        //{
        //    // update the rotation of the projectile during trajectory motion
        //    transform.rotation = Quaternion.LookRotation(rigid.velocity) * initialRotation;
        //}
    }

    void OnCollisionEnter(Collision collision)
    {
        bTouchingGround = true;
        if (listeningForCollision)
        {
            if (collision.gameObject.name == "floor")
            {
                
                distanceTravelled = transform.position.z - initialPosition.z;
                listeningForCollision = false;
                currAvgMarginOfError = Math.Abs(expectedDist - distanceTravelled);
                avgMarginOfError = (avgMarginOfError + currAvgMarginOfError) / numberOfLaunches;
                
                
                
            }
        }
    }

    void OnCollisionExit()
    {
        bTouchingGround = false;
    }

   
    // launches the object towards the TargetObject with a given LaunchAngle
    void Launch()
    { 
        var angelRad = angleSlider.value * (Mathf.PI / 180);
        var launchAngle = new Vector3(0, (float)Math.Sin(angelRad), (float)Math.Cos(angelRad));
        rigid.velocity = (launchAngle * velSlider.value);
        bTargetReady = false;
    }


}


