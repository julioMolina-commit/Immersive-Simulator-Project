using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    [SerializeField] private string selectedScene;
    public void LoadSelectedScene()
    {
        SceneManager.LoadScene(selectedScene, LoadSceneMode.Single);
    }
}