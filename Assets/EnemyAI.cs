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

    public Animator animator;
    public Transform attackPoint;
	public float attackRange = 0.5f;
	public int attackDamage = 1;
	public LayerMask enemyLayers;

    public float attackRate = 1000000000f;
    float nextAttackTime = 0f;

    void Start() {

        currentHealth = maxHealth;

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null) {
            Debug.LogError("No player found? Panic!");
            return;
        }

        seeker.StartPath (transform.position, target.position, OnPathComplete);

        StartCoroutine (UpdatePath());

    }

    public void TakeDamage(int damage){

        currentHealth-= damage;

        if(currentHealth<=0){
            Die();
        }
    }

    void Die(){
        Debug.Log("Enemy died!");
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }

    void Attack(){
        animator.SetBool("IsAttacking", true);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange,enemyLayers);

		foreach (Collider2D enemy in hitEnemies)
		{
			enemy.GetComponent<Player>().TakeDamage(attackDamage);
		}
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


    void FixedUpdate (){
        if (target == null){
            // TODO: insert a player search here
            return;
        }
        // TODO: always look at player
        if (path == null)
            return;
            if (currentWaypoint >= path.vectorPath.Count){
                if (pathIsEnded){
                   
                    return;
                }

                Debug.Log("End of path reacher");
                animator.SetFloat("Speed",0.0f);
                if(Time.time >= nextAttackTime){
                    Attack();
                    nextAttackTime = Time.time+1f/attackRate;
                }
                pathIsEnded = true;
                return;
            }
            pathIsEnded = false;
            animator.SetFloat("Speed",speed);
            animator.SetBool("IsAttacking", false);
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