using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class EventExport
{
    static string resultsFileName = "wyniki.txt";
    static string folderName = "wyniki";

    static public void exportMainResultsToFile()
    {
        Directory.CreateDirectory(folderName);

        using (StreamWriter writetext = new StreamWriter(folderName+ "\\"+ resultsFileName))
        {

            string results = "";
            foreach (var entity in GameFlowManager.eventsLog)
            {
                results += entity.toString();
                results += "\n";
            }

            writetext.WriteLine(results);
        }
    }
    
}
