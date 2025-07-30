using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn1; //左クリックで出現するオブジェクト
    [SerializeField] private GameObject objectToSpawn2; //右クリックで出現するオブジェクト

    private GameObject currentObject1; //現在出現しているobjectToSpawn1
    private GameObject currentObject2; //現在出現しているobjectToSpawn2

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) //左クリック
        {
            //すでにオブジェクトが存在していたら削除
            if (currentObject1 != null)
            {
                Destroy(currentObject1);
            }

            //マウス位置を取得
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; // カメラからの適切な距離を設定

            //画面座標をワールド座標に変換
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //新しいオブジェクトを生成
            currentObject1 = Instantiate(objectToSpawn1, worldPosition, Quaternion.identity);
        }

        if (Input.GetMouseButtonDown(1)) // 右クリック
        {
            //すでにオブジェクトが存在していたら削除
            if (currentObject2 != null)
            {
                Destroy(currentObject2);
            }

            //マウス位置を取得
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; //カメラからの適切な距離を設定

            //画面座標をワールド座標に変換
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            //新しいオブジェクトを生成
            currentObject2 = Instantiate(objectToSpawn2, worldPosition, Quaternion.identity);
        }
    }
}
