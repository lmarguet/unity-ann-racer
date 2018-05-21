using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets
{
    public class ANNDrive : MonoBehaviour
    {
        public float speed = 8;
        public float rotationSpeed = 9;
        public float VisibleDistance = 200f;
        public int epochs = 1000;

        public float translation;
        public float rotation;


        private ANN ann;
        private bool trainingDone = false;
        private float trainingProgress = 0;
        private double sse = 0;
        private double lastSSE = 1;

        void Start()
        {
            ann = new ANN(5, 2, 1, 10, 0.5);
            StartCoroutine(LoadTrainingSet());
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(25, 25, 250, 30), "SSE: " + lastSSE);
            GUI.Label(new Rect(25, 40, 250, 30), "Alpha: " + ann.alpha);
            GUI.Label(new Rect(25, 75, 250, 30), "Trained: " + trainingProgress);
        }

        private IEnumerator LoadTrainingSet()
        {
            var path = Application.dataPath + "/training-data.txt";

            if (File.Exists(path))
            {
                var lineCount = File.ReadAllLines(path).Length;
                var tdf = File.OpenText(path);
                var calcOutputs = new List<double>();
                var inputs = new List<double>();
                var outputs = new List<double>();

                for (var i = 0; i < epochs; i++)
                {
                    sse = 0;
                    tdf.BaseStream.Position = 0;
                    string line;
                    var currentWeights = ann.PrintWeights();
                    while ((line = tdf.ReadLine()) != null)
                    {
                        var data = line.Split(',');
                        var thisError = 0f;

                        if (Convert.ToDouble(data[5]) != 0 && Convert.ToDouble(data[6]) != 0)
                        {
                            inputs.Clear();
                            outputs.Clear();

                            for (var j = 0; j < 5; j++)
                            {
                                inputs.Add(Convert.ToDouble(data[j]));
                            }

                            var o1 = Map(0, 1, -1, 1, Convert.ToSingle(data[5]));
                            var o2 = Map(0, 1, -1, 1, Convert.ToSingle(data[6]));

                            outputs.Add(o1);
                            outputs.Add(o2);

                            calcOutputs = ann.Train(inputs, outputs);
                            thisError = CalculateError(outputs, calcOutputs);
                        }

                        sse += thisError;
                    }

                    trainingProgress = (float) i / (float) epochs;
                    sse /= lineCount;
                    if (lastSSE < sse)
                    {
                        ann.LoadWeights(currentWeights);
                        ann.alpha = Mathf.Clamp((float) ann.alpha - 0.001f, 0.01f, 0.9f);
                    }
                    else
                    {
                        ann.alpha = Mathf.Clamp((float) ann.alpha + 0.001f, 0.01f, 0.9f);
                        lastSSE = sse;
                    }

                    yield return null;
                }
            }

            trainingDone = true;
        }

        private static float CalculateError(IList<double> outputs, IList<double> calcOutputs)
        {
            var ouputDiffA = (float) (outputs[0] - calcOutputs[0]);
            var ouputDiffB = (float) (outputs[1] - calcOutputs[1]);
            
            return (Mathf.Pow(ouputDiffA, 2) + Mathf.Pow(ouputDiffB, 2)) / 2f;
        }

        private double Map(int newFrom, int newTo, int origFrom, int origTo, float value)
        {
            if (value <= origFrom)
            {
                return newFrom;
            }else if (value >= origTo)
            {
                return newTo;
            }

            return (newTo - newFrom) * ((value - origFrom) / (origTo - origFrom)) + newFrom;
        }
        
        private float Round(float x)
        {
            return (float) System.Math.Round(x, System.MidpointRounding.AwayFromZero) / 2f;
        }

        private void Update()
        {
            if(!trainingDone) return;
            
            var calcOutputs = new List<double>();
            var inputs = new List<double>();
            var outputs = new List<double>();
            
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
            
            inputs.Add(fDist);
            inputs.Add(rDist);
            inputs.Add(lDist);
            inputs.Add(r45Dist);
            inputs.Add(l45Dist);
            
            outputs.Add(0);
            outputs.Add(0);

            calcOutputs = ann.CalcOutput(inputs, outputs);

            var translationInput = (float) Map(-1, 1, 0, 1, (float) calcOutputs[0]);
            var rotationInput = (float) Map(-1, 1, 0, 1, (float) calcOutputs[1]);

            translation = translationInput * speed * Time.deltaTime;
            rotation = rotationInput * rotationSpeed * Time.deltaTime;
            
            transform.Translate(0, 0, translation);
            transform.Rotate(0, rotation, 0);
        }
    }
}