using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MaquinaEstadosCartas : MonoBehaviour
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
        COMMIT_EXPLICACION_DERECHA,
        COMMIT_EXPLICACION_IZQUIERDA,

        PRE_MUERTE,

        TRANSICIONANDO,

        NINGUNO
    }


    public GameState estadoActual { get; private set; }
    private GameState estadoAuxiliar = GameState.NINGUNO;
    public HistoryManager historyManager;
    public AnswerSelector selector;
    public IconManager iconManager;

    public void Awake(){
        
    }

    public void CambiarDeEstado(GameState nuevoEstado)
    {
        switch (estadoActual)
        {

            default:
                break;
        }
        Debug.Log($"Iniciando cambio de estado a {nuevoEstado}");
        switch (nuevoEstado)
        {
            case GameState.INICIALIZANDO:
                EntraEstadoInicializando();
                break;

            case GameState.ELIGE_CARTA:
                EntraEstadoEligeCarta();
                break;
            case GameState.SHOW_RESPUESTA_DERECHA:
                EntraEstadoShowRespuestaDerecha();
                break;
            case GameState.SHOW_RESPUESTA_IZQUIERDA:
                EntraEstadoShowRespuestaIzquierda();
                break;
            case GameState.COMMIT_RESPUESTA_DERECHA:
                EntraEstadoCommitRespuestaDerecha();
                break;
            case GameState.COMMIT_RESPUESTA_IZQUIERDA:
                EntraEstadoCommitRespuestaIzquierda();
                break;
            case GameState.SHOW_EXPLICACION:
                EntraEstadoShowExplicacion();
                break;
            case GameState.COMMIT_EXPLICACION_DERECHA:
                EntraEstadoCommitExplicacionDerecha();
                break;
            case GameState.COMMIT_EXPLICACION_IZQUIERDA:
                EntraEstadoCommitExplicacionIzquierda();
                break;

        }
        Debug.Log($"Estado Actual");

    
    }

    private void EntraEstadoInicializando()
    {
        estadoActual = GameState.INICIALIZANDO;
        historyManager.SetEstadoInicializando();
        CambiarDeEstado(GameState.ELIGE_CARTA);

    }

    private void EntraEstadoEligeCarta()
    {   
        estadoActual = GameState.TRANSICIONANDO;
        estadoAuxiliar = GameState.ELIGE_CARTA;
        iconManager.SetEstadoEligeCarta();
        selector.ShowEligeCarta();
        
    }

    private void EntraEstadoShowRespuestaDerecha()
    {
        estadoActual = GameState.TRANSICIONANDO;
        estadoAuxiliar = GameState.SHOW_RESPUESTA_DERECHA;
        iconManager.SetEstadoShowRespuestaDerecha();
        selector.ShowRespuestaDerecha();

    }

    private void EntraEstadoShowRespuestaIzquierda()
    {
        estadoActual = GameState.TRANSICIONANDO;
        estadoAuxiliar = GameState.SHOW_RESPUESTA_IZQUIERDA;
        iconManager.SetEstadoShowRespuestaIzquierda();
        selector.ShowRespuestaIzquierda();

    }

    private void EntraEstadoCommitRespuestaDerecha()
    {   
        estadoActual = GameState.TRANSICIONANDO;
        //estadoAuxiliar = GameState.COMMIT_RESPUESTA_DERECHA; // ¿REALMENTE SE NECESITA?
        iconManager.SetEstadoCommit();
        selector.CommitRespuestaDerecha();
    }

    private void EntraEstadoCommitRespuestaIzquierda()
    {
        estadoActual = GameState.TRANSICIONANDO;
        //estadoAuxiliar = GameState.COMMIT_RESPUESTA_IZQUIERDA;
        iconManager.SetEstadoCommit();
        selector.CommitRespuestaIzquierda();
    }


    private void EntraEstadoShowExplicacion()
    {
        estadoActual = GameState.TRANSICIONANDO;
        estadoAuxiliar = GameState.SHOW_EXPLICACION;
        iconManager.SetEstadoShowExplicacion();
        selector.ShowExplicacion();
    }


    private void EntraEstadoCommitExplicacionDerecha()
    {
        estadoActual = GameState.TRANSICIONANDO;
        estadoAuxiliar = GameState.COMMIT_EXPLICACION_DERECHA;
        iconManager.SetEstadoCommit();
        selector.CommitExplicacionDerecha();
    }

    private void EntraEstadoCommitExplicacionIzquierda()
    {
        estadoActual = GameState.TRANSICIONANDO;
        estadoAuxiliar = GameState.COMMIT_EXPLICACION_IZQUIERDA;
        iconManager.SetEstadoCommit();
        selector.CommitExplicacionIzquierda();
    }

    public void AvisaFinalAnimacion()
    {
        if (estadoActual != GameState.TRANSICIONANDO || estadoAuxiliar == GameState.NINGUNO) return;

        estadoActual = estadoAuxiliar;
        estadoAuxiliar = GameState.NINGUNO;
    }


    //Getters del Enum de Estados
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaInicializando()
    {
        return estadoActual == GameState.INICIALIZANDO;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaEligeCarta()
    {
        return estadoActual == GameState.ELIGE_CARTA;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaShowRespuestaDerecha()
    {
        return estadoActual == GameState.SHOW_RESPUESTA_DERECHA;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaShowRespuestaIzquierda()
    {
        return estadoActual == GameState.SHOW_RESPUESTA_IZQUIERDA;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaCommitRespuestaDerecha()
    {
        return estadoActual == GameState.TRANSICIONANDO && estadoAuxiliar == GameState.COMMIT_RESPUESTA_DERECHA;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaCommitRespuestaIzquierda()
    {
        return estadoActual == GameState.TRANSICIONANDO && estadoAuxiliar == GameState.COMMIT_RESPUESTA_IZQUIERDA;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaShowExplicacion()
    {
        return  estadoActual == GameState.SHOW_EXPLICACION ||
                estadoAuxiliar == GameState.COMMIT_EXPLICACION_DERECHA ||
                estadoAuxiliar == GameState.COMMIT_EXPLICACION_IZQUIERDA;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaCommitExplicacionDerecha()
    {
        return estadoActual == GameState.COMMIT_EXPLICACION_DERECHA;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaCommitExplicacionIzquierda()
    {
        return estadoActual == GameState.COMMIT_EXPLICACION_IZQUIERDA;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaPreMuerte()
    {
        return estadoActual == GameState.PRE_MUERTE;
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool EstaTransicionando()
    {
        return estadoActual == GameState.TRANSICIONANDO;
    }
}
