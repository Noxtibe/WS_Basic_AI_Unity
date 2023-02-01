using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SC_MurderRobotAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private List<GameObject> _targets; // For multiple points

    private int current = 0;

    private GameObject enemy;
    public int delay = 3;
    public float distanceTreshold = 1.0f;

    public bool redTeam = false;
    public bool blueTeam = false;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _targets = GameObject.FindGameObjectsWithTag("Path").ToList();

        // Shuffling the path
        System.Random rand = new System.Random();
        _targets = _targets.OrderBy(e => rand.Next()).ToList();

        MoveToNextTarget();

        if(gameObject.tag == "Blue")
        {
            blueTeam = true;
        }
        ChangeTeamColor(blueTeam);
    }

    void Update()
    {
        if(Vector3.Distance(transform.position, _targets[current].transform.position) <= distanceTreshold)
        {
            current = (current +1)%(_targets.Count);
            Invoke("MoveToNextTarget", delay);
        }

        if(enemy != null)
        {
            _agent.SetDestination(enemy.transform.position);
            if (Vector3.Distance(transform.position, enemy.transform.position) <= distanceTreshold)
            {
                enemy.GetComponent<SC_MurderRobotAI>().Convert();
                enemy = null;
                MoveToNextTarget();
            }
        }
    }

    void MoveToNextTarget()
    {
        _agent.SetDestination(_targets[current].transform.position);
    }

    void Convert()
    {
        enemy = null;

        if(blueTeam)
        {
            blueTeam = false;
            gameObject.tag = "Red";
        }
        else
        {
            redTeam = false;
            gameObject.tag = "Blue";
        }
        ChangeTeamColor(blueTeam);
        
        Invoke("MoveToNextTarget", delay);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(blueTeam)
        {
            if(gameObject.CompareTag("Red"))
            {
                enemy = other.gameObject;
            }
        }
        else
        {
            if(gameObject.CompareTag("Blue"))
            {
                enemy = other.gameObject;
            }
        }
    }

    // My method to change the colors
    void ChangeTeamColor(bool blueTeam)
    {
        if(blueTeam)
        {
            GameObject body = transform.GetChild(0).gameObject;
            for(int i = 0; i < body.transform.childCount; i++)
            {
                body.transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            }
            body.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        } 
        else
        {
            GameObject body = transform.GetChild(0).gameObject;
            for (int i = 0; i < body.transform.childCount; i++)
            {
                body.transform.GetChild(i).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            }
            body.transform.GetChild(1).GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }
}
