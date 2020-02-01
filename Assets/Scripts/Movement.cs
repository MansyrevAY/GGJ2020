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
}

public class Movement : MonoBehaviour
{
    public float speed;
    public charInput input;
    public Collider2D borders;
    
    [Range(0,200)]
    public float thrusterMultiplier;
    [Range(0, 200)]
    public float rollMultiplier;

    private Vector2 screenBounds;
    private float objectWidth;
    private float objectHeight;
    private GameObject camera;
    
    private Rigidbody2D robotRigidbody;

    private bool flyingMode;
    public bool FlyingMode
    {
        get { return flyingMode; }
    }


    private void Awake()
    {
        robotRigidbody = GetComponent<Rigidbody2D>();
        objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x;
        objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        screenBounds = camera.GetComponent<Camera>()
            .ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, camera.transform.position.z));
        
    }

    void Start()
    {
        flyingMode = true;
    }

    void FixedUpdate()
    {
        Vector3 movedPosition = Move();    
    }

    private void Update()
    {
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
            Fly(movementVector);
        }

        return comparation;
    }

    private void Roll(Vector3 comparation, Vector2 movementVector)
    {
        if (IsInside(comparation))
            if(rigidbody2D.velocity != movementVector * rollMultiplier) rigidbody2D.velocity = movementVector * rollMultiplier;
    }

    private void Fly(Vector2 moveInput)
    {
        robotRigidbody.AddForce(moveInput * thrusterMultiplier);
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
    }

    private void ChangeToRollMode()
    {
        robotRigidbody.velocity = Vector2.zero;

        GetComponent<Collider2D>().isTrigger = false;
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


