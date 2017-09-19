using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidBehaviour : MonoBehaviour {

    
    private Rigidbody2D rb;
    public List<GameObject> affectedBy;

    public float separationDistance = 10.0f;
    public float maxSpeed = 10.0f;

    [Header("Prefabs")]
    public GameObject FloakPrefab;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        affectedBy = new List<GameObject>();
	}


    void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed); //Max speed

    }

    // Update is called once per frame
    void Update () {

        GetInput(); //Doesn't really accurate
        FaceMovement();

        //Flocking
        Separation();
        Aligment();
        Cohesion();


    }

    private void GetInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 click = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 destination = new Vector2(click.x, click.y);

            Vector2 newOne = (destination - new Vector2(transform.position.x, transform.position.y));

            rb.AddForce(newOne);
        }
    }

    private void FaceMovement()
    {
        Vector2 dir = rb.velocity;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Separation()
    {
        foreach (GameObject actual in affectedBy)
        {
            if(Vector2.Distance(new Vector2(transform.position.x,transform.position.y),new Vector2(actual.transform.position.x,actual.transform.position.y))  < separationDistance) //Distance no es lo mas eficiente del mundo pero ¯\_(ツ)_/¯ 
            {
                Vector2 separationVector = new Vector2(transform.position.x - actual.transform.position.x, transform.position.y - actual.transform.position.y);

                float scale = separationVector.magnitude / separationDistance;

                rb.AddForce(separationVector.normalized / scale);
            }
        }
    }

    private void Aligment()
    {
        Vector2 aligmentVector = Vector2.zero;

        if(affectedBy.Count > 0)
        {
            foreach (GameObject actual in affectedBy)
            {
                aligmentVector += actual.GetComponent<Rigidbody2D>().velocity;
            }

            aligmentVector.x = aligmentVector.x / affectedBy.Count;
            aligmentVector.y = aligmentVector.y / affectedBy.Count;

            rb.AddForce(aligmentVector.normalized);
        }

    }

    private void Cohesion()
    {
        Vector2 center = Vector2.zero;

        if (affectedBy.Count > 0)
        {
            foreach (GameObject actual in affectedBy)
            {
                center.x += actual.transform.position.x;
                center.y += actual.transform.position.y;
            }

            center.x = center.x / affectedBy.Count;
            center.y = center.y / affectedBy.Count;


            rb.AddForce((center - new Vector2(transform.position.x, transform.position.y)).normalized);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Boid" && other.isTrigger)
            affectedBy.Add(other.transform.gameObject);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boid" && other.isTrigger)
            affectedBy.Remove(other.transform.gameObject);
    }

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Boid" && other.isTrigger && transform.parent != null)
        {
            if (GetComponentInParent<FlockBehaviour>().flockMembers.Contains(other.gameObject))
            {
                GetComponentInParent<FlockBehaviour>().flockMembers.Add(other.gameObject);
                other.gameObject.transform.parent = transform.parent;
            }
        }
        else if(other.gameObject.tag == "Boid" && other.isTrigger && transform.parent == null)
        {
            GameObject floak = Instantiate(FloakPrefab);
            transform.parent = floak.transform;

            if (!floak.GetComponent<FlockBehaviour>().flockMembers.Contains(transform.gameObject))
                floak.GetComponent<FlockBehaviour>().flockMembers.Add(transform.gameObject);

            if (!floak.GetComponent<FlockBehaviour>().flockMembers.Contains(other.gameObject))
                floak.GetComponent<FlockBehaviour>().flockMembers.Add(other.gameObject);
        }

    }*/
}
