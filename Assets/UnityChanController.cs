using System.Collections;
using UnityEngine.UI;
using UnityEngine;


public class UnityChanController : MonoBehaviour
{
    //アニメーションするためのコンポーネントを入れる
    private Animator myAnimator;
    //Unityちゃんを移動させるコンポーネントを入れる（追加）
    private Rigidbody myRigidbody;
    //前方向の速度（追加）
    private float velocityZ = 16f;
    //横方向の速度
    private float velocityX = 10f;
    //上方向の速度
    private float velocityY = 10f;
    //左右の移動できる範囲
    private float movableRange = 3.4f;
    //動きを減速させる係数
    private float coefficient = 0.99f;
    //ゲーム終了の判定
    private bool isEnd = false;
    //ゲーム終了時に表示するテキスト
    private GameObject stateText;
    //スコアを表示するテキスト
    private GameObject scoreText;
    //得点
    private int score = 0;
    //左ボタン押下の判定
    private bool isLButtonDown = false;
    //右ボタン押下の判定
    private bool isRButtonDown = false;
    //ジャンプボタン押下の判定
    private bool isJButtonDown = false;

    // Start is called before the first frame update
    void Start()
    {
        //Animator コンポーネントを取得
        this.myAnimator = GetComponent<Animator>();
        //走るアニメーションを開始
        this.myAnimator.SetFloat ("Speed", 1);
        //Rigidbodyコンポーネントを取得
        this.myRigidbody = GetComponent<Rigidbody>();
        //シーン中のstateTextオブジェクトを取得
        this.stateText = GameObject.Find("GameResultText");
        //得点のscoreText オブジェクトを取得
        this.scoreText = GameObject.Find("ScoreText");
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isEnd)
        {
            this.velocityZ *= this.coefficient;
            this.velocityY *= this.coefficient;
            this.velocityX *= this.coefficient;
            this.myAnimator.speed *= this.coefficient;
        }
        //横方向の入力による速度
        float inputVelocityX = 0;
        //上方向の入力による速度
        float inputVelocityY = 0;
        //Unityちゃんを矢印キーまたはボタンに応じて左右 に移動させる
        if ((Input.GetKey(KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x)
        {
            //左方向への速度を代入
            inputVelocityX =  -this.velocityX;
        }
        else if((Input.GetKey(KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange)
        {
            //右方向への速度を代入
            inputVelocityX = this.velocityX;
        }
        //ジャンプしていない（transform.position.yが0.5以下の状態）時にスペースが押されたらジャンプする動作
        if ((Input.GetKey(KeyCode.Space) ||this.isJButtonDown) && this.transform.position.y < 0.5f)
        {
            this.myAnimator.SetBool("Jump", true);
            //上方向の速度を代入（velocityYは横方向と同じ10f）
            inputVelocityY = this.velocityY;
        }
        else
        {
            //現在のY軸の速度を代入＝上方向には移動しない動作
            inputVelocityY = this.myRigidbody.velocity.y;
        }
        //Jumpステートの場合はJumpにfalseをセットする（追加）
        if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName ("Jump"))
        {
            this.myAnimator.SetBool("Jump", false);
        }
        
        //Unityちゃんに速度を与える
        this.myRigidbody.velocity = new Vector3 ( inputVelocityX , inputVelocityY , velocityZ );
    }
    //トリガーモードで他のオブジェクトと接触した場合の処理
    void OnTriggerEnter(Collider other)
    {
        //障害物に衝突した判定
        if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag")
        {
            this.isEnd = true;
            //stateText にGAME OVERを表示
            this.stateText.GetComponent<Text>().text = "GAME OVER";
        }
        //ゴール地点に到着した場合
        if (other.gameObject.tag == "GoalTag")
        {
            this.isEnd = true;
            //stateText に CLEAR!! を表示
            if(this.score > 150)
            {
                this.stateText.GetComponent<Text>().text = "High Score!!";
            }
            else
            {
                this.stateText.GetComponent<Text>().text = "CLEAR!!";
            }

        }
        if (other.gameObject.tag == "CoinTag")
        {
            //スコアを加算
            this.score += 10;
            //scoreText で獲得した点数を表示
            this.scoreText.GetComponent<Text>().text = "Score "+ this.score + "pt"; 
            //パーティクルを再生
            GetComponent<ParticleSystem> ().Play ();
            //接触したコインのオブジェクトを破棄
            Destroy (other.gameObject);
        }
    }
    //ジャンプボタンを押した場合の処理
    public void GetMyJumpButtonDown()
    {
        this.isJButtonDown = true;
    }
    //ジャンプボタンを話した場合の処理
    public void GetMyJumpButtonUp()
    {
        this.isJButtonDown = false;
    }
    //左ボタンを押し続けた場合の処理
    public void GetMyLeftButtonDown()
    {
        this.isLButtonDown = true;
    }
    //左ボタンを話した場合の処理
    public void GetMyLeftButtonUp()
    {
        this.isLButtonDown = false;
    }
    //右ボタンを押し続けた場合の処理
    public void GetMyRightButtonDown()
    {
        this.isRButtonDown = true;
    }
    public void GetMyRightButtonUp()
    {
        this.isRButtonDown = false;
    }
}