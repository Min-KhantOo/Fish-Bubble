using UnityEngine;

public class JellyFish : Fish
{
    private Vector2 direction;
    private float timer;

    void Start()
    {
        ChooseDirection();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            ChooseDirection();
        }

        Move(direction);
    }

    void ChooseDirection()
    {
        direction = Random.insideUnitCircle.normalized;
        timer = Random.Range(1f, 3f);
    }
}