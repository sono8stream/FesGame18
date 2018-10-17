using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KoitanLib;
using UnityEngine.SceneManagement;

public class TestAI : MonoBehaviour {
    private Player player;
    private int playerNo;
    public WayPointNavigationManager wpnm;
    public List<Vector2> route = new List<Vector2>();
    private PlatformerMotor2D platformerMotor2D;
    private StandManager nearestStand;
    private Money targetMoney;
    private Bomb targetBomb;
    private StandManager enemyStand;
    private bool isSearching;
    private int cancelCount;
    private TargetState targetState;
    private Vector3 moneyOriginalPos;
    private Vector3 bombOriginalPos;
    private float bombExplosionSeconds;

    //デバッグ用
    private void OnDrawGizmos()
    {
        int routeLength = route.Count;
        if (routeLength >= 1)
        {
            //色
            Gizmos.color = Color.white;
            switch (player.teamColor)
            {
                case TeamColor.RED:
                    Gizmos.color = Color.red;
                    break;
                case TeamColor.BLUE:
                    Gizmos.color = Color.blue;
                    break;
                case TeamColor.ORANGE:
                    Gizmos.color = new Color(1f, 0.5f, 0);
                    break;
                case TeamColor.GREEN:
                    Gizmos.color = Color.green;
                    break;
            }

            //Gizmos.DrawLine(transform.position, route[0]);
            Vector3 pos = (route[0] + (Vector2)transform.position) / 2;
            var diff = (route[0] - (Vector2)transform.position).normalized;
            var rot = Quaternion.FromToRotation(Vector3.right, diff);
            Gizmos.DrawMesh(InstanceRouteMesh(transform.position,route[0]),pos,rot);

            Gizmos.DrawWireSphere(route[routeLength - 1], 1f);
        }
        if (routeLength >= 2)
        {
            for (int i = 0; i < routeLength - 1; i++)
            {
                //Gizmos.DrawLine(route[i], route[i + 1]);
                Vector3 pos = (route[i] + route[i+1]) / 2;
                var diff = (route[i+1] - route[i]).normalized;
                var rot = Quaternion.FromToRotation(Vector3.right, diff);
                Gizmos.DrawMesh(InstanceRouteMesh(route[i], route[i+1]), pos, rot);
            }
        }
    }

