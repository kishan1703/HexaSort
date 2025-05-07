using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [Header("Queue Setup")]

    public List<CustStayPoint> queuePoints; // Index 0 = front, last = back
    public List<CustStayPoint> counterPoints; // Index 0 = front, last = back


    [Header("Spawning")]
    public CustomerController customerPrefab;
    public Transform spawnPoint;

    public List<CustomerController> activeCustomers = new List<CustomerController>();
    public int maxCustomerCount = 10;
    public float spawnInterval = 1;
    public SplineComputer path;
    public static Action OnEndReached;
    Action OnRemoveCanFromPath;
    private void Awake()
    {
        Instance = this;

        OnEndReached = (() =>
        {
            activeCustomers.ForEach(i => i.splineFollower.follow = false);
        });

        OnRemoveCanFromPath = (() =>
        {
            activeCustomers.ForEach(i => i.splineFollower.follow = true);
        });
    }
    void Start()
    {
        // SpawnCustomer();
        StartCoroutine(CustomerPool());
    }

    IEnumerator CustomerPool()
    {
        while (activeCustomers.Count < maxCustomerCount)
        {
            SpawnCustomer();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    public void SpawnCustomer()
    {
        CustomerController customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        // customer.wayPoints = CreateCustomerPath();
        customer.splineFollower.spline = path;
        activeCustomers.Add(customer);
    }
    List<Transform> CreateCustomerPath()
    {
        List<CustStayPoint> availablePoints = queuePoints
            .Where(x => !x.isOccupied)
            .ToList();

        if (availablePoints.Count == 0)
            return new List<Transform>();

        availablePoints[0].isOccupied = true;

        List<Transform> reversedPath = availablePoints
        .Select(x => x.transform)
        .ToList();
        reversedPath.Reverse();

        return reversedPath;
    }
    [ContextMenu("RemoveCustomer")]
    public void RemoveCustomer()
    {
        CustStayPoint custStayPoint = counterPoints.Find(x => x.isOccupied == false);
        activeCustomers[0].transform.position = custStayPoint.transform.position;
        activeCustomers.Remove(activeCustomers[0]);
        OnRemoveCanFromPath?.Invoke();
    }
}
