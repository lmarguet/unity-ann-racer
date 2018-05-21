using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

public class Drive : MonoBehaviour
{
    public float speed = 50.0F;
    public float rotationSpeed = 150.0F;
    public float VisibleDistance = 200f;

    private readonly List<string> collectedTrainingData = new List<string>();
    private StreamWriter tdf;

    void Start()
    {
        var path = Application.dataPath + "/training-data.txt";
        tdf = File.CreateText(path);
    }


    private void OnApplicationQuit()
    {
        foreach (var trainingEntry in collectedTrainingData)
        {
            tdf.WriteLine(trainingEntry);
        }

        tdf.Close();
    }

    void Update()
    {
        var translationInput = Input.GetAxis("Vertical") * speed;
        var rotationInput = Input.GetAxis("Horizontal") * rotationSpeed;
 
        var translation = Time.deltaTime * speed * translationInput;
        var rotation = Time.deltaTime * rotationSpeed * rotationInput;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);


//        Debug.DrawLine(transform.position, transform.forward * VisibleDistance, Color.red);
//        Debug.DrawLine(transform.position, transform.right * VisibleDistance, Color.red);

        RaycastHit hit;
        var fDist = 0f;
        var rDist = 0f;
        var lDist = 0f;
        var r45Dist = 0f;
        var l45Dist = 0f;

        if (Physics.Raycast(transform.position, transform.forward, out hit, VisibleDistance))
        {
            fDist = 1 - Round( hit.distance / VisibleDistance);
        }

        if (Physics.Raycast(transform.position, transform.right, out hit, VisibleDistance))
        {
            rDist = 1 - Round( hit.distance / VisibleDistance);
        }


        if (Physics.Raycast(transform.position, -transform.right, out hit, VisibleDistance))
        {
            lDist = 1 - Round( hit.distance / VisibleDistance);
        }


        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(-45, Vector3.up) * transform.right, out hit,
            VisibleDistance))
        {
            r45Dist = 1 - Round( hit.distance / VisibleDistance);
        }

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -transform.right, out hit,
            VisibleDistance))
        {
            l45Dist = 1 - Round( hit.distance / VisibleDistance);
        }

        var td = new StringBuilder()
            .Append(fDist)
            .Append(",")
            .Append(rDist)
            .Append(",")
            .Append(lDist)
            .Append(",")
            .Append(r45Dist)
            .Append(",")
            .Append(l45Dist)
            .Append(",")
            .Append(
                Round(translationInput)
            ).Append(",")
            .Append(
                Round(rotationInput)
            ).Append(",")
            .ToString();

        if (!collectedTrainingData.Contains(td))
        {
            collectedTrainingData.Add(td);
        }
    }


    private float Round(float x)
    {
        return (float) System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2f;
    }
}