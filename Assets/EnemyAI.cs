using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]


public class EnemyAI : MonoBehaviour {
    public Transform target;

    public float updateRate = 2f;

    private Seeker seeker;
    private Rigidbody2D rb;

    public Path path;

    public float speed = 300f;
    public ForceMode2D fMode;

    [HideInInspector]
    private bool pathIsEnded = false;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;
    public int maxHealth = 100;
	public int currentHealth;

	public HealthBar healthBar;

    void Start() {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null) {
            Debug.LogError("No player found? Panic!");
            return;
        }

        seeker.StartPath (transform.position, target.position, OnPathComplete);

        StartCoroutine (UpdatePath());

    }

    IEnumerator UpdatePath (){
        if (target == null){
            // insert a player search here
            yield return false;
        }

        seeker.StartPath (transform.position, target.position, OnPathComplete);

        yield return new WaitForSeconds (1f/updateRate);
        StartCoroutine (UpdatePath());

    }

    public void OnPathComplete (Path p) {
        Debug.Log("We got a path. Did it have an error? " + p.error);
        if(!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    void TakeDamage(int damage)
	{
		currentHealth -= damage;

		healthBar.SetHealth(currentHealth);
	}

    void FixedUpdate (){
        if (target == null){
            // TODO: insert a player search here
            return;
        }
        // TODO: always look at player
        if (path == null)
            return;
            if (currentWaypoint >= path.vectorPath.Count){
                if (pathIsEnded)
                    return;

                Debug.Log("End of path reacher");
                pathIsEnded = true;
                return;
            }
            pathIsEnded = false;

            // Direction to the next waypoint
            Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
            dir *= speed * Time.fixedDeltaTime;

            // move the AI
            rb.AddForce (dir, fMode);
            float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if(dist < nextWaypointDistance){
                currentWaypoint++;
                return;
            }
    }
}