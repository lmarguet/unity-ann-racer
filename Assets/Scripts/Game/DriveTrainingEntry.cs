using System.Collections.Generic;
using System.Text;

namespace DefaultNamespace
{
    public class DriveTrainingEntry
    {
        private List<float> values;

        public DriveTrainingEntry()
        {
            values = new List<float>();
        }

        public DriveTrainingEntry Add(float value)
        {
            values.Add(value);
            return this;
        }

        public float GetValueAt(int index)
        {
            return values[index];
        }

        public float NumValues
        {
            get { return values.Count; }
        }
    }
}