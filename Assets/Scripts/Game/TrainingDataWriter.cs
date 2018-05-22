using System.IO;
using NUnit.Framework;
using UnityEngine;

namespace Game
{
    public class TrainingDataWriter
    {
        public void Save(DriveTrainingData data, string path)
        {
            var trainningFile = File.CreateText(path);
            var entries = data.GetUniqueEntriesList();
            
            foreach (var entry in entries)
            {
                var parsedEntry = TrainingDataUtils.ParseTrainingDataEntry(entry);
                trainningFile.WriteLine(parsedEntry);
            }

            trainningFile.Close();

            Assert.IsTrue(File.Exists(path));
        }
    }
}