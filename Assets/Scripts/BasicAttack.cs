using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    private float attackTimer = 0f;
    
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private GameObject bullet;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        
        if(attackTimer >= attackSpeed)
        {
            attackTimer = 0;
            Instantiate(bullet, transform.position, gameObject.transform.rotation);
        }
    }
}
