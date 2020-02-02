﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct charInput
{
    public int top;
    public int bottom;
    public int left;
    public int right;
    public int switchMode;
    public int fix;
}

[System.Serializable]
public struct thrusters
{
    public ParticleSystem top;
    public ParticleSystem left;
    public ParticleSystem right;
    public ParticleSystem bot;

}

public class Movement : MonoBehaviour
{
    public float speed;
    public charInput input;
    public Collider2D borders;
    public float repairOffset;
    [Range(0,200)]
    public float thrusterMultiplier;
    [Range(0, 200)]
    public float rollMultiplier;
    public thrusters Thrusters;

    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private GameObject camera;
    private Collider2D childShipCollider;
    private Collider2D robotCollider;
    private Rigidbody2D robotRigidbody;
    private GameObject breaches;
    private Rigidbody2D rigidbody2D;
    

    int currentObjectInstance = -1;

    private bool isInside = true;

    private bool flyingMode;
    public bool FlyingMode
    {
        get { return flyingMode; }
    }


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        screenBounds = camera.GetComponent<Camera>()
            .ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));

        foreach (Transform item in borders.transform)
        {
            if (item.gameObject.name.Equals("Ship (1)"))
            {
                childShipCollider = item.gameObject.GetComponent<Collider2D>();
            }
        }

        robotCollider = GetComponent<Collider2D>();

        breaches = GameObject.FindGameObjectWithTag("BreachesParent");
    }

    void Start()
    {
        flyingMode = true;
    }

    void FixedUpdate()
    {
        Vector3 movedPosition = Move();
        SetInside();
        
    }

    private void SetInside()
    {
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.position, 0.1f);

        string res = "";

        foreach (RaycastHit2D item in hit)
        {
            res += item.transform.name + ", ";
        }

        //Debug.Log(res);

        if (res.Contains("Ship (1)"))
            isInside = true;
        else
            isInside = false;
    }

    private void Update()
    {
        SetInside();
        

        GetMode();

    }

    private Vector3 Move()
    {
        Vector2 movementVector = GetVectorForPlayer() * Time.deltaTime * speed;

        
        Vector3 comparation = transform.position + new Vector3(movementVector.x, movementVector.y, 0);

        if (!flyingMode) // rolling
        {
            Roll(comparation, movementVector);
        }
        else // flying
        {
            //transform.Translate(movementVector);
            Fly(movementVector);
        }

        return comparation;
    }

    private void Roll(Vector3 comparation, Vector2 movementVector)
    {
        if (isInside)
            if(GetComponent<Rigidbody2D>().velocity != movementVector * rollMultiplier) GetComponent<Rigidbody2D>().velocity = movementVector * rollMultiplier;
    }

    private void Fly(Vector2 moveInput)
    {
        rigidbody2D.AddForce(moveInput * thrusterMultiplier);
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x * -1 + objectWidth, screenBounds.x - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y * -1 + objectHeight, screenBounds.y - objectHeight);
        transform.position = viewPos;
    }

    private Vector2 GetVectorForPlayer()
    {
        Vector2 vector2 = Vector2.zero;

//      To check the pressed key number
//        if (Input.anyKey)
//        {
//            foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
//            {
//                if (Input.GetKeyDown(kcode))
//                    Debug.Log("KeyCode down: " + (int)kcode);
//            }
//        }

        if (Input.GetKey((KeyCode)input.left))
        {
            Thrusters.left.gameObject.SetActive(true);
            if (!Thrusters.left.isPlaying)
                Thrusters.left.Play();
            vector2.x = -1;
        }
        else
        {
            Thrusters.left.gameObject.SetActive(false);
        }

        if (Input.GetKey((KeyCode)input.right))
        {
            Thrusters.right.gameObject.SetActive(true);
            if (!Thrusters.right.isPlaying)
                Thrusters.right.Play();
            vector2.x = 1;
        }
        else
        {
            Thrusters.right.gameObject.SetActive(false);
        }

        if (Input.GetKey((KeyCode)input.top))
        {
            Thrusters.top.gameObject.SetActive(true);
            if(!Thrusters.top.isPlaying)
                Thrusters.top.Play();
            vector2.y = 1;
        }
        else
        {
            Thrusters.top.gameObject.SetActive(false);
        }

        if (Input.GetKey((KeyCode)input.bottom))
        {
            Thrusters.bot.gameObject.SetActive(true);
            if (!Thrusters.bot.isPlaying)
                Thrusters.bot.Play();
            vector2.y = -1;
        }
        else
        {
            Thrusters.bot.gameObject.SetActive(false);
        }

        return vector2.normalized;
    }

    private void GetMode()
    {
        if (Input.GetKeyDown((KeyCode)input.switchMode))
        {
            if (isInside)
                flyingMode = !flyingMode;
            if (flyingMode)
            {
                Debug.Log("I am flying!");
                ChangeToFlyModel();
            }
            else
            {
                Debug.Log("I am rolling!");
                ChangeToRollMode();
            }
        }
    }

    private void ChangeToFlyModel()
    {
        gameObject.layer = 11;
        //GetComponent<Collider2D>().isTrigger = true;
        //rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    private void ChangeToRollMode()
    {
        rigidbody2D.velocity = Vector2.zero;

        gameObject.layer = 10;
        GetComponent<Collider2D>().isTrigger = false;
        //rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
    }

    private void FixBreach()
    {
        Transform[] breachesPositions = breaches.GetComponentsInChildren<Transform>();

        if(breachesPositions.Length > 1)
        {
            int repaired = 0;
            // Because i = 0 is parent object
            for (int i = 1; i < breachesPositions.Length; i++)
            {
                float robotBreachDistance = Vector3.Distance(breachesPositions[i].position, transform.position);
                Debug.Log(robotBreachDistance);
                if (robotBreachDistance < repairOffset)
                {
                    breachesPositions[i].gameObject.SetActive(false);
                    repaired++;
                }
            }

            Debug.Log($"Repaired: {repaired}");

            foreach (Transform breachesPosition in breachesPositions)
            {
                if (!breachesPosition.gameObject.activeInHierarchy)
                {
                    Debug.Log($"{breachesPosition.position} is destroyed");
                    Destroy(breachesPosition.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("No breaches to repair");
        }

    }

    private bool IsInside(Vector3 mov)
    {
        bool isInside = true;

        if (mov.x > borders.bounds.center.x + borders.bounds.extents.x ||
            mov.x < borders.bounds.center.x - borders.bounds.extents.x)
            isInside = false;
        if (mov.y > borders.bounds.center.y + borders.bounds.extents.y ||
            mov.y < borders.bounds.center.y - borders.bounds.extents.y)
            isInside = false;

        return isInside;
    }
}


