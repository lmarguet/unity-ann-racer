using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace Game
{
    public class Drive : MonoBehaviour
    {
        public float speed = 50.0F;
        public float rotationSpeed = 100.0F;
        public float visibleDistance = 200.0f;

        private DriveTrainingData trainingData;
        private StreamWriter tdf;

        void Start()
        {
            trainingData = new DriveTrainingData();
        }

        private void OnApplicationQuit()
        {
            var writer = new TrainingDataWriter();
            writer.Save(trainingData, "training-data");
        }

        void Update()
        {
            var translationInput = Input.GetAxis("Vertical");
            var rotationInput = Input.GetAxis("Horizontal");
            ProcessPlayerInput(translationInput, rotationInput);

            var distances = DriveUtils.PerformRayCasts(transform, visibleDistance);
            var trainingEntry = DriveUtils.ParseToTrainingData(distances, translationInput, rotationInput);
            
            trainingData.AddEntry(trainingEntry);
        }

        private void ProcessPlayerInput(float translationInput, float rotationInput)
        {
            var translation = Time.deltaTime * speed * translationInput;
            var rotation = Time.deltaTime * rotationSpeed * rotationInput;

            transform.Translate(0, 0, translation);
            transform.Rotate(0, rotation, 0);
        }

        
    }
}