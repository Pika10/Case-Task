using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class LevelNode : MonoBehaviour
{
    [SerializeField] public int poolThreshold;
    [SerializeField] public Animation gateAnimation;
    [HideInInspector] public Transform controlPoint;
    [HideInInspector] public Transform endPoint;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public List<GameObject> objects;
    [HideInInspector] public int ID;
    private int objectInPoolCount;
    void Awake()
    {
        InitializeLevelNode();
    }

    void InitializeLevelNode()
    {
        foreach (Transform tr in transform)
        {
            switch (tr.tag)
            {
                case "ControlPoint":
                    controlPoint = tr;
                    break;
                case "EndPoint":
                    endPoint = tr;
                    break;
                case "PoolText":
                    text = tr.GetChild(0).GetComponent<TextMeshProUGUI>();
                    break;
            }
        }

        text.text = "0 / " + poolThreshold;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            Destroy(other.gameObject,1.5f);
            objectInPoolCount += 1;
            text.text = objectInPoolCount + " / " + poolThreshold;
        }
    }

    private void ControlPoolTimed()
    {
        Invoke(nameof(ControlPool),2);
    }
    private void ControlPool()
    {
        if (objectInPoolCount >= poolThreshold)
        {
            gateAnimation.Play();
            GameManager.Instance.Invoke(nameof(GameManager.Instance.GoNextNode),0.7f);
        }
        else
        {
            GameManager.Instance.LoseLevel();
        }
    }

    public void EnableNode()
    {
        GameManager.ControlEnter += ControlPoolTimed;
    }

    public void DisableNode()
    {
        GameManager.ControlEnter -= ControlPoolTimed;
    }
}