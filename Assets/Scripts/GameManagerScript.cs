using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private Transform[] pathPoints;
    [SerializeField] private GameObject circlePrefab;
    private Transform environmentT;


    private void Start()
    {
        environmentT = GameObject.Find("Environment").transform;
        DrawPaths();
    }

    private void DrawPaths()
    {
        for (int i = 0; i < pathPoints.Length; i++)
        {
            if (i % 3 == 0)     //condition for every third number
            {
                Vector2 lastPos = new Vector2(pathPoints[i].position.x, pathPoints[i].position.z);
                for (float t = 0; t <= 1; t += .01f)
                {
                    Vector2 pos = BezierCurve(new Vector2(pathPoints[i].position.x, pathPoints[i].position.z),
                        new Vector2(pathPoints[i + 1].position.x, pathPoints[i + 1].position.z),
                        new Vector2(pathPoints[i + 2].position.x, pathPoints[i + 2].position.z), t);
                    if (Vector2.Distance(lastPos, pos) > .3f)
                    {
                        lastPos = pos;
                        Instantiate(circlePrefab, new Vector3(pos.x, .003f, pos.y), Quaternion.Euler(90, 0, 0), environmentT);
                    }
                }
            }
        }

    }

    private Vector2 BezierCurve(Vector2 a, Vector2 b, Vector2 c, float t)
    {
        Vector2 p0 = Vector2.Lerp(a, b, t);
        Vector2 p1 = Vector2.Lerp(b, c, t);
        return Vector2.Lerp(p0, p1, t);
    }
}
