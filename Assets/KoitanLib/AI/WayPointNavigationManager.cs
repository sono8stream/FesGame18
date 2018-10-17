using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class WayPointNavigationManager : MonoBehaviour {

    public WayPoint[] wayPoints;
    public Vector2 goalPoint;
    public WayPoint nearestPoint;
    public List<WayPoint> openList = null;

    private void Awake()
    {
        wayPoints = GetComponentsInChildren<WayPoint>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        wayPoints = GetComponentsInChildren<WayPoint>();
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(goalPoint, 1f);
        if(nearestPoint!=null){
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(nearestPoint.point, goalPoint);
        }



        //マウスのクリックがあったら処理
        if (Event.current == null || Event.current.type != EventType.MouseUp)
        {
            return;
        }

        //処理中のイベントからマウスの位置取得
        Vector3 mousePosition = Event.current.mousePosition;

        //シーン上の座標に変換
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);

        
        /*
        goalPoint = mousePosition;
        Vector2 startPoint = wayPoints[0].point;
        List<Vector2> shortestRoute = new List<Vector2>();
        shortestRoute = SearchShortestRoute(startPoint, goalPoint);
        Gizmos.color = Color.red;
        for (int i = 0; i < shortestRoute.Count - 1;i++){
            Gizmos.DrawLine(shortestRoute[i], shortestRoute[i + 1]);
        }
        */
    }
#endif

    public WayPoint SearchNearestPoint(Vector2 target,float maxCost){
        float minCost = (target - wayPoints[0].point).staMagnitude();
        WayPoint tmpPoint = wayPoints[0];
        for (int i = 1; i < wayPoints.Length; i++)
        {
            float tmpCost = (target - wayPoints[i].point).staMagnitude();
            if (tmpCost < minCost)
            {
                minCost = tmpCost;
                tmpPoint = wayPoints[i];
            }
        }
        return minCost < maxCost ? tmpPoint : null;
    }

    public WayPoint OpenNode(WayPoint wp,float cost,WayPoint parent){
        //すでにOpenしているので何もしない
        if (wp.state != WayPoint.State.None) return null;
        wp.Open(parent, cost);
        openList.Add(wp);
        return wp;
    }

    public void OpenAround(WayPoint parent){
        if (parent.transition == null) return;
        var transition = parent.transition;
        var cost = parent.cost;
        var loadCost = parent.loadCost;
        for (int i = 0; i < transition.Length;i++){
            OpenNode(transition[i], cost + loadCost[i], parent);
        }
    }

    //最小スコアのノード取得
    public WayPoint SearchMinScoreNodeFromOpenList(){
        // 最小スコア
        float min = float.MaxValue;
        // 最小実コスト
        float minCost = float.MaxValue;
        WayPoint minNode = null;
        foreach (WayPoint wp in openList)
        {
            float score = wp.GetScore();
            if (score > min)
            {
                // スコアが大きい
                continue;
            }
            if (score == min && wp.cost >= minCost)
            {
                // スコアが同じときは実コストも比較する
                continue;
            }

            // 最小値更新.
            min = score;
            minCost = wp.cost;
            minNode = wp;
        }
        return minNode;
    }

    public List<Vector2> SearchShortestRoute(Vector2 start,Vector2 goal){
        //初期化
        openList.Clear();
        foreach (WayPoint way in wayPoints)
        {
            way.state = WayPoint.State.None;
            way.parentWayPoint = null;
            way.CalcCost();
            way.CalcHeuristic(goal);
        }

        wayPoints = GetComponentsInChildren<WayPoint>();
        WayPoint startwp = SearchNearestPoint(start, 20f);
        WayPoint goalwp = SearchNearestPoint(goal, 20f);
        List < Vector2 > shortestList = new List<Vector2>();
        //たどり着けないので抜ける
        if (startwp == null || goalwp == null) return shortestList;
        //スタート
        WayPoint wp = OpenNode(startwp, 0, null);
        openList.Add(wp);
        int cnt = 0;//試行回数。1000回超えたら強制中断
        while(cnt<1000){
            cnt++;
            openList.Remove(wp);
            OpenAround(wp);
            wp = SearchMinScoreNodeFromOpenList();
            if(wp == null){
                //Debug.Log("Not found path.");
                break;
            }
            if(wp == goalwp){
                //ゴールにたどり着いた
                //Debug.Log("Success.");
                openList.Remove(wp);
                wp.GetPath(shortestList);
                shortestList.Reverse();
                break;
            }
        }
        //ゴールの座標を入れる
        shortestList.Add(goal);
        //Debug.Log("試行回数:" + cnt);
        return shortestList;
    }
}
