using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //conePrefabを入れる
    public GameObject conePrefab;
    //スタート地点
    private int startPos = 20;
    //ゴール地点
    private int goalPos = 360;
    //アイテムを出すx方向の範囲（頻度）
    private float posRange = 3.4f;
    // カメラ（Destroyするために使う）のゲームオブジェクトの定義
    private GameObject mainCamObj;
    //unityちゃんの位置を使うために定義
    private GameObject unitychan;
    //（アドバイスにより追加）可変長Listとして宣言
    private List <GameObject> allItemList = new List <GameObject> ();
    //Unitychan の現在位置を管理する変数
    private float unityChanPositionBeforeZ;
    //次回コーン・コイン・車のPrefabを作成する位置の変数
    private float willGeneratePositionZ;

    // Start is called before the first frame update
    void Start()
    {
        //カメラの位置を取得
        mainCamObj = GameObject.FindGameObjectWithTag("MainCamera");
        //unitychanのゲームオブジェクトを格納
        unitychan = GameObject.Find ("unitychan");
        //初期化してUnityちゃんの初期のポジションを格納
        unityChanPositionBeforeZ = this.unitychan.transform.position.z;
        
        willGeneratePositionZ = this.unitychan.transform.position.z + 50f;
        for (unityChanPositionBeforeZ = 15f; unityChanPositionBeforeZ < willGeneratePositionZ; unityChanPositionBeforeZ += 15f )
        {
            ItemGen(unityChanPositionBeforeZ);
        }
        willGeneratePositionZ += 10f;
        Debug.Log("Current Will Genarate PositionZ Before start Update:" + willGeneratePositionZ);
    }

    // Update is called once per frame
    void Update()
    {
        if (unitychan.transform.position.z + 45f <= goalPos && unitychan.transform.position.z + 45f >= willGeneratePositionZ)
        {
            Debug.Log("WillGeneratePositionZ:" + willGeneratePositionZ);
            ItemGen(this.willGeneratePositionZ);
            willGeneratePositionZ += 15f;
         
        }

        //カメラより後方のGameObjectをDestroyする
        for (int i =0 ; i < allItemList.Count ;  i++)
        {
            if (allItemList[i] != null && allItemList[i].transform.position.z < this. mainCamObj.transform.position.z)
            {
                Destroy(allItemList[i]);
                Debug.Log("Destroyed" + allItemList[i].name +" ,Position.z: "+ allItemList[i].transform.position.z);
            }
        }    
    }
    private void ItemGen(float itemCreationPositionZ)
    {
        //Unityちゃんが１５m進むたびに５０m分ずつとる 
        //ここからは変更しない予定
//        for (int i = startPos; i < goalPos ; i += 15)
//        {
             //どのアイテムを出すのかをランダムに設定
            int num = Random.Range (1,11);
            if(num <= 2)
            {
                //コーンをx軸方向に一直線に生成
                for (float j = -1; j <= 1; j += 0.4f)
                {
                    GameObject cone = Instantiate (conePrefab);
                    cone.transform.position = new Vector3( 4 * j, cone.transform.position.y, itemCreationPositionZ );
                    allItemList.Add(cone);
                }
            }
            else
            {
                //レーン毎にアイテムを生成
                for (int j = -1; j <=1; j++)
                {
                    //アイテムの種類を決める
                    int item = Random.Range (1, 11);
                    //アイテムを奥z座標のオフセットをランダムに設定
                    int offsetZ = Random.Range (-5, 6);
                    //６０％のコイン配置：３０％車配置：１０％何もなし
                    if (1 <= item && item <= 6)
                    {
                        //コインを生成
                        GameObject coin = Instantiate (coinPrefab);
                        coin.transform.position = new Vector3 (posRange * j, coin.transform.position.y, itemCreationPositionZ + offsetZ);
                        allItemList.Add(coin);
                    }
                    else if (7 <= item && item <= 9)
                    {
                        //車を生成
                        GameObject car = Instantiate (carPrefab);
                        car.transform.position = new Vector3 (posRange * j, car.transform.position.y, itemCreationPositionZ + offsetZ);
                        allItemList.Add(car);
                    }
                }
            }
    }
}
