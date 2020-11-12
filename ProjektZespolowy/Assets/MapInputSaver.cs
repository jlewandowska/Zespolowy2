using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class MapInputSaver : MonoBehaviour
{
    public GameObject inputField;

    public void saveMapInput()
    {
        var obj = inputField.GetComponent<InputField>();
        var text = obj.text;

        string fileName = "wyniki/flagi.txt";
        System.IO.Directory.CreateDirectory("wyniki");
        using (StreamWriter writetext = new StreamWriter(fileName))
        {
            
            writetext.WriteLine("Ilość map: " + text);
        }
    }
}

