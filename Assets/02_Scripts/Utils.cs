using UnityEngine;

public class Utils
{
    private static Vector3 position;
    private static Rect levelRect;
    private static Vector2 absPosition;

    public static Vector3 PositionInLevel()
    {
        position = Vector3.zero;

        levelRect = GameManager.Instance.GetCurrentLevelRect();
        position.x = levelRect.x + levelRect.width * Random.value;
        position.y = levelRect.y + levelRect.height * Random.value;
        return position;
    }

    public static Vector3 ConstraintPositionToLevel(Vector3 position)
    {
        levelRect = GameManager.Instance.GetCurrentLevelRect();
        position.x = Mathf.Clamp(position.x, levelRect.x, levelRect.x + levelRect.width);
        position.y = Mathf.Clamp(position.y, levelRect.y, levelRect.y + levelRect.height);
        return position;
    }

    public static Vector3 PositionOutsideLevel()
    {
        absPosition = Vector2.zero;

        int direction = Random.Range(0, 4);
        if (direction == 0) //Left
            absPosition = new Vector2(-0.1f, Random.value);
        if (direction == 1) //Right
            absPosition = new Vector2(1.1f, Random.value);
        if (direction == 0) //Bottom
            absPosition = new Vector2(Random.value, -0.1f);
        if (direction == 1) //Top
            absPosition = new Vector2(Random.value, 1.1f);

        return ProcentageToPositionInLevel(absPosition);
    }

    public static Vector3 PositionOnLevelBorder()
    {
        absPosition = Vector2.zero;

        int direction = Random.Range(0, 4);
        if (direction == 0) //Left
            absPosition = new Vector2(0.1f, Random.Range(0.1f, 0.9f));
        if (direction == 1) //Right
            absPosition = new Vector2(0.9f, Random.Range(0.1f, 0.9f));
        if (direction == 2) //Bottom
            absPosition = new Vector2(Random.Range(0.1f, 0.9f), 0.1f);
        if (direction == 3) //Top
            absPosition = new Vector2(Random.Range(0.1f, 0.9f), 0.9f);

        return ProcentageToPositionInLevel(absPosition);
    }

    public static Vector3 ProcentageToPositionInLevel(Vector2 absPosition)
    {
        return ProcentageToPositionInLevel(absPosition.x, absPosition.y);
    }

    public static Vector3 ProcentageToPositionInLevel(float x, float y)
    {
        position = Vector3.zero;

        levelRect = GameManager.Instance.GetCurrentLevelRect();
        position.x = levelRect.x + levelRect.width * x;
        position.y = levelRect.y + levelRect.height * y;
        return position;
    }
}