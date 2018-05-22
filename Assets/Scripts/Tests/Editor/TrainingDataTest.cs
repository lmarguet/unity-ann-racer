using System.Collections.Generic;
using System.IO;
using System.Linq;
using DefaultNamespace;
using Game;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Tests.Editor
{
    public class TrainingDataTest
    {
        [Test]
        public void TestTrainingEntryParsing()
        {
            var entry = CreateEntry(new[] {1, 0, 2, 3, 0.5f});
            var parsed = TrainingDataUtils.ParseTrainingDataEntry(entry);
            Assert.AreEqual(parsed, "1,0,2,3,0.5");
        }

        private static DriveTrainingEntry CreateEntry(IEnumerable<float> values)
        {
            var entry = new DriveTrainingEntry();
            foreach (var value in values)
            {
                entry.Add(value);
            }

            return entry;
        }

        [Test]
        public void TestDuplicateRemove()
        {
            var trainingData = CreateTrainingData();

            var numEntries = trainingData.GetUniqueEntriesList()
                .ToList()
                .Count;

            Assert.AreEqual(3, numEntries);
        }

        private static DriveTrainingData CreateTrainingData()
        {
            var trainingData = new DriveTrainingData();
            trainingData.AddEntry(CreateEntry(new[] {1, 0, 2, 3, 0.5f}));
            trainingData.AddEntry(CreateEntry(new[] {1, 0, 2, 3, 0.5f}));
            trainingData.AddEntry(CreateEntry(new[] {1, 0, 2, 3, 0.5f}));

            trainingData.AddEntry(CreateEntry(new[] {3, 2, 1, 5, 0.2f}));

            trainingData.AddEntry(CreateEntry(new[] {8, 2, 1, 6, 0f}));
            return trainingData;
        }

        [Test]
        public void TestPathBuilder()
        {
            var path = TrainingDataUtils.BuildDataFilePath("test-data");
            Assert.AreEqual(Application.dataPath + "/Data/test-data.txt", path);
        }

        [Test]
        public void TestSave()
        {
            var trainingData = CreateTrainingData();

            var path = TrainingDataUtils.BuildDataFilePath("test-data");
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            var writer = new TrainingDataWriter();
            writer.Save(trainingData, path);

            Assert.IsTrue(File.Exists(path));
        }
    }
}