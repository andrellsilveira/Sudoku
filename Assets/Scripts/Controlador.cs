using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SelecionarCelula();
    }

    void SelecionarCelula()
    {
        RaycastHit2D _hit = new RaycastHit2D();

#if UNITY_STANDALONE || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        }
#endif
#if UNITY_ANDROID
        //_hit = Physics2D.Raycast(new Vector2(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).x, Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position).y), Vector2.zero, 0);
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                _hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
            }            
        }
#endif

        if (_hit)
        {
            if (_hit.collider.CompareTag("Celula"))
            {
                Debug.Log("Selecionada célula " + _hit.transform.name);
            }
        }

    }
}
