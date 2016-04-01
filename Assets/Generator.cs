﻿using UnityEngine;
using System.Collections;

public class Generator : MonoBehaviour
{
    public bool useGlobalMultipliers = false;
    public float resourcePerSecond = 0.1f;

    public float bonusCash = 0.0f;

    public float tickDuration = 1.0f;

    public bool startImmediately = false;
    public bool isStarted = false;

    //public float levelUpMultiplier = 0.1f;
    public int currentLevel = 0;

    public GameObject progressBar = null;
    private IValue _pbValue = null;
    //public MonoBehaviour behavior = null;

    public bool printTick = false;

    public float progress = 0.0f;

    public void OnPurchased()
    {
        if (!startImmediately && currentLevel == 0)
            StartCoroutine(Tick());

        currentLevel++;
    }//OnPurchase

    //public MonoBehaviour behavior = null;
	// Use this for initialization
	void Start ()
    {
        GameManager.self.AddResource(bonusCash);
        
        if(progressBar != null)
        {
            _pbValue = progressBar.GetComponentInChildren<IValue>();
            //print(_pbValue);
            //behavior = (MonoBehaviour)_pbValue;
            //behavior = (MonoBehaviour)_pbValue;
            if (_pbValue == null)
            {
                Debug.LogWarning("Progress bar game object does not have an attached script that implements IValue");
            }//else
        }

        if (startImmediately)
            StartCoroutine(Tick());
    }

    void Update()
    {
        if (_pbValue != null)
        {
            _pbValue.Value = progress;
        }//if

    }//Update

    public float TotalResourcePerSecond(int level)
    {
        return (resourcePerSecond * level + ((useGlobalMultipliers) ? GameManager.self.TotalTickMultiplier : 0)); 
    }

    public IEnumerator Tick()
    {
        float timer = 0.0f;
        isStarted = true;
        while (true)
        {
            GameManager.self.AddResource(TotalResourcePerSecond(currentLevel));

            if (printTick)
                print(name + " -> TICK (" + tickDuration + "s) for $" + TotalResourcePerSecond(currentLevel));

            while (timer < tickDuration)
            {
                timer += Time.deltaTime;
                progress = timer / tickDuration;
                yield return new WaitForEndOfFrame();
            }//while
            timer = 0;
            //yield return new WaitForSeconds(generateAfterSeconds);
            yield return new WaitForEndOfFrame();


        }//while

    }//Tick

    
    
}
