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
    // Start is called before the first frame update
    private GameObject mainCamObj;
    //（アドバイスにより追加）可変長Listとして宣言
    private List <GameObject> allItemList = new List <GameObject> ();
    //クラス（参照型）・構造体struct（データ格納する：値型）

    void Start()
    {
        //カメラの位置を取得
        mainCamObj = GameObject.FindGameObjectWithTag("MainCamera");
        //一定の距離ごとにアイテムを自動生成
        //Unityちゃんが１５m進むたびに５０m分ずつとる 
        for (int i = startPos; i < goalPos ; i += 15)
        {
            //どのアイテムを出すのかをランダムに設定
            int num = Random.Range (1,11);
            if(num <= 2)
            {
                //コーンをx軸方向に一直線に生成
                for (float j = -1; j <= 1; j += 0.4f)
                {
                    GameObject cone = Instantiate (conePrefab);
                    cone.transform.position = new Vector3( 4 * j, cone.transform.position.y, i );
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
                        coin.transform.position = new Vector3 (posRange * j, coin.transform.position.y, i + offsetZ);
                        allItemList.Add(coin);
                    }
                    else if (7 <= item && item <= 9)
                    {
                        //車を生成
                        GameObject car = Instantiate (carPrefab);
                        car.transform.position = new Vector3 (posRange * j, car.transform.position.y, i + offsetZ);
                        allItemList.Add(car);

                        //何かの変数もしくは配列にposition.z を格納すべきでは？
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    for (int i =0 ; i < allItemList.Count ;  i++)
    　{
        if (allItemList[i] != null && allItemList[i].transform.position.z < this. mainCamObj.transform.position.z)
        {
            Destroy(allItemList[i]);
            Debug.Log("Destroyed" + allItemList[i].name +" ,Position.z: "+ allItemList[i].transform.position.z);
        }
     }    
    }
}
