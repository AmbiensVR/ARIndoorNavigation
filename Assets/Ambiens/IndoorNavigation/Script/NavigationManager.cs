using ArchToolkit;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NavigationManager : MonoBehaviour
{

    public Transform targetLocationParent;

    private Transform startingPoint;
    private Transform currentTargetLocation;

    public LineRenderer lineRenderer;
    public NavMeshPath path;

    float elapsed;

    private List<Transform> TargetLocations;

    public RectTransform buttonsParent;
    public GameObject buttonsPrefab;

    public float PathYOffset = 1;

    public void Start()
    {
        path = new NavMeshPath();
        elapsed = 0.0f;

        this.TargetLocations = new List<Transform>();
        foreach (Transform c in this.targetLocationParent)
        {
            if (c != this.targetLocationParent) this.TargetLocations.Add(c);
        }
        
        ArchToolkitManager.Instance.OnVisitorCreated += this.OnVisitorCreated;

    }

    private void OnVisitorCreated()
    {
        Debug.Log("VisitorCreated");
        this.startingPoint = ArchToolkitManager.Instance.visitor.Head.transform;

        foreach(var t in this.TargetLocations)
        {
            var b=GameObject.Instantiate(buttonsPrefab, buttonsParent);
            b.GetComponentInChildren<TextMeshProUGUI>().text = t.gameObject.name;
            b.GetComponent<Button>().onClick.AddListener(() => { this.NavigateTo(t); });
        }
    }

    NavMeshHit hit;
    List<Vector3> corners = new List<Vector3>();
    public void Update()
    {
        

        if (currentTargetLocation == null || startingPoint==null)
        {
            
            lineRenderer.positionCount = 0;
        }
        else
        {
            // Update the way to the goal every second.
            elapsed += Time.deltaTime;
            if (elapsed > 0.5f)
            {
                elapsed -= 0.5f;
                if (NavMesh.SamplePosition(startingPoint.position, out hit, 5.0f, NavMesh.AllAreas))
                {
                    NavMesh.CalculatePath(hit.position, currentTargetLocation.position, NavMesh.AllAreas, path);
                }

                
            }

            lineRenderer.positionCount = path.corners.Length;

            corners.Clear();

            foreach(Vector3 c in path.corners)
            {
                corners.Add(c + Vector3.up * this.PathYOffset);
            }

            lineRenderer.SetPositions(corners.ToArray());
        }
        
            
    }

    public void NavigateTo(Transform target)
    {
        this.currentTargetLocation = target;
    }
}
