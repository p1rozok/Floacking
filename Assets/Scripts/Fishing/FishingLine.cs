using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class FishingLine : MonoBehaviour
{
    [SerializeField] private GameObject point1;
    [SerializeField] private LineRenderer fishingLine;

    private GameObject point2;

    private void Start()
    {
        SetPositionOnLineRender(0);
    }

    private void Update()
    {
        SetLine();
    }
    private void SetLine()
    {
        if(point1==null||point2==null)
            return;
        SetPositionOnLineRender(2);
        fishingLine.SetPosition(0, point1.transform.position);
        fishingLine.SetPosition(1, point2.transform.position);
    }   
    
    public void SetPosition(Vector3 position)
    {

        point2.transform.position = position;

    }

    public void SetPoint(GameObject point)
    {
        point2 = point;


    }    
    private void SetPositionOnLineRender(int amountLine)
    {

        fishingLine.positionCount = amountLine;
    }
}