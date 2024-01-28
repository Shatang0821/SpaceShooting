using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Viewport : Singleton<Viewport>
{
    float minX;     //カメラ視角の範囲

    float maxX;     //カメラ視角の範囲

    float minY;     //カメラ視角の範囲

    float maxY;     //カメラ視角の範囲

    float middleX;  //画面の中央x座標

    void Start()
    {
        Camera mainCamera = Camera.main;

        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;//視角座標から世界座標に変換
        minX = bottomLeft.x;
        minY = bottomLeft.y;
        maxX = topRight.x;
        maxY = topRight.y;
    }

    public Vector3 PlayerMoveablePosition(Vector3 playerPosition,float paddingx,float paddingy)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Clamp(playerPosition.x, minX + paddingx, maxX - paddingx);
        position.y = Mathf.Clamp(playerPosition.y, minY + paddingy, maxY - paddingy);


        return position;
    }

    /// <summary>
    /// 敵生成位置を画面外とランダムのY軸で生成させる
    /// </summary>
    /// <param name="paddingX">敵のX幅</param>
    /// <param name="paddingY">敵のY幅</param>
    /// <returns></returns>
    public Vector3 RandomEnemySpawnPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;

        //画面のmaxX+自身サイズの半分にすれば画面外で生成させる
        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }

    /// <summary>
    /// 敵の移動範囲を右半分に抑える
    /// </summary>
    /// <param name="paddingX">敵のX幅</param>
    /// <param name="paddingY">敵のY幅</param>
    /// <returns></returns>
    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(middleX, maxX - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
    /// <summary>
    /// 敵の移動範囲を右半分に抑える
    /// </summary>
    /// <param name="padding">敵の幅</param>
    /// <returns>位置情報</returns>
    public Vector3 RandomRightHalfPosition(Vector3 padding)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(middleX, maxX - padding.x);
        position.y = Random.Range(minY + padding.y, maxY - padding.y);

        return position;
    }

    /// <summary>
    /// 使用しない　敵を全画面内に抑える
    /// </summary>
    /// <param name="paddingX">敵のX幅</param>
    /// <param name="paddingY">敵のY幅</param>
    /// <returns></returns>
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(minX + paddingX, maxY - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
}
