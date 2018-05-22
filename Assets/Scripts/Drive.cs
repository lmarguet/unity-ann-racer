using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using JetBrains.Annotations;

public class Drive : MonoBehaviour
{
    public float speed = 50.0F;
    public float rotationSpeed = 100.0F;
    public float visibleDistance = 200.0f;

    private readonly List<string> collectedTrainingData = new List<string>();
    private StreamWriter tdf;

    void Start()
    {
        var path = Application.dataPath + "/trainingData.txt";
        tdf = File.CreateText(path);
    }

    private void OnApplicationQuit()
    {
        foreach (var td in collectedTrainingData)
        {
            tdf.WriteLine(td);
        }

        tdf.Close();
    }

    void Update()
    {
        var translationInput = Input.GetAxis("Vertical");
        var rotationInput = Input.GetAxis("Horizontal");
        ProcessPlayerInput(translationInput, rotationInput);

        var distances = Utils.PerformRayCasts(transform, visibleDistance);

        var trainingData = ParseToTrainingData(distances, translationInput, rotationInput);

        if (!collectedTrainingData.Contains(trainingData))
        {
            collectedTrainingData.Add(trainingData);
        }
    }

    private void ProcessPlayerInput(float translationInput, float rotationInput)
    {
        var translation = Time.deltaTime * speed * translationInput;
        var rotation = Time.deltaTime * rotationSpeed * rotationInput;

        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
    }

    private static string ParseToTrainingData([NotNull] IDictionary<string, float> distances, float translationInput, float rotationInput)
    {
        if (distances == null) throw new ArgumentNullException("distances");

        return new StringBuilder()
            .Append(distances["forward"])
            .Append(',')
            .Append(distances["right"])
            .Append(',')
            .Append(distances["left"])
            .Append(',')
            .Append(distances["right45"])
            .Append(',')
            .Append(distances["left45"])
            .Append(',')
            .Append(Utils.Round(translationInput))
            .Append(',')
            .Append(Utils.Round(rotationInput))
            .ToString();
    }
}