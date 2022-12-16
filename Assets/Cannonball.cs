using UnityEngine;

public class Cannonball : MonoBehaviour
{

    Collider _collider;

    void OnCollisionEnter(Collision other)
    {
        var ship = other.gameObject.GetComponent<Ship>();
        if( ship != null)
            ship.TakeAShot();
        Destroy(gameObject);
    }
}
