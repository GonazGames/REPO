using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerController1 : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    private void Start()
    {
        // Get the VideoPlayer component on the same game object
        videoPlayer = GetComponent<VideoPlayer>();

        // Set a callback to be called when the video finishes playing
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        // Load the specified scene when the video finishes playing
        SceneManager.LoadScene("Menu_Main");
    }
}
