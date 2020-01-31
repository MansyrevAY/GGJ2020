using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public charInput input;
    public Collider2D borders;

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = GetVectorForFirstPlayer() * Time.deltaTime * speed;

        Vector3 comparation = transform.position + new Vector3(movementVector.x, movementVector.y, 0);


        
        if(IsInside(comparation))
            transform.Translate(movementVector);
        
    }

    private Vector2 GetVectorForFirstPlayer()
    {
        Vector2 vector2 = Vector2.zero;

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

    private bool IsInside(Vector3 mov)
    {
        bool isInside = true;

        if (mov.x > borders.bounds.center.x + borders.bounds.extents.x &&
            mov.x < borders.bounds.center.x - borders.bounds.extents.x)
            isInside = false;
        if (mov.y > borders.bounds.center.y + borders.bounds.extents.y &&
            mov.y < borders.bounds.center.y - borders.bounds.extents.y)
            isInside = false;

        return isInside;
    }
}

[System.Serializable]
public struct charInput
{
    public int top;
    public int bottom;
    public int left;
    public int right;
}
