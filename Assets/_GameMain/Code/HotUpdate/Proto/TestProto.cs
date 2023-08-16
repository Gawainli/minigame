using System.Collections;
using System.Collections.Generic;
using System.IO;
using MiniGame.Logger;
using UnityEngine;

public class TestProto : MonoBehaviour
{
    public static void Test()
    {
        Debug.Log("TestProto.Test 55555");
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        var person = new Person{ Name = "Tom", Age = 18 };
        var ms = new MemoryStream();
        ProtoBuf.Serializer.Serialize(ms, person);
        var bytes = ms.ToArray();
        LogModule.Info("bytes length: " + bytes.Length);
        ms.Close();
        
        var ms2 = new MemoryStream(bytes);
        var newPerson = ProtoBuf.Serializer.Deserialize<Person>(ms2);
        LogModule.Info("newPerson: " + newPerson.Name+ " " + newPerson.Age);
        ms2.Close();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
