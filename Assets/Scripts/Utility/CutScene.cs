using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CutSceneInTwo()
    {
        Scene scene = SceneManager.GetSceneByName("Stan-Scene");
        GameObject[] sceneObj = scene.GetRootGameObjects();
        sceneObj[1].name = "Scene";
        sceneObj[2].name = "Scene";

        for (int i = 1; i < sceneObj.Length / 2; i++)
        {
            if (i == 2) continue;
            sceneObj[i].transform.parent = sceneObj[1].transform;
        }
        for(int i = sceneObj.Length / 2; i < sceneObj.Length; i++)
        {
            sceneObj[i].transform.parent = sceneObj[2].transform;
        }
    }
    public void CutWhatWasCutInTwo()
    {

        Scene scene = SceneManager.GetSceneByName("Stan-Scene");
        GameObject[] sceneObj = scene.GetRootGameObjects();
        Transform[] children;
        for (int i = 1; i < sceneObj.Length; i++)
        {
            children = sceneObj[i].GetComponentsInChildren<Transform>();
            if(children.Length <= 5)
            {
                continue;
            }
            children[0].name = "Scene";
            children[1].name = "Scene";
            children[0].parent = null;
            children[1].parent = null;
            for (int j = 0; j < children.Length / 2; j++)
            {
                if (j == 1) continue;
                children[j].parent = children[0];
            }
            for (int j = children.Length / 2; j < children.Length; j++)
            {
                children[j].parent = children[1];
            }
        }

        
        
    }

}
