using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Viewport : Singleton<Viewport>
{
    float minX;     //ƒJƒƒ‰‹Šp‚Ì”ÍˆÍ

    float maxX;     //ƒJƒƒ‰‹Šp‚Ì”ÍˆÍ

    float minY;     //ƒJƒƒ‰‹Šp‚Ì”ÍˆÍ

    float maxY;     //ƒJƒƒ‰‹Šp‚Ì”ÍˆÍ

    float middleX;  //‰æ–Ê‚Ì’†‰›xÀ•W

    void Start()
    {
        Camera mainCamera = Camera.main;

        Vector2 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f));
        Vector2 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f, 1f));

        middleX = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 0f)).x;//‹ŠpÀ•W‚©‚ç¢ŠEÀ•W‚É•ÏŠ·

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
    /// “G¶¬ˆÊ’u‚ğMaxX‚Æƒ‰ƒ“ƒ_ƒ€‚ÌY²‚Å¶¬‚³‚¹‚é
    /// </summary>
    /// <param name="paddingX">“G‚ÌX•</param>
    /// <param name="paddingY">“G‚ÌY•</param>
    /// <returns></returns>
    public Vector3 RandomEnemySpawnPosition(float paddingX,float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = maxX + paddingX;
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }

    /// <summary>
    /// “G‚ÌˆÚ“®”ÍˆÍ‚ğ‰E”¼•ª‚É—}‚¦‚é
    /// </summary>
    /// <param name="paddingX">“G‚ÌX•</param>
    /// <param name="paddingY">“G‚ÌY•</param>
    /// <returns></returns>
    public Vector3 RandomRightHalfPosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(middleX, maxY - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }

    /// <summary>
    /// g—p‚µ‚È‚¢@“G‚ğ‘S‰æ–Ê“à‚É—}‚¦‚é
    /// </summary>
    /// <param name="paddingX">“G‚ÌX•</param>
    /// <param name="paddingY">“G‚ÌY•</param>
    /// <returns></returns>
    public Vector3 RandomEnemyMovePosition(float paddingX, float paddingY)
    {
        Vector3 position = Vector3.zero;

        position.x = Random.Range(minX + paddingX, maxY - paddingX);
        position.y = Random.Range(minY + paddingY, maxY - paddingY);

        return position;
    }
}
