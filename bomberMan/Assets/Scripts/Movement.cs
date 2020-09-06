using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    protected bool IsMoving = false;
    protected Vector2 input;
    protected Vector3 endPosition;
    protected Vector3 startPosition;
    protected float t;
    protected Direction direction;
    protected int currentRow = 0;
    protected int currentCol = 0;
    [SerializeField] private float moveSpeed;

    public virtual void Update()
    {
        if (!IsMoving)
        {
            MovementControl();

            if (input != Vector2.zero)
            {
                StartCoroutine(Move(transform));
            }
        }
    }

    public IEnumerator Move(Transform transform)
    {
        IsMoving = true;
        startPosition = transform.position;
        t = 0;

        endPosition = new Vector3(startPosition.x + input.x,
            startPosition.y + input.y, startPosition.z);

        while (t < 1f)
        {
            t += Time.deltaTime * (moveSpeed);
            transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }
        IsMoving = false;
        input.x = 0;
        input.y = 0;
        direction = Direction.None;
        yield return 0;
    }

    public void MovementControl()
    {
        switch (direction)
        {
            case Direction.Up:
                if (CanMove(Vector2.up))
                {
                    input.y += 1;
                    currentCol++;
                }
                break;
            case Direction.Down:
                if (CanMove(Vector2.down))
                {
                    input.y -= 1;
                    currentCol--;
                }
                break;
            case Direction.Left:
                if (CanMove(Vector2.left))
                {
                    input.x -= 1;
                    currentRow--;
                }
                break;
            case Direction.Right:
                if (CanMove(Vector2.right))
                {
                    input.x += 1;
                    currentRow++;
                }
                break;
        }
    }

    protected bool CanMove(Vector2 _dir)
    {
        RaycastHit hit;
        Physics.Raycast(this.transform.position, _dir, out hit, 1);
        if (hit.collider != null && (hit.collider.tag.Equals("wall") || hit.collider.tag.Equals("block")))
            return false;
        return true;
    }


}
