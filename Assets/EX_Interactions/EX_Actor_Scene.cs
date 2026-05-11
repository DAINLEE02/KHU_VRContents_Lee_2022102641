using UnityEngine;
using UnityEngine.SceneManagement;

public class EX_Actor_Scene : MonoBehaviour
{
    public string sceneName;

    public void Execute()
    {
        // 1초 뒤에 씬을 이동하도록 설정 (잡는 걸 확인할 시간 확보)
        Invoke("DelayedLoadScene", 1.0f);
    }

    void DelayedLoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}