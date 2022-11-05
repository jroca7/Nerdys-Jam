using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragNShoot : MonoBehaviour
{
    public float power = 10f;
    public Rigidbody2D rb;

    public Vector2 minPower;
    public Vector2 maxPower;

    public TrajectoryLine tl;
    public Transform player_position;

    Camera cam;
    Vector2 force;
    Vector3 startPoint;
    Vector3 endPoint;

    bool activate = false;

    public Movement movement;
    private void Start()
    {
        cam = Camera.main;
        tl = GetComponent<TrajectoryLine>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            startPoint.z = 15;
        }

        //if (Input.GetMouseButton(0))
        //{
        //    Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        //   currentPoint.z = 15;
        //    tl.RenderLine(startPoint, currentPoint);

        //}

        if (Input.GetMouseButtonUp(0) && activate == true)
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            StopCoroutine(DisableMovement(0));
            StartCoroutine(DisableMovement(.1f));

            force = new Vector2(Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x)*-1, Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y)*-1);
            rb.AddForce(force * power, ForceMode2D.Impulse);
            tl.EndLine();
            Time.timeScale = 1f;
            activate = false;
        }

        if (activate == true)
        {
            startPoint = player_position.position;
            startPoint.z = 15;
            Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            currentPoint.z = 15;
            tl.RenderLine(startPoint, currentPoint);
        }
    }
    IEnumerator DisableMovement(float time)
    {
        movement.canMove = false;
        yield return new WaitForSeconds(.5f);
        movement.canMove = true;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("changeJump") && (Input.GetKey("e")))
        {
            activate = true;
            Time.timeScale = 0.1f;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Time.timeScale = 1f;
        activate = false;
        tl.EndLine();

    }

}