    // Use this for initialization
    void Start () {
        player = GetComponent<Player>();
        playerNo = player.PlayerID;
        platformerMotor2D = GetComponent<PlatformerMotor2D>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        StartCoroutine(SetAIcon());
        DecideAction();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + " scene loaded");
        SetAIController(false);
    }

    // Update is called once per frame
    void Update () {
        //クリックしたらそこに向かう
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 screenToWorldPointPosition = Camera.main.ScreenToWorldPoint(mousePos);
            route = wpnm.SearchShortestRoute(transform.position, screenToWorldPointPosition);
            //デバッグ用
            wpnm.goalPoint = screenToWorldPointPosition;
        }

        //メインルーチン
        if(route.Count!=0){
            cancelCount--;
            if (cancelCount <= 0 
                && platformerMotor2D.motorState == PlatformerMotor2D.MotorState.OnGround) {
                DecideAction();
            }
            switch(targetState){
                case TargetState.Urichi:
                    if(nearestStand.isStand == true){
                        DecideAction();
                    }
                    break;
                case TargetState.Money:
                    if(targetMoney == null)                                       
                    {
                        DecideAction();
                    }
                    else if((targetMoney.transform.position - moneyOriginalPos).magnitude > 4)
                    {
                        DecideAction();
                    }
                    break;

                case TargetState.Bomb:
                    if(targetBomb == null){
                        DecideAction();
                    }
                    else if (targetBomb.owner != null)
                    {
                        DecideAction();
                    }
                    else if((targetBomb.transform.position - bombOriginalPos).magnitude > 4)
                    {
                        DecideAction();
                    }
                    break;
                case TargetState.enemyStand:
                    if(!enemyStand.isStand){
                        DecideAction();
                    }
                    break;

            }
            /// 爆弾が爆発しそうだったら投げる
            if(player.havingItem != null){
                bombExplosionSeconds -= Time.deltaTime;
                //Debug.Log("爆発するまで残り" + bombExplosionSeconds.ToString() + "秒");
                if (bombExplosionSeconds <= 0.5f)
                {
                    //Debug.Log("投げます");
                    StartCoroutine(ButtonA());
                    DecideAction();
                }
            }


            Vector2 toPoint = route[0];
            if((toPoint.x - transform.position.x) > 0){
                MoveHorizontal(1);
            }
            else if((toPoint.x - transform.position.x) < 0){
                MoveHorizontal(-1);
            }
            else{
                MoveHorizontal(0);
            }

            if(Mathf.Abs(toPoint.x - transform.position.x)<4
               && (toPoint.y - transform.position.y)>2
               && platformerMotor2D.motorState == PlatformerMotor2D.MotorState.OnGround){
                StartCoroutine(Jump(0.5f));
            }

            if((toPoint.y - transform.position.y)<-2){
                if(platformerMotor2D.motorState != PlatformerMotor2D.MotorState.OnGround)
                {
                    MoveVertical(1);
                }
            }
            else{
                MoveVertical(0);
            }

            if((toPoint - (Vector2)transform.position).magnitude<2){
                route.RemoveAt(0);
                cancelCount = 60;
                //MoveHorizontal(0);
                if (route.Count == 0)
                {
                    StartCoroutine(ButtonA());
                    DecideAction();
                }
            }
        }
        else{
            MoveHorizontal(0);
            if (!isSearching) StartCoroutine(SearchingAction());
        }
    }

    void DecideAction(){
        cancelCount = 60;
        targetState = TargetState.None;
        enemyStand = SearchEnemyStand();
        nearestStand = SearchUrichi();
        targetBomb = SearchBomb();
        targetMoney = SearchMoney();
        if (nearestStand)
        {
            route = wpnm.SearchShortestRoute(transform.position, nearestStand.transform.position);
            targetState = TargetState.Urichi;
        }
        else{
            if (player.havingItem != null && enemyStand)
            {
                //左側か右側どちらが近いか(あとで実装)
                float RDistance = (Vector3.right * 8 + enemyStand.transform.position - transform.position).magnitude;
                float LDistance = (Vector3.right * -8 + enemyStand.transform.position - transform.position).magnitude;
                float shiftX = RDistance < LDistance ? 8 : -8;
                route = wpnm.SearchShortestRoute(transform.position, enemyStand.transform.position + Vector3.right *shiftX);
                route.Add(enemyStand.transform.position + Vector3.right * shiftX / 1.5f);
                //route = wpnm.SearchShortestRoute(transform.position, enemyStand.transform.position);
                targetState = TargetState.enemyStand;
            }
            else
            {
                if(enemyStand){
                    if (targetBomb && targetMoney)
                    {
                        if (Random.Range(0, 10) > 3)
                        {
                            SetRouteToMoney();
                        }
                        else
                        {
                            SetRouteToBomb();
                        }
                    }
                    else
                    {
                        if (targetBomb)
                        {
                            SetRouteToBomb();
                        }
                        if (targetMoney)
                        {
                            SetRouteToMoney();
                        }
                    }
                }
                else{
                    SetRouteToMoney();
                }
            }
        }



    }

    IEnumerator SearchingAction(){
        isSearching = true;
        DecideAction();
        yield return new WaitForSeconds(1.0f);
        isSearching = false;
    }

    //一番近い売地を検索
    StandManager SearchUrichi(){
        float minCost = float.MaxValue;
        StandManager nearestSM = null;
        foreach(StandManager stand in FindObjectsOfType<StandManager>()){
            if(!stand.isStand){
                float tmpCost = (stand.transform.position - transform.position).magnitude;
                if(tmpCost < minCost){
                    minCost = tmpCost;
                    nearestSM = stand;
                }
            }
        }
        return nearestSM;
    }

    Money SearchMoney(){
        Money goodMoney = null;
        float maxScore = float.MinValue;
        foreach (Money money in FindObjectsOfType<Money>())
        {
            if (money.colorID == -1 || money.colorID == (int)player.teamColor){
                float tmpScore = 0;
                float distanceScore;
                float moneyScore;
                distanceScore = (money.transform.position - transform.position).magnitude;
                moneyScore = money.value/100f;
                //デバッグ
                Debug.Log("距離減点:" + distanceScore + "金点:" + moneyScore);
                tmpScore = money.value - distanceScore;
                if(tmpScore>maxScore){
                    maxScore = tmpScore;
                    goodMoney = money;
                }
            }

        }
        return goodMoney;
    }

    Bomb SearchBomb()
    {
        float minCost = float.MaxValue;
        Bomb searchBomb = null;
        foreach (Bomb bomb in FindObjectsOfType<Bomb>())
        {
            if (bomb.owner == null)
            {
                float tmpCost = (bomb.transform.position - transform.position).magnitude;
                if (tmpCost < minCost)
                {
                    minCost = tmpCost;
                    searchBomb = bomb;
                }
            }
        }
        return searchBomb;
    }

    StandManager SearchEnemyStand()
    {
        float minCost = float.MaxValue;
        StandManager enemySM = null;
        foreach (StandManager stand in FindObjectsOfType<StandManager>())
        {
            if (stand.owner != player)
            {
                float tmpCost = (stand.transform.position - transform.position).magnitude;
                if (tmpCost < minCost)
                {
                    minCost = tmpCost;
                    enemySM = stand;
                }
            }
        }
        return enemySM;
    }

    void SetRouteToMoney(){
        route = wpnm.SearchShortestRoute(transform.position, targetMoney.transform.position);
        targetState = TargetState.Money;
        moneyOriginalPos = targetMoney.transform.position;
    }

    void SetRouteToBomb(){
        route = wpnm.SearchShortestRoute(transform.position, targetBomb.transform.position);
        targetState = TargetState.Bomb;
        bombOriginalPos = targetBomb.transform.position;
        bombExplosionSeconds = 5.0f;
    }

    void MoveHorizontal(float value){
        KoitanInput.AxisTable[playerNo][Axis.L_Horizontal].SetAIValue(value);
    }

    void MoveVertical(float value)
    {
        KoitanInput.AxisTable[playerNo][Axis.L_Vertical].SetAIValue(value);
    }


    void SetAIController(bool available)
    {
        Dictionary<ButtonID, KoitanButton> koitanButtons = KoitanInput.ButtonTable[playerNo];
        foreach (KeyValuePair<ButtonID, KoitanButton> pair in koitanButtons)
        {
            pair.Value.SetIsAI(available);
        }
        Dictionary<Axis, KoitanAxis> koitanAxises = KoitanInput.AxisTable[playerNo];
        foreach (KeyValuePair<Axis, KoitanAxis> pair in koitanAxises)
        {
            pair.Value.SetIsAI(available);
        }
    }

    //コントローラーの接続を取るのに時間がかかるため
    IEnumerator SetAIcon(){
        yield return new WaitForSeconds(1f);
        SetAIController(true);
    }

    IEnumerator Jump(float time){
        KoitanInput.ButtonTable[playerNo][ButtonID.X].SetAIValue(true);
        yield return new WaitForSeconds(time);
        KoitanInput.ButtonTable[playerNo][ButtonID.X].SetAIValue(false);
    }

    IEnumerator ButtonA()
    {
        KoitanInput.ButtonTable[playerNo][ButtonID.A].SetAIValue(true);
        yield return new WaitForSeconds(0.1f);
        KoitanInput.ButtonTable[playerNo][ButtonID.A].SetAIValue(false);
    }

    Mesh InstanceArrowMesh(Vector3 from,Vector3 to){
        var mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            from ,
            to + Vector3.down,
            to + Vector3.up
        };
        mesh.triangles = new int[] {
            0 , 1 , 2 , 2 , 1 , 0  
        };
        mesh.RecalculateNormals();
        return mesh;
    }

    Mesh InstanceRouteMesh(Vector3 from,Vector3 to)
    {
        var mesh = new Mesh();
        float routeLength = (to - from).magnitude;
        mesh.vertices = new Vector3[] {
            new Vector3(routeLength/2,0.05f),
            new Vector3(-routeLength/2,0.05f),
            new Vector3(-routeLength/2,-0.05f),
            new Vector3(routeLength/2,-0.05f),
            new Vector3(0.5f,0),
            new Vector3(-0.25f,0.5f),
            new Vector3(-0.25f,-0.5f)
        };
        mesh.triangles = new int[] {
            0 , 1 , 2 ,
            2 , 3 , 0 ,
            4 , 5 , 6 ,
            2 , 1 , 0 ,
            0 , 3 , 2 ,
            6 , 5 , 4 ,
        };
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        return mesh;
    }

    public enum TargetState{
        None,
        Urichi,
        enemyStand,
        Money,
        Bomb
    }
}
