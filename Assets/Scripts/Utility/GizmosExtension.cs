using UnityEngine;

public static class GizmosExtension
{
    public static void DrawWireCapsule2D(Vector2 center, Vector2 size, CapsuleDirection2D direction, float angle)
    {
        var oldMatrix = Gizmos.matrix;
        
        // 位置と回転を考慮した行列を作成してmatrixに乗算する
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        var rotationMatrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Gizmos.matrix *= rotationMatrix;

        if (direction == CapsuleDirection2D.Vertical)
        {
            DrawVerticalCapsule(size);
        }
        else
        {
            var matrix90 = Matrix4x4.Rotate(Quaternion.Euler(0, 0, -90f));
            Gizmos.matrix *= matrix90;
            DrawVerticalCapsule(new Vector2(size.y, size.x));
        }

        // matrixを元に戻さないと他のGizmos描画に影響がでるため戻す
        Gizmos.matrix = oldMatrix;
    }

    private static void DrawVerticalCapsule(Vector2 size)
    {
        float radius = size.x / 2f;
        float halfHeight = Mathf.Max(0f, (size.y / 2f) - radius);
        
        DrawWireArc2D(new Vector3(0, halfHeight), radius, 0f, 180f);
        DrawWireArc2D(new Vector3(0, -halfHeight), radius, 0f, -180f);
        Gizmos.DrawLine(new Vector3(-radius, halfHeight), new Vector3(-radius, -halfHeight));
        Gizmos.DrawLine(new Vector3(radius, halfHeight), new Vector3(radius, -halfHeight));
    }

    public static void DrawWireArc2D(Vector2 center, float radius, float startAngle, float endAngle)
    {
        int segments = 32;
        float angleRange = endAngle - startAngle;

        for (int i = 0; i < segments; i++)
        {
            float a1 = (startAngle + angleRange * (float)i / segments) * Mathf.Deg2Rad;
            float a2 = (startAngle + angleRange * (float)(i + 1) / segments) * Mathf.Deg2Rad;

            Gizmos.DrawLine(
                center + new Vector2(Mathf.Cos(a1), Mathf.Sin(a1)) * radius,
                center + new Vector2(Mathf.Cos(a2), Mathf.Sin(a2)) * radius
            );
        }
    }
}