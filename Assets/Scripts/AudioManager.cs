using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sokobrain
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;
        public static AudioManager Instance { get { return _instance; } }

        public float pitchChangeFloor, pitchChangeCeiling;

        [Header("AudioSources")] 
        [SerializeField] private AudioSource mainSource;
        [SerializeField] private AudioSource bgSource;
        
        [Header("Clips")]
        [SerializeField] private List<AudioClip> normalMoves;
        [SerializeField] private AudioClip disconnectedMove; //When only 1 of 2 players move
        [SerializeField] private AudioClip failedMove;
        [SerializeField] private List<AudioClip> undoMoves;
        [SerializeField] private AudioClip ghostSwitch;
        [SerializeField] private AudioClip backgroundMusic;
        [SerializeField] private AudioClip goalSound1;
        [SerializeField] private AudioClip goalSound2;

        private float mainSourceRestingVolume = 0.5328289f;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            bgSource.clip = backgroundMusic;
            bgSource.loop = true;
            bgSource.Play();
        }

        public void ChangeSFXVolume(Slider sender) {
            mainSourceRestingVolume = sender.value * 0.1f;
            mainSource.volume = mainSourceRestingVolume;
            PlayNormalMove();
        }

        public void ChangeMusicVolume(Slider sender) {
            bgSource.volume = sender.value * 0.05f;
        }

        public void PlayGhostSwitchAudio() {
            mainSource.pitch = 1;
            mainSource.volume = mainSourceRestingVolume;
            mainSource.PlayOneShot(ghostSwitch);
            mainSource.pitch = UnityEngine.Random.Range(pitchChangeFloor, pitchChangeCeiling);
            mainSource.volume = UnityEngine.Random.Range(mainSourceRestingVolume - 0.25f, mainSourceRestingVolume);
        }

        public void PlayDisconnectedMove() {
            mainSource.PlayOneShot(disconnectedMove);
        }

        public void PlayNormalMove()
        {
            mainSource.pitch = 1;
            mainSource.volume = mainSourceRestingVolume;
            mainSource.PlayOneShot(normalMoves[UnityEngine.Random.Range(0, normalMoves.Count)]);
            mainSource.pitch = UnityEngine.Random.Range(pitchChangeFloor, pitchChangeCeiling);
            mainSource.volume = UnityEngine.Random.Range(mainSourceRestingVolume - 0.25f, mainSourceRestingVolume);
        }

        public void PlayFailedMove() {
            mainSource.PlayOneShot(failedMove);
        }

        public void PlayUndoMove() {
            mainSource.pitch = 1;
            mainSource.volume = mainSourceRestingVolume;
            mainSource.PlayOneShot(undoMoves[UnityEngine.Random.Range(0, undoMoves.Count)]);
            mainSource.pitch = UnityEngine.Random.Range(pitchChangeFloor, pitchChangeCeiling);
            mainSource.volume = UnityEngine.Random.Range(mainSourceRestingVolume - 0.25f, mainSourceRestingVolume);
        }

        public void PlayGoalSound1() {
            mainSource.pitch = 1;
            mainSource.volume = mainSourceRestingVolume;
            mainSource.PlayOneShot(goalSound1);
            Debug.Log("Goal 1");
        }

        public void PlayGoalSound2() {
            mainSource.pitch = 1;
            mainSource.volume = mainSourceRestingVolume;
            mainSource.PlayOneShot(goalSound2);
            Debug.Log("Goal 2");
        }
    }
}