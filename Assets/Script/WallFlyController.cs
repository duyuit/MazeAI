using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFlyController : MonoBehaviour
{
    public float FlyingSpeed = 1;
    public float DestroyTime = 3.5f;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _gravity = 9.8f * Vector3.down;
    private Vector3 jumpPosition;
    // Start is called before the first frame update
    float timer = 0.0f;
    private bool isVerti;
    bool rotating = false;

    IEnumerator rotateObject(Vector3 eulerAngles, float duration)
    {
        if (rotating)
        {
            yield break;
        }
        rotating = true;

        Vector3 newRot = transform.eulerAngles + eulerAngles;

        Vector3 currentRot = transform.eulerAngles;

        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.eulerAngles = Vector3.Lerp(currentRot, newRot, counter / duration);
            yield return null;
        }
        rotating = false;
    }
    void Start()
    {
 

    }
    bool isDone = false;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > DestroyTime)
        {
            Destroy(gameObject);
            GameController.DeleteWall(jumpPosition, isVerti);
        }
        if (isDone)
            return;
        _velocity += Time.deltaTime * _gravity;
        transform.position += Time.deltaTime * _velocity;
        if(Vector3.Distance(transform.position, jumpPosition) < 0.1)
        {
            GameController.CreateWall(jumpPosition, isVerti);
            transform.position = jumpPosition;
            isDone = true;
        }
       // transform.Rotate(Vector3.forward * 200 * Time.deltaTime, Space.Self);

    }
    public void SetVelocityToJump(Vector3 goToJumpTo, bool isVerti)
    {
        jumpPosition = goToJumpTo;
        var toTarget = goToJumpTo - this.transform.position;
        _velocity = (toTarget - (Mathf.Pow(FlyingSpeed, 2) * 0.5f * _gravity)) / FlyingSpeed;

        this.isVerti = isVerti;
        if (isVerti)
            StartCoroutine(rotateObject(new Vector3(0, 0, 360), FlyingSpeed));
        else
            StartCoroutine(rotateObject(new Vector3(0, 0, 360 + 90), FlyingSpeed));
    }
   
}
