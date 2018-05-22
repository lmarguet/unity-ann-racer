using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game
{
    public class ANNDrive : MonoBehaviour
    {
        ANN ann;
        public float visibleDistance = 200;
        public int epochs = 1000;
        public float speed = 50.0F;
        public float rotationSpeed = 100.0F;
        public bool loadFromFile;

        private bool trainingDone;
        private float trainingProgress;
        private double sse;
        private double lastSSE = 1;
        private float translation;
        private float rotation;

        // Use this for initialization
        private void Start()
        {
            ann = new ANN(5, 2, 1, 10, 0.05);
            if (loadFromFile)
            {
                LoadWeightsFromFile();
                trainingDone = true;
            }
            else
                StartCoroutine(LoadTrainingSet());
        }

        public void OnGUI()
        {
            GUI.Label(new Rect(25, 25, 250, 30), "SSE: " + lastSSE);
            GUI.Label(new Rect(25, 40, 250, 30), "Alpha: " + ann.alpha);
            GUI.Label(new Rect(25, 55, 250, 30), "Trained: " + trainingProgress);
        }

        private IEnumerator LoadTrainingSet()
        {
            var path = Application.dataPath + "/trainingData.txt";
            var currentWeights = ann.PrintWeights();
            var calcOutputs = new List<double>();

            if (File.Exists(path))
            {
                var lineCount = File.ReadAllLines(path).Length;
                var tdf = File.OpenText(path);
                var inputs = new List<double>();
                var outputs = new List<double>();

                for (var i = 0; i < epochs; i++)
                {
                    //set file pointer to beginning of file
                    sse = 0;
                    tdf.BaseStream.Position = 0;
                    string line;
                    while ((line = tdf.ReadLine()) != null)
                    {
                        var data = line.Split(',');
                        //if nothing to be learned ignore this line
                        var thisError = 0f;
                        var o2 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[6]));
                        var o1 = Map(0, 1, -1, 1, System.Convert.ToSingle(data[5]));

                        if (System.Convert.ToDouble(data[5]) != 0
                            && System.Convert.ToDouble(data[6]) != 0)
                        {
                            inputs.Clear();
                            outputs.Clear();

                            for (var j = 0; j < 5; j++)
                            {
                                inputs.Add(System.Convert.ToDouble(data[j]));
                            }

                            outputs.Add(o1);
                            outputs.Add(o2);

                            calcOutputs = ann.Train(inputs, outputs);
                            thisError = ((Mathf.Pow((float) (outputs[0] - calcOutputs[0]), 2) +
                                          Mathf.Pow((float) (outputs[1] - calcOutputs[1]), 2))) / 2.0f;
                        }

                        sse += thisError;
                    }

                    trainingProgress = (float) i / epochs;
                    sse /= lineCount;

                    //if sse isn't better then reload previous set of weights
                    //and decrease alpha
                    if (lastSSE < sse)
                    {
                        ann.LoadWeights(currentWeights);
                        ann.alpha = Mathf.Clamp((float) ann.alpha - 0.001f, 0.01f, 0.9f);
                    }
                    else //increase alpha
                    {
                        ann.alpha = Mathf.Clamp((float) ann.alpha + 0.001f, 0.01f, 0.9f);
                        lastSSE = sse;
                    }

                    yield return null;
                }
            }

            trainingDone = true;

            SaveWeightsToFile();
        }

        private void SaveWeightsToFile()
        {
            var path = Application.dataPath + "/weights.txt";

            var wf = File.CreateText(path);
            wf.WriteLine(ann.PrintWeights());
            wf.Close();
        }

        private void LoadWeightsFromFile()
        {
            var path = Application.dataPath + "/weights.txt";
            var wf = File.OpenText(path);

            if (!File.Exists(path)) return;

            var line = wf.ReadLine();
            ann.LoadWeights(line);
        }

        public void Update()
        {
            if (!trainingDone) return;

            var calcOutputs = CalcOutputs();

            var translationInput = Map(-1, 1, 0, 1, (float) calcOutputs[0]);
            var rotationInput = Map(-1, 1, 0, 1, (float) calcOutputs[1]);

            ApplyInputToTransform(translationInput, rotationInput);
        }

        private List<double> CalcOutputs()
        {
            var distances = DriveUtils.PerformRayCasts(transform, visibleDistance);

            return ann.CalcOutput(
                new List<double>
                {
                    distances["forward"],
                    distances["right"],
                    distances["left"],
                    distances["right45"],
                    distances["left45"]
                }
            );
        }


        private static float Map(float newfrom, float newto, float origfrom, float origto, float value)
        {
            if (value <= origfrom)
                return newfrom;

            if (value >= origto)
                return newto;

            return (newto - newfrom)
                   * ((value - origfrom)
                      / (origto - origfrom))
                   + newfrom;
        }


        private void ApplyInputToTransform(float translationInput, float rotationInput)
        {
            translation = translationInput * speed * Time.deltaTime;
            rotation = rotationInput * rotationSpeed * Time.deltaTime;

            transform.Translate(0, 0, translation);
            transform.Rotate(0, rotation, 0);
        }
    }
}