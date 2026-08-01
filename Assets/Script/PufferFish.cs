using UnityEngine;

public class PufferFish : Fish
{
    public float attackDistance = 5f;
    public Transform player;

    private bool isDead = false;


    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("PufferFish found player");
        }
        else
        {
            Debug.Log("PufferFish cannot find player");
        }
    }


    void Update()
    {
        if (isDead || player == null)
            return;


        Vector3 direction =
            (player.position - transform.position).normalized;


        Move(direction);


        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            Attack();
        }
    }


    protected override void Die()
    {
        isDead = true;

        Debug.Log("Puffer fish dead!");

        Destroy(gameObject);
    }


    public override void Attack()
    {
        Debug.Log("Puffer fish shoots spikes!");
    }
}