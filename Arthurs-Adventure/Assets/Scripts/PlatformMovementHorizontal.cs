
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformMovementHorizontal : MonoBehaviour
{
    [SerializeField] float offsetLeft = 0, offsetRight = 0, speed = 1;
    [SerializeField] bool hasReachedRight= false, hasReachedLeft = false;
    Vector3 startPosition = Vector3.zero;
    SpriteRenderer sprite;

    float adjustment;
    
    Rigidbody2D rb2d;
 
    void Awake()
    {
        startPosition = transform.position;
        rb2d = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        adjustment = sprite.bounds.extents.x / 10f;
    }

    void FixedUpdate()
    {
        if (!hasReachedRight)
        {
            if (transform.position.x < startPosition.x + offsetRight - adjustment)
            {
                Move(offsetRight);        
            }
            else if (transform.position.x >= startPosition.x + offsetRight - adjustment)
            {
                hasReachedRight = true;
                hasReachedLeft = false;
            }
        }
        else if (!hasReachedLeft)
        {
            if (transform.position.x > startPosition.x + offsetLeft + adjustment)
            {
                Move(offsetLeft);
            }
            else if (transform.position.x <= startPosition.x + offsetLeft + adjustment)
            {
                hasReachedRight = false;
                hasReachedLeft = true;
            }
        }
    }
 
    void Move(float offset)
    {
        transform.position = Vector3.MoveTowards(transform.position,
                                                new Vector3(startPosition.x + offset,
                                                            transform.position.y,
                                                            transform.position.z),
                                                speed * Time.deltaTime);
    }

    public Vector2 GetVelocity()
    {
        return rb2d.velocity;
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
 
        // Gizmos.color = Color.red;
 
        float width = GetComponent<SpriteRenderer>().size.x;
        float height = GetComponent<SpriteRenderer>().size.y;


        
        float offsetNegX  = startPosition.x + offsetLeft;
        float offsetPosX  = startPosition.x + offsetRight;
        float offsetBottomPoint = transform.position.y + (height / 2);
        float offsetTopPoint    = transform.position.y - (height / 2);
        float offsetTransformNegX = transform.position.x - (width / 2);
        float offsetTransformPosX = transform.position.x + (width / 2); 
        float offsetTransformTopPoint = transform.position.y + (height / 2);
        float offsetTransformBottomPoint = transform.position.y - (height / 2);


        float endX = transform.position.x + offsetRight + (width / 2);
        
 
        // Gizmos.DrawLine(new Vector3(offsetNegX, offsetTopPoint, 0),
        //                 new Vector3(offsetNegX, offsetBottomPoint, 0));
 
        // Gizmos.color = Color.green;
 
        // Gizmos.DrawLine(new Vector3(offsetPosX, offsetTopPoint, 0),
        //                 new Vector3(offsetPosX, offsetBottomPoint, 0));
 
        Gizmos.color = Color.blue;
 
        Gizmos.DrawLine(new Vector3(offsetTransformNegX, offsetTransformBottomPoint, 0),
                        new Vector3(offsetTransformNegX, offsetTransformTopPoint, 0));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(endX, offsetTransformBottomPoint, 0),
                        new Vector3(endX, offsetTransformTopPoint, 0));
    }
    #endif
}
