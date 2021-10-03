using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("The player's movement speed")]
    public float moveSpeed;

    [Tooltip("The player's jump height")]
    public float jumpHeight = 1;
    //The vector3 that the player moves by each update
    Vector3 movement;

    [Tooltip("The child object that's position is used in ground checks")]
    public Transform groundChecker;
    [Tooltip("The child object that's position is used for the left side of the player in ground checks")]
    public Transform leftEdge;
    [Tooltip("The child object that's position is used for the right side of the player in ground checks")]
    public Transform rightEdge;
    [Tooltip("The child object that's position is used for the middle of the player in ground checks")]
    public Transform playerMiddle;
    public Rigidbody playerRigidbody;

    //So our ground checker only hits objects on the ground layer
    int layerMask;
    void OnMove(InputValue value)
    {
        //Vector3 moveVal = value.Get<Vector2>();
        //movement = new Vector3(moveVal.x * moveSpeed, 0, 0);
        float moveVal = value.Get<float>();
        movement = new Vector3(moveVal * moveSpeed, 0, 0);
    }

    void OnJump()
    {
        ProcessJump(0.75f);
    }

    void OnHighJump()
    {
        ProcessJump(1f);
    }

    void ProcessJump(float jumpHeightModifier)
    {
        if (Physics.Linecast(leftEdge.position, groundChecker.position, layerMask) || Physics.Linecast(rightEdge.position, groundChecker.position, layerMask) || Physics.Linecast(playerMiddle.position, groundChecker.position, layerMask))
        {
            playerRigidbody.AddForce(Vector3.up * jumpHeight * jumpHeightModifier, ForceMode.Impulse);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        layerMask = 1 << LayerMask.NameToLayer("Ground");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(movement * Time.deltaTime);
    }
}
