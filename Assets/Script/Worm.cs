using UnityEngine;

public class Worm : Fish
{
    //public float padding = 0.5f;

    private Camera cam;

    private enum Edge
    {
        Bottom,
        Right,
        Top,
        Left
    }

    private Edge currentEdge = Edge.Bottom;

    void Start()
    {
        cam = Camera.main;

        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        // Spawn at bottom-left corner
        transform.position = new Vector3(min.x + padding, min.y + padding, 0);
    }

    void Update()
    {
        Vector3 min = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));
        Vector3 max = cam.ViewportToWorldPoint(new Vector3(1, 1, 0));

        switch (currentEdge)
        {
            case Edge.Bottom:

                Move(Vector2.right);

                if (transform.position.x >= max.x - padding)
                    currentEdge = Edge.Right;

                break;


            case Edge.Right:

                Move(Vector2.up);

                if (transform.position.y >= max.y - padding)
                    currentEdge = Edge.Top;

                break;


            case Edge.Top:

                Move(Vector2.left);

                if (transform.position.x <= min.x + padding)
                    currentEdge = Edge.Left;

                break;


            case Edge.Left:

                Move(Vector2.down);

                if (transform.position.y <= min.y + padding)
                    currentEdge = Edge.Bottom;

                break;
        }
    }
}