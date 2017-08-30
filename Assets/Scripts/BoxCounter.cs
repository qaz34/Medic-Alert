using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class BoxCounter : MonoBehaviour
{
    public int boxesForInstance = 0;
    void OnApplicationQuit()
    {
        try
        {
            string line;
            using (FileStream fileStream = new FileStream("BoxesDestroyed.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                using (StreamReader re = new StreamReader(fileStream))
                {
                    line = re.ReadLine();
                    boxesForInstance += int.Parse(line);                    
                }
                //add comment
                
            }
            using (FileStream fileStream = new FileStream("BoxesDestroyed.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
            {
                using (StreamWriter wr = new StreamWriter(fileStream))
                {
                    wr.WriteLine(boxesForInstance.ToString());
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.Message);
        }
    }
}
