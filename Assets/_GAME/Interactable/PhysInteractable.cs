using _GAME;
using UnityEngine;

public class PhysInteractable : Interactable
{
    private Vector3 force;
    
    public override void BeginInteract(Player player)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    public override void EndInteract()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }
}
