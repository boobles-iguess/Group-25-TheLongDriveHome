using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void PickUp(Transform holdPoint)
    {
        rb.useGravity = false; //turn gravity off
        rb.linearVelocity = Vector3.zero; //pick upable object remains still
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(holdPoint); //make pickupable object the child of the hold point
        transform.localPosition = Vector3.zero;
    }

    public void Drop()
    {
        rb.useGravity = true; // turn gravity on
        transform.SetParent(null); // un child the pick upable object
    }

    public void MoveToHoldPoint(Vector3 targetPosition)
    {
        rb.MovePosition(targetPosition);
    }
}
