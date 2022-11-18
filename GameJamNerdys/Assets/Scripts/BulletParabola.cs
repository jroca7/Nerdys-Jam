using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParabola : MonoBehaviour
{
    public GameObject tower;
    public GameObject target;

    public float speed = 5f;

    private float towerX;
    private float targetX;

    private float dist;
    private float nextX;
    private float baseY;
    private float height;
    private void Start()
    {
        tower = GameObject.FindGameObjectWithTag("InstancePoint");
        target = GameObject.FindGameObjectWithTag("Player");

        
    }

    private void Update()
    {
        if(transform.position == target.transform.position)
        {
            Destroy(gameObject);
        }

        shot();
    }

    void shot()
    {
        towerX = tower.transform.position.x;
        targetX = target.transform.position.x;

        dist = targetX - towerX;
        nextX = Mathf.MoveTowards(transform.position.x, targetX, speed * Time.deltaTime);
        baseY = Mathf.Lerp(tower.transform.position.y, target.transform.position.y, (nextX - towerX) / dist);
        height = 2 * (nextX - towerX) * (nextX - targetX) / (-0.15f * dist * dist);

        Vector3 movePosition = new Vector3(nextX, baseY + height, transform.position.z);
        transform.rotation = LookAtTarget(movePosition - transform.position);
        transform.position = movePosition;
    }
    public static Quaternion LookAtTarget(Vector2 rotation)
    {
        return Quaternion.Euler(0, 0, Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
