using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
    //Unityちゃんのゲームオブジェクト
    public GameObject unitychan;
    //カメラとUnityちゃんの距離
    private float difference;
    // Start is called before the first frame update
    void Start()
    {
        //Unityちゃんのオブジェクトを取得
        this.unitychan = GameObject.Find("unitychan");
        //Unityちゃんとカメラ位置（Z座標）の差を求める、この場合、Unityちゃんはposition.z=0 でカメラが-10なので距離は10で固定
        this.difference = unitychan.transform.position.z - this.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        //Unityちゃんの位置に合わせてカメラの位置を移動（DifferenceはStartで差分を取得済みなので常に10が入る）
        this.transform.position = new Vector3 (0, this.transform.position.y, this.unitychan.transform.position.z-difference);
    }
}
