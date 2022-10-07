using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float timeBeforeDestroyed = 5f;

    private float destroyTimer = 0f;

    void Update()
    {
        destroyTimer += Time.deltaTime;
        if (destroyTimer >= timeBeforeDestroyed)
        {
            Destroy(gameObject);
        }

        transform.position += transform.right * (speed * Time.deltaTime);
    }

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this object (2D physics only).
    /// <para></para>
    /// This message is sent to the trigger Collider2D and the Rigidbody2D (if any) that the trigger Collider2D belongs
    /// to, and to the Rigidbody2D (or the Collider2D if there is no Rigidbody2D) that touches the trigger.
    /// <para></para>
    /// Note: Trigger events are only sent if one of the Colliders also has a Rigidbody2D attached.
    /// Trigger events are sent to disabled MonoBehaviours, to allow enabling Behaviours in response to collisions.
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().Health -= damage;
        }

        Destroy(gameObject);
    }
}