using System;
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

    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private GameObject camera;
    private GameObject breaches;
    private Rigidbody2D rigidbody2D;

    private bool flyingMode;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        screenBounds = camera.GetComponent<Camera>()
            .ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));
        breaches = GameObject.FindGameObjectWithTag("BreachesParent");
    }

    void Start()
    {
        flyingMode = true;
    }

    void Update()
    {
        Vector3 movedPosition = Move();

        if (Input.GetKeyDown((KeyCode) input.fix))
        {
            if (IsInside(movedPosition))
                FixBreach();
        }
    
    }

    private Vector3 Move()
    {
        Vector2 movementVector = GetVectorForPlayer() * Time.deltaTime * speed;

        GetMode();

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
        if (IsInside(comparation))
            rigidbody2D.velocity = movementVector * rollMultiplier;
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
            vector2.x = -1;
        }

        if (Input.GetKey((KeyCode)input.right))
        {
            vector2.x = 1;
        }

        if (Input.GetKey((KeyCode)input.top))
        {
            vector2.y = 1;
        }

        if (Input.GetKey((KeyCode)input.bottom))
        {
            vector2.y = -1;
        }

        return vector2.normalized;
    }

    private void GetMode()
    {
        if (Input.GetKeyDown((KeyCode)input.switchMode))
        {
            if(IsInside(transform.position))
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
        GetComponent<Collider2D>().isTrigger = true;
        //rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
    }

    private void ChangeToRollMode()
    {
        rigidbody2D.velocity = Vector2.zero;

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


