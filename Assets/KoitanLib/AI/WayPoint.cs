using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {
    public Vector2 point;
    public WayPoint[] transition;
    public float[] loadCost;
    public WayPoint parentWayPoint;
    public State state;

    // 止まってるかどうか

    //実コスト
    public float cost = 0;
    //ヒューリスティック・コスト
    public float heuristic = 0;
    //親ノード
    WayPoint parent = null;

    //スコアの計算
    public float GetScore(){
        return cost + heuristic;
    }
    //ヒューリスティック・コストの計算
    public void CalcHeuristic(Vector2 goal){
        heuristic = (goal - point).staMagnitude();
    }
    /// ステータスがNoneかどうか.
    public bool IsNone()
    {
        return state == State.None;
    }
    /// ステータスをOpenにする.
    public void Open(WayPoint parent, float cost)
    {
        this.state = State.Open;
        this.cost = cost;
        this.parent = parent;
    }
    /// ステータスをClosedにする.
    public void Close()
    {
        state = State.Closed;
    }
    ///パスを取得する
    public void GetPath(List<Vector2> pList)
    {
        pList.Add(point);
        if (parent != null)
        {
            parent.GetPath(pList);
        }
    }

    public void CalcCost(){
        loadCost = new float[transition.Length];
        for (int i = 0; i < loadCost.Length; i++)
        {
            Vector3 to = transition[i].transform.position;
            loadCost[i] = (to - transform.position).staMagnitude();
        }
    }

    void OnValidate()
    {
        point = transform.position;
    }

    private void OnDrawGizmos()
    {
        point = transform.position;
        Gizmos.color = new Color(1,1,1,1f);
        Gizmos.DrawSphere(transform.position, 1f);
        foreach (WayPoint wp in transition){
            Vector3 to = wp.transform.position;
            Vector3 direction = (to - transform.position).normalized * 1f;
            //ちょっと重い？
            //float size = (to - transform.position).magnitude;
            //Gizmos.color = new Color(1, 1 - size/4f, 1 - size/4f);
            Gizmos.DrawLine(transform.position + direction, to - direction);
            Gizmos.DrawRay(to - direction, Quaternion.Euler(0, 0, 30) * -direction);
            Gizmos.DrawRay(to - direction, Quaternion.Euler(0, 0, -30) * -direction);
        }
    }

    private void Awake()
    {
        loadCost = new float[transition.Length];
        for (int i = 0; i < loadCost.Length;i++){
            Vector3 to = transition[i].transform.position;
            loadCost[i] = (to - transform.position).magnitude;
        }
    }

    public enum State{
        None,
        Open,
        Closed
    }

}