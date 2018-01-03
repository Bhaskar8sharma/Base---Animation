using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockController : MonoBehaviour
{

    public GameObject manager;
    public Vector2 location = Vector2.zero;
    public Vector2 velocity;
    private Rigidbody2D unitrb2d;
    private Vector2 goalPosition = Vector2.zero; 
    private RaycastHit2D[] hits;
    public BoidSpawner limit;

    // Use this for initialization
    void Start()
    {
        unitrb2d = this.GetComponent<Rigidbody2D>();
        manager = transform.parent.gameObject;
        limit = manager.GetComponent<BoidSpawner>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Flock();
        AvoidOthers();
        SetHeadDirection();
        goalPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
    }

    private void SetHeadDirection()
    {
        transform.up = unitrb2d.velocity;
    }

    private Vector2 Seek(Vector2 target)
    {
        return (target - location);

    }

    private Vector2 Align()
    {
        float neighborDistance = limit.neighborDistance;
        Vector2 sum = Vector2.zero;
        hits = Physics2D.CircleCastAll(location, neighborDistance, Vector2.zero);
        int count = 0;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject == this.gameObject) continue;
            if (hit.transform.gameObject.GetComponent<FlockController>().manager == manager)
            {
                count++;
                sum += hit.transform.GetComponent<Rigidbody2D>().velocity;
            }

        }
        if (count > 0)
        {
            sum /= count;
            return (sum - velocity);
        }
        else
        {
            return sum;
        }
    }

    private void AvoidOthers()
    {
        hits = Physics2D.CircleCastAll(location, 1f, Vector2.zero);
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<FlockController>().manager != manager)
            {
                hit.rigidbody.AddForce(hit.transform.position - this.transform.position);
            }
        }
    }

    private Vector2 Cohesion()
    {
        float neighborDistance = limit.neighborDistance;
        Vector2 sum = Vector2.zero;
        hits = Physics2D.CircleCastAll(location, neighborDistance, Vector2.zero);
        int count = 0;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject == this.gameObject) continue;
            if (hit.transform.gameObject.GetComponent<FlockController>().manager == manager)
            {
                count++;
                sum += hit.transform.GetComponent<FlockController>().location;
            }
        }
        if (count > 0)
        {
            sum /= count;
            return Seek(sum);
        }
        else
        {
            return sum;
        }
    }
    private void ApplyForce(Vector2 force)
    {
        if (force.magnitude > limit.maxForce)
        {
            force = force.normalized * manager.GetComponent<BoidSpawner>().maxForce;
        }
        unitrb2d.AddForce(force);


        if (unitrb2d.velocity.magnitude > limit.maxVelocity)
        {
            unitrb2d.velocity = unitrb2d.velocity.normalized * limit.maxVelocity;
        }
        if (Seek(goalPosition).magnitude < 1)
        {
            unitrb2d.drag = 1f;
        }
        else
        {
            unitrb2d.drag = 0;
        }
    }
    private void Flock()
    {
        //goes to seek target
        location = this.transform.position;
        velocity = unitrb2d.velocity;
        Vector2 unitMovement = Vector2.zero;
        Vector2 goal = Vector2.zero;
        Vector2 align = Align();
        Vector2 cohere = Cohesion();
        goal = Seek(goalPosition);
        unitMovement = align + cohere + goal;
        ApplyForce(unitMovement.normalized);
    }
}
