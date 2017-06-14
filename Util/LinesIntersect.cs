using UnityEngine;

public class LinesIntersect
{

    public static bool DoLinesIntersect(LineSeg a, LineSeg b)
    {
		return FasterLineSegmentIntersection(a.a, a.b, b.a, b.b);
    }

    public static bool FasterLineSegmentIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {

        Vector2 a = p2 - p1;
        Vector2 b = p3 - p4;
        Vector2 c = p1 - p3;

        float alphaNumerator = b.y * c.x - b.x * c.y;
        float alphaDenominator = a.y * b.x - a.x * b.y;
        float betaNumerator = a.x * c.y - a.y * c.x;
        float betaDenominator = a.y * b.x - a.x * b.y;

        bool doIntersect = true;

        if (alphaDenominator == 0 || betaDenominator == 0)
        {
            doIntersect = false;
        }
        else
        {

            if (alphaDenominator > 0)
            {
                if (alphaNumerator < 0 || alphaNumerator > alphaDenominator)
                {
                    doIntersect = false;

                }
            }
            else if (alphaNumerator > 0 || alphaNumerator < alphaDenominator)
            {
                doIntersect = false;
            }

            if (doIntersect && betaDenominator > 0)
            {
                if (betaNumerator < 0 || betaNumerator > betaDenominator)
                {
                    doIntersect = false;
                }
            }
            else if (betaNumerator > 0 || betaNumerator < betaDenominator)
            {
                doIntersect = false;
            }
        }

        return doIntersect;
    }
}

[System.Serializable]
public class LineSeg
{
    public Vector2 a;
    public Vector2 b;

    public Color color;

    public LineSeg(Vector2 a, Vector2 b)
    {
        this.a = a;
        this.b = b;
    }

    public LineSeg(Vector3 a, Vector3 b)
    {
        this.a = new Vector2(a.x, a.z);
        this.b = new Vector2(b.x, b.z);
    }

    public void Draw(Color color)
    {
        Debug.DrawLine(new Vector3(a.x, 0, a.y), new Vector3(b.x, 0, b.y), color, 10000.0f, false);
        this.color = color;
    }

}
