using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class StabatExtensions{
    //上に行くほうが大変なので補正する係数
    static float yCostCoefficient = 1.5f;

    /// <summary>
    /// すたばと用２点間の距離コスト
    /// 必ず(to - from)の形にする
    /// </summary>
    public static float staMagnitude(this Vector2 vec){
        if(vec.y > 4)
        {
            vec.y *= yCostCoefficient * Mathf.Floor(vec.y/4f);
        }
        return vec.magnitude;
    }

    /// <summary>
    /// すたばと用２点間の距離コスト
    /// 必ず(to - from)の形にする
    /// </summary>
    public static float staMagnitude(this Vector3 vec)
    {
        if (vec.y > 0)
        {
            vec.y *= yCostCoefficient;
        }
        return vec.magnitude;
    }

    //何故か使えません
    /// <summary>
    /// Gizmosで矢印を生成
    /// </summary>
    public static void DrawArrow(this Gizmos gizmos , Vector3 from ,Vector3 to){
        var mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            from ,
            to + Vector3.down,
            to + Vector3.up
        };
        mesh.triangles = new int[] {
            0, 1, 2
        };
        mesh.RecalculateNormals();
        Gizmos.DrawMesh(mesh);
        gizmos.ToString();
    }
}
