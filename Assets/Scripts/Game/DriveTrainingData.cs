using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Game
{
    public class DriveTrainingData
    {
        private List<DriveTrainingEntry> entries;

        public DriveTrainingData()
        {
            entries = new List<DriveTrainingEntry>();
        }


        public void AddEntry(DriveTrainingEntry entry)
        {
            entries.Add(entry);
        }

        public IEnumerable<DriveTrainingEntry> GetUniqueEntriesList()
        {
            return entries.Distinct(new DriveTrainingEntryEqualityComparer());
        }
    }

    internal class DriveTrainingEntryEqualityComparer : IEqualityComparer<DriveTrainingEntry>
    {
        public bool Equals(DriveTrainingEntry x, DriveTrainingEntry y)
        {
            if (x.NumValues != y.NumValues)
            {
                return false;
            }

            for (var i = 0; i < x.NumValues; i++)
            {
                if (x.GetValueAt(i) != y.GetValueAt(i))
                    return false;
            }

            return true;
        }

        public int GetHashCode(DriveTrainingEntry obj)
        {
            return 0;
        }
    }
}