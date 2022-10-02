using System;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int damage = 1;
    
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * (speed * Time.deltaTime);
    }
    

    private void OnTriggerEnter2D(Collider2D col)
    {
        print("Trigger");
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().Health -= damage;
        }
        Destroy(this);
        
    }
}
