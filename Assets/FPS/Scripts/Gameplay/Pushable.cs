using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : MonoBehaviour
{
    [SerializeField] float _pushPower = 2.0f;
    [SerializeField] float _weight = 6.0f;
    [SerializeField] float _gravity = -9.8f;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        var body = hit.collider.attachedRigidbody;
        Vector3 force;

        // no rigidbody
        if (body == null || body.isKinematic) { return; }

        // We use gravity and weight to push things down, we use
        // our velocity and push power to push things other directions
        if (hit.moveDirection.y < -0.3)
        {
            force = new Vector3(0, -0.5f, 0) * _gravity * _weight;
        }
        else
        {
            force = hit.controller.velocity * _pushPower;
        }

        // Apply the push
        body.AddForceAtPosition(force, hit.point);
    }
}
