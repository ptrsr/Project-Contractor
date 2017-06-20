using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoOnImage : MonoBehaviour {
    private Texture _image;

    [SerializeField]
    private VideoClip _videoToPlay;

    [SerializeField]
    private Material _mat;

    private VideoPlayer _videoPlayer;
    private VideoSource _videoSource;

    private AudioSource _audioSource;
     
	// Use this for initialization
	void Start () {
        _image = new Texture();
        PlayVideo();
	}

    public void PlayVideo()
    {
        StartCoroutine(playVideo());
    }


    IEnumerator playVideo()
    {
        _videoPlayer = gameObject.AddComponent<VideoPlayer>();
        _audioSource = gameObject.AddComponent<AudioSource>();

        _videoPlayer.playOnAwake = false;
        _audioSource.playOnAwake = false;

        _audioSource.Pause();
        
        _videoPlayer.source = VideoSource.VideoClip;

        _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        _videoPlayer.EnableAudioTrack(0, true);
        _videoPlayer.SetTargetAudioSource(0, _audioSource);

        _videoPlayer.clip = _videoToPlay;
        _videoPlayer.Prepare();

        WaitForSeconds waitTime = new WaitForSeconds(1);
        while (!_videoPlayer.isPrepared)
        {
            Debug.Log("Preparing video");
            yield return waitTime;
            break;

        }


        _image = _videoPlayer.texture;
        _mat.SetTexture("_Video", _image);

        _videoPlayer.Play();

        _audioSource.Play();

        Debug.Log("Playing video");

        while (_videoPlayer.isPlaying)
        {
            Debug.Log("Video Time: " + Mathf.FloorToInt((float)_videoPlayer.time));
            yield return null;
        }

        Debug.Log("Done Playing video");

    }
}
