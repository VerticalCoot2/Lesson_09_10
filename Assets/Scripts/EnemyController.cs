using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Transform unit;
    private NavMeshAgent ai;
    private Animator anim;

    private float health;
    private float damage;
    public float Health { get { return health; } set { health = value; } }
    public float Damage { get { return damage; } set { damage = value; } }



    void Start()
    {
        ai = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        Health = 500;
        Damage = 100;
    }

    void Update()
    {
        if (MySelectable.allMySelectables.Count > 0)
        {
            if (unit != null && Vector3.Distance(transform.position, unit.position) < 15)
            {
                transform.LookAt(unit);
                if (Vector3.Distance(transform.position, unit.position) > 1.5f)
                    transform.position += transform.forward * ai.speed * Time.deltaTime;
                anim.SetFloat("Running", ai.speed);
            }
            else
                unit = MySelectable.allMySelectables[Random.Range(0, MySelectable.allMySelectables.Count)].transform;
        }
        if (unit == null) anim.SetFloat("Running", 0f);
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "myUnit")
        {
            anim.SetBool("Attack", true);
            StartCoroutine(Attack(collision));
        }
    }
    IEnumerator Attack(Collider col)
    {
        while (col != null && col.GetComponent<UnitController>().Health > 0)
        {
            transform.LookAt(unit);
            col.GetComponent<UnitController>().Health -= Damage;
            yield return new WaitForSeconds(UnitController.delay * 3f);
        }
        anim.SetBool("Attack", false);
        if (col != null)
        {
            Destroy(col.gameObject, UnitController.delay);
            for (int i = 0; i < MySelectable.allMySelectables.Count; i++)
            {
                if (Vector3.Distance(transform.position, col.transform.position) < 5)
                {
                    MySelectable.allMySelectables.Remove(col.gameObject.GetComponent<MySelectable>());
                    MySelectable.currentlySelected.Remove(col.gameObject.GetComponent<MySelectable>());
                }
            }
        }
    }
}
