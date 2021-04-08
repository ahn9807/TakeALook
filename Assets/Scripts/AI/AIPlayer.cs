using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPlayer : MonoBehaviour
{
    private enum AIState
    {
        Init,
        Find,
        Chase,
        Hit,
    };

    public enum CHARACTER
    {
        None = -1,
        GIST,
        KAIST,
        UNIST,
        PHONICS
    };

    protected bool grounded = false;
    protected Collider2D collider;

    public int playerState = 1;
    public int playerPosition = 1;
    public int speed;
    public Collider2D ground;
    protected GameController GameController;

    private int position;
    private GameObject[] ball;
    private GameObject targetBall;
    private AIState state;

    private float vspd;
    private float hspd;

    void Start()
    {
        GameController = GameObject.Find("GameController").GetComponent<GameController>();
        if (this.name == "Player1")
            SetState(GameController.GetPlayer1());
        else
            SetState(GameController.GetPlayer2());
        GetComponent<Rigidbody2D>().gravityScale = 0;

        state = AIState.Init;
    }

    private void Update()
    {
        switch(state) {
            case AIState.Init:
                InitUpdate();
                break;
            case AIState.Find:
                FindUpdate();
                break;
            case AIState.Chase:
                ChaseUpdate();
                break;
            case AIState.Hit:
                HitUpdate();
                break;
        }
    }

    private void FixedUpdate()
    {
        //        transform.position = new Vector3(hspd, vspd, -5);
        gameObject.GetComponent<Rigidbody2D>().MovePosition(new Vector2(hspd, vspd));
    }

    void InitUpdate() {
        if (gameObject.transform.position.x > 0)
            position = 1;
        else
            position = -1;

        state = AIState.Find;
    }

    void FindUpdate() {
        targetBall = GetTargetBall();
        if(targetBall != null) {
            state = AIState.Chase;
        }
    }

    void ChaseUpdate() {
        Vector2 targetPos = Vector2.zero;
        targetBall = GetTargetBall();

        if (targetBall == null)
        {
            int rnd_num = Random.Range(0, 2);
            Vector2 desPos;
            switch (rnd_num) {
                case 0:
                    desPos = Vector3.MoveTowards(transform.position, new Vector3(200*position, 0, -5), speed*Time.fixedDeltaTime);
                    hspd = desPos.x;
                    vspd = desPos.y;
                    break;
                case 1:
                    desPos = Vector3.MoveTowards(transform.position, new Vector3(200*position, 0, -5), speed * Time.fixedDeltaTime);
                    hspd = desPos.x;
                    vspd = desPos.y;
                    break;
                case 2:
                    desPos = Vector3.MoveTowards(transform.position, new Vector3(200*position, 0, -5), speed * Time.fixedDeltaTime);
                    hspd = desPos.x;
                    vspd = desPos.y;
                    break;
                }
            return;
        }

        if(position == -1 && targetBall.transform.position.x < gameObject.transform.position.x) {
            //뒤에 있는 경우 처리
            targetPos = new Vector3(500 * position, 0, -5);
        }
        else if(position == 1 && targetBall.transform.position.x > gameObject.transform.position.x) {
            //뒤에 있는 경우 처리
            targetPos = new Vector3(500 * position, 0, -5);
        }
        else {
            //상대적으로 앞에 있는 경우 처리
            targetPos = new Vector3(targetBall.transform.position.x-5, targetBall.transform.position.y-5);
        }

        Vector2 destinationPos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.fixedDeltaTime);
        hspd = destinationPos.x;
        vspd = destinationPos.y;
    }

    void HitUpdate() {

    }

    GameObject GetTargetBall()
    {   
        ball = GameObject.FindGameObjectsWithTag("Bomb");
        GameObject returnval = null;
        float minDis = Mathf.Infinity;
        Vector2 curPos = transform.position;

        List<GameObject> selectedBall = new List<GameObject>();

        foreach(GameObject target in ball) {
            if(target == null) {
                continue;
            }
            if(position == 1 && target.transform.position.y > -60 && target.transform.position.x < 350) {
                selectedBall.Add(target);
                continue;
            }
            if(position == -1 && target.transform.position.y > -60 && target.transform.position.x > -350) {
                selectedBall.Add(target);
                continue;
            }
        }

        foreach(GameObject target in selectedBall) {
            Vector2 targetPos = new Vector3(target.transform.position.x, target.transform.position.y, -5);
            Vector2 directionToTarget = targetPos- curPos;
            float disToTarget = directionToTarget.sqrMagnitude;
            if(disToTarget < minDis) {
                minDis = disToTarget;
                returnval = target;
            }
        }

        return returnval;
    }








    private bool CheckGroudeded()
    {
        return collider.IsTouching(ground);
    }

    public void SetState(int state)
    {
        var Kaist = transform.Find("Kaist").gameObject;
        var Unist = transform.Find("Unist").gameObject;
        var Gist = transform.Find("Gist").gameObject;
        var Phonics = transform.Find("Phonics").gameObject;

        playerState = state;
        if (this.name == "Player1")
        {
            SetPosition(1);
        }
        else
        {
            SetPosition(2);
        }

        string character_name;
        if (state == (int)CHARACTER.GIST)
        {
            character_name = "Gist";
            Destroy(Kaist);
            Destroy(Unist);
            Destroy(Phonics);
            if (playerPosition == 1)
            {
                GameObject.Find("Castle1").GetComponent<CastleController>().SetSpriteCat();
            }
            else if (playerPosition == 2)
            {
                GameObject.Find("Castle2").GetComponent<CastleController>().SetSpriteCat();
            }

        }
        else if (state == (int)CHARACTER.KAIST)
        {
            character_name = "Kaist";
            Destroy(Gist);
            Destroy(Unist);
            Destroy(Phonics);
            if (playerPosition == 1)
            {
                GameObject.Find("Castle1").GetComponent<CastleController>().SetSpriteCat();
            }
            else if (playerPosition == 2)
            {
                GameObject.Find("Castle2").GetComponent<CastleController>().SetSpriteCat();
            }
        }
        else if (state == (int)CHARACTER.PHONICS)
        {
            character_name = "Phonics";
            Destroy(Kaist);
            Destroy(Unist);
            Destroy(Gist);
            if (playerPosition == 1)
            {
                GameObject.Find("Castle1").GetComponent<CastleController>().SetSpriteDog();
            }
            else if (playerPosition == 2)
            {
                GameObject.Find("Castle2").GetComponent<CastleController>().SetSpriteDog();
            }
        }
        else
        {
            character_name = "Unist";
            Destroy(Kaist);
            Destroy(Gist);
            Destroy(Phonics);
            if (playerPosition == 1)
            {
                GameObject.Find("Castle1").GetComponent<CastleController>().SetSpriteDog();
            }
            else if (playerPosition == 2)
            {
                GameObject.Find("Castle2").GetComponent<CastleController>().SetSpriteDog();
            }
        }

        var targetObject = transform.Find(character_name).gameObject;
        collider = targetObject.GetComponent<Collider2D>();
    }

    public void SetPosition(int position)
    {
        this.playerPosition = position;
    }
}
