using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    public int maxCarryProducts;
    public List<GameObject> carryProductList;
    private FixedJoystick fixedJoystick;
    private Vector3 currentDirection = Vector3.zero;
    private float actualSpeed;
    private float currentX = 0;
    private float currentY = 0;
    private readonly float interpolation = 10;
    private Animator anim;

    private void Start()
    {
        fixedJoystick = GameObject.Find("Canvas/Fixed Joystick").GetComponent<FixedJoystick>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (fixedJoystick.input != Vector2.zero)
        {
            Transform camera = Camera.main.transform;

            currentX = Mathf.Lerp(currentX, fixedJoystick.input.x, Time.deltaTime * interpolation);
            currentY = Mathf.Lerp(currentY, fixedJoystick.input.y, Time.deltaTime * interpolation);

            Vector3 direction = camera.forward * currentY + camera.right * currentX;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                currentDirection = Vector3.Slerp(currentDirection, direction, Time.deltaTime * interpolation);

                transform.rotation = Quaternion.LookRotation(currentDirection);
                transform.position += currentDirection * moveSpeed * Time.deltaTime;

                actualSpeed = direction.magnitude;
                anim.SetFloat("MoveSpeed", actualSpeed);
            }

        }
        else if (actualSpeed > 0)
        {
            actualSpeed -= Time.deltaTime * 2;
            if (actualSpeed < 0) actualSpeed = 0;
            anim.SetFloat("MoveSpeed", actualSpeed);
        }
    }
}
