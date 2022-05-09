using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private Animator animator;
     int velocityHash;
    [SerializeField]
    private float velocity;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float deceleration;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        velocityHash = Animator.StringToHash("Velocity");
    }

    private void Update()
    {
        bool forwardPressed =Input.GetKey(KeyCode.W);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);
        if (forwardPressed && velocity < 5.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (!forwardPressed && velocity < 0.0f)
        {
            velocity = 0.0f;
        }
            animator.SetFloat(velocityHash, velocity);
    }
}
