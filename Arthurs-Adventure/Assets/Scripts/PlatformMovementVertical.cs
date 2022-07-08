using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovementVertical : MonoBehaviour
{
    [SerializeField] float offsetUp = 0, offsetDown = 0, speed = 1;
    [SerializeField] bool hasReachedUp = false, hasReachedDown = false;
    Vector3 startPosition = Vector3.zero;
    [SerializeField] float adjustmentFloat = 0.02f;

    SpriteRenderer platformSprite;
 
    void Awake()
    {
        startPosition = transform.position;
        platformSprite =  FindObjectOfType<SpriteRenderer>();
    }
    
    void FixedUpdate()
    {
        if (!hasReachedUp)
        {
            if (transform.position.y < startPosition.y + offsetUp - adjustmentFloat)
            {
                Move(offsetUp);        
            }
            else if (transform.position.y >= startPosition.y + offsetUp - adjustmentFloat)
            {
                hasReachedUp = true;
                hasReachedDown = false;
            }
        }
        else if (!hasReachedDown)
        {
            if (transform.position.y > startPosition.y + offsetDown + adjustmentFloat)
            {
                Move(offsetDown);
            }
            else if (transform.position.y <= startPosition.y + offsetDown + adjustmentFloat)
            {
                hasReachedUp = false;
                hasReachedDown = true;
            }
        }
    }
 
    void Move(float offset)
    {
        transform.position = Vector3.MoveTowards(transform.position,
                                                new Vector3(transform.position.x,
                                                            startPosition.y  + offset,
                                                            transform.position.z),
                                                speed * Time.deltaTime);
    }
    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
 
        Gizmos.color = Color.red;
 
        float width = GetComponent<SpriteRenderer>().size.x;
        float height = GetComponent<SpriteRenderer>().size.y;
 
        float offsetNegY  = startPosition.y + (height / 2) + offsetDown;
        float offsetPosY  = startPosition.y - (height / 2) + offsetUp;
        float offsetLeftPoint = transform.position.x + (width / 2);
        float offsetRightPoint    = transform.position.x - (width / 2);
        float offsetTransformNegY = transform.position.y - (height / 2) + offsetDown;
        float offsetTransformPosY = transform.position.y + (height / 2) + offsetUp;
        float offsetTransformLeftPoint = transform.position.x + (width / 2);
        float offsetTransformRightPoint = transform.position.x - (width / 2);
 
        Gizmos.DrawLine(new Vector3(offsetLeftPoint, offsetNegY, 0),
                        new Vector3(offsetRightPoint, offsetNegY, 0));
 
        Gizmos.color = Color.green;
 
        Gizmos.DrawLine(new Vector3(offsetLeftPoint, offsetPosY, 0),
                        new Vector3(offsetRightPoint, offsetPosY, 0));
 
        Gizmos.color = Color.blue;
 
        Gizmos.DrawLine(new Vector3(offsetTransformLeftPoint, offsetTransformNegY, 0),
                        new Vector3(offsetTransformRightPoint, offsetTransformNegY, 0));
        Gizmos.DrawLine(new Vector3(offsetTransformLeftPoint, offsetTransformPosY, 0),
                        new Vector3(offsetTransformRightPoint, offsetTransformPosY, 0));
    }
    #endif
}
