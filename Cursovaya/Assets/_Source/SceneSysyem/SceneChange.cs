using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneSysyem
{
    public class SceneChange : MonoBehaviour

    {
       public void OpenMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void OpenGame()
        {
            SceneManager.LoadScene("SampleScene");
        }

    }
}

