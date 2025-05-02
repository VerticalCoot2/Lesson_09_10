using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{
    public static float delay = 0.5f;

    private NavMeshAgent agent;
    private Animator anim;
    private RaycastHit hit;
    private MySelectable ms;

    private float health;
    private float damage;
    public float Health{ get { return health; } set { health = value; }}
    public float Damage{ get { return damage; } set { damage = value; }}


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        ms = GetComponent<MySelectable>();
        Health = 500;
        Damage = 100;
    }

    void Update()
    {
        Moving();
    }

    private void Moving()
    {
        foreach (var item in MySelectable.allMySelectables)
        {
            if (ms.selected)
            {
                anim.SetFloat("Running", agent.remainingDistance);
                if (Input.GetMouseButtonDown(0))
                {
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
                    {
                        agent.destination = hit.point;
                    }
                }
            }
            else
                anim.SetFloat("Running", 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            agent.destination = other.gameObject.transform.position;
            anim.SetBool("Attack", true);
            StartCoroutine(Attack(other));
        }
    }

    IEnumerator Attack(Collider col)
    {
        while (col != null && col.GetComponent<EnemyController>().Health > 0)
        {
            transform.LookAt(col.transform);
            col.GetComponent<EnemyController>().Health -= Damage;
            yield return new WaitForSeconds(delay * 3f);
        }
        anim.SetBool("Attack", false);
        if (col != null)
        {
            Destroy(col.gameObject, delay);
        }
    }
}
