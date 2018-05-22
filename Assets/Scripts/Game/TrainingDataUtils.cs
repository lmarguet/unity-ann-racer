using System.Text;
using DefaultNamespace;
using UnityEngine;

namespace Game
{
    public class TrainingDataUtils
    {

        public static string ParseTrainingDataEntry(DriveTrainingEntry entry)
        {
            var builder = new StringBuilder();
            for (var i = 0; i < entry.NumValues; i++)
            {
                if (builder.Length > 0)
                {
                    builder.Append(',');
                }
                builder.Append(entry.GetValueAt(i));
            }        
    
            return builder.ToString();
        }

        public static string BuildDataFilePath(string filename)
        {
            return Application.dataPath + "/Data/" + filename + ".txt";
        }
    }
}