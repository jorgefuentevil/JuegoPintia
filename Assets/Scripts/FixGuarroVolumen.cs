using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixGuarroVolumen : MonoBehaviour
{

    [SerializeField] private GameObject volumen;

    // Start is called before the first frame update
    void Start()
    {
        volumen.SetActive(true);
        volumen.SetActive(false);
    }
}
