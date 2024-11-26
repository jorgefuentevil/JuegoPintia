using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaquinaEstadosCartas1 : MonoBehaviour
{
    public enum GameState
    {
        INICIALIZANDO,

        ELIGE_CARTA,

        SHOW_RESPUESTA_DERECHA,
        SHOW_RESPUESTA_IZQUIERDA,

        COMMIT_RESPUESTA_DERECHA,
        COMMIT_RESPUESTA_IZQUIERDA,

        SHOW_EXPLICACION,
        COMMIT_EXPLICACION,

        PRE_MUERTE,

        TRANSICIONANDO
    }


    public GameState EstadoActual { get; private set; }


    private void Awake()
    {
        CambiarDeEstado(GameState.INICIALIZANDO);
    }



    public void CambiarDeEstado(GameState nuevoEstado)
    {
        switch (EstadoActual)
        {

            default:
                break;
        }

        switch(nuevoEstado)
        {
            case GameState.INICIALIZANDO:
                break;
        }


    }
}
