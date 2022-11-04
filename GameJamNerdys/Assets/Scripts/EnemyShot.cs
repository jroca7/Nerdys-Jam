using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShot : MonoBehaviour
{
    public Transform player_pos;
    public Transform instance_point; //lugar de donde sale la bala
    public GameObject bullet;
    public float time_bullet;
 
    void Start()
    {
        player_pos = GameObject.Find("Player").transform;
    }


    void Update()
    {
        time_bullet += Time.deltaTime;
        if (time_bullet >= 2)
        {
            Instantiate(bullet, instance_point.position, Quaternion.identity);
            time_bullet = 0;
        }

        if (player_pos.position.x > this.transform.position.x)
        {
            this.transform.localScale = new Vector2(-1, 1);
        }
        else
        {
            this.transform.localScale = new Vector2(1, 1);
        }
    }
}
