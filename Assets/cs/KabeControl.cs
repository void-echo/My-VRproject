using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KabeControl : MonoBehaviour
{
    public static string kabeInfo = "0000000000000000000000000000000000000000";
    public static KabeControl myself = null;
    public static float kabeHeight = 0.25f;
    public GameObject kabePrefab;

    public Dictionary<int, Transform> kabeSet;

    // before create
    void Awake()
    {
        myself = this;

        this.kabeSet = new Dictionary<int, Transform>();
        for (int i = 1; i <= 40; i++)
        {
            int baseZ = -1;
            if (i >= 1 && i < 10)
            {
                baseZ = 0;
            }
            else if (i >= 10 && i < 19)
            {
                baseZ = 1;
            }
            else if (i >= 19 && i < 28)
            {
                baseZ = 2;
            }
            else if (i >= 28 && i < 37)
            {
                baseZ = 3;
            }
            else if (i >= 37 && i <= 40)
            {
                baseZ = 4;
            }
            else
            {
                // Debug.Log("error");
            }

            baseZ = baseZ * 10 - 20;

            int relativeNum = i % 9;
            int X = -1;
            switch (relativeNum)
            {
                case 1:
                    X = 15;
                    break;
                case 2:
                    X = 5;
                    break;
                case 3:
                    X = -5;
                    break;
                case 4:
                    X = -15;
                    break;
                case 5:
                    X = 20;
                    break;
                case 6:
                    X = 10;
                    break;
                case 7:
                    X = 0;
                    break;
                case 8:
                    X = -10;
                    break;
                case 0:
                    X = -20;
                    break;
                default:
                    // print("error");
                    // Debug.Log("error: relativeNum = " + relativeNum + ", Expected 0-8");
                    break;
            }

            int relativeZ = -1;
            if (relativeNum >= 1 && relativeNum <= 4)
            {
                relativeZ = 0;
            }
            else
            {
                relativeZ = 5;
            }
            int Z = baseZ + relativeZ;
            GameObject tmpForTransform = new GameObject();
            tmpForTransform.transform.position = new Vector3(X, kabeHeight, Z);
            if (i >= 1 && i <= 4 || i >= 10 && i <= 13 || i >= 19 && i <= 22 || i >= 28 && i <= 31 || i >= 37 && i <= 40)
            {
                tmpForTransform.transform.Rotate(0, 90, 0);
            }
            // kabe_pos.position = new Vector3(X, kabeHeight, Z);
            this.kabeSet.Add(i, tmpForTransform.transform);

            // delete tmpForTransform
            Destroy(tmpForTransform);
        }
    }

    // Start is called before the first frame update
    public void Start_(string kabeInfo = "0000000000000000000000000000000000000000")
    {
        KabeControl.kabeInfo = kabeInfo;

        for (int i = 0; i < 40; i++)
        {
            if (kabeInfo[i] == '1')
            {
                CreateKabe(i + 1);
            }
        }        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void updateKabeBySeq(string seq)
    {
        // Debug.Log("updateKabeBySeq: " + seq);
        // Debug.Log("updateKabeBySeq: " + seq.Length);
        if (seq.Length != 40)
        {
            Debug.Log("updateKabeBySeq: seq.Length != 40");
            return;
        }
        // firstly delete all objects created by prefab named "Kabe"
        GameObject[] kabeObjects = GameObject.FindGameObjectsWithTag("Kabe");
        foreach (GameObject kabeObject in kabeObjects)
        {
            // inactivate the object
            kabeObject.SetActive(false);
        }


        kabeInfo = seq;
        // find KabeControl c# object, and delete it
        for (int i = 0; i < 40; i++)
        {
            if (kabeInfo[i] == '1')
            {
                myself.CreateKabe(i + 1);
            }
        }

    }

    void CreateKabe(int index)
    {
        Instantiate(this.kabePrefab, this.kabeSet[index].position, this.kabeSet[index].rotation);
    }
}
