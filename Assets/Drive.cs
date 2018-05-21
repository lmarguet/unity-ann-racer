using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Drive : MonoBehaviour
{
    public float speed = 50.0F;
    public float rotationSpeed = 150.0F;
    public float VisibleDistance = 200f;


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
        var fDist = VisibleDistance;
        var rDist = VisibleDistance;
        var lDist = VisibleDistance;
        var r45Dist = VisibleDistance;
        var l45Dist = VisibleDistance;

        if (Physics.Raycast(transform.position, transform.forward, out hit, VisibleDistance)){
            fDist = hit.distance;
        }

        if (Physics.Raycast(transform.position, transform.right, out hit, VisibleDistance)){
            rDist = hit.distance;
        }


        if (Physics.Raycast(transform.position, -transform.right, out hit, VisibleDistance)){
            lDist = hit.distance;
        }


        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * transform.up, out hit,
                            VisibleDistance)){
            r45Dist = hit.distance;
        }

        if (Physics.Raycast(transform.position, Quaternion.AngleAxis(45, Vector3.up) * -transform.up, out hit,
                            VisibleDistance)){
            l45Dist = hit.distance;
        }

        var td = new StringBuilder()
                .Append(fDist).Append(",")
                .Append(rDist).Append(",")
                .Append(lDist).Append(",")
                .Append(r45Dist).Append(",")
                .Append(l45Dist).Append(",")
                .Append(translationInput).Append(",")
                .Append(rotationInput).Append(",")
                .ToString();
        
        
    }
}