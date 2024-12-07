public struct Decision
{
    public Decision(int _id, string _imagen, string _personaje, string _desc, Respuesta _res_der, Respuesta _res_izq)
    {
        id = _id;
        imagen = _imagen;
        personaje = _personaje;
        desc = _desc;
        res_der = _res_der;
        res_izq = _res_izq;
    }

    public int id;
    public string imagen;
    public string personaje;
    public string desc;
    public Respuesta res_der;
    public Respuesta res_izq;
}

public struct Respuesta
{
    public Respuesta(string _respuesta, short[] _efectos, string _explicacion, int _siguiente)
    {
        respuesta = _respuesta;
        efectos = _efectos;
        explicacion = _explicacion;
        siguiente = _siguiente;
    }

    public string respuesta;
    public short[] efectos;
    public string explicacion;
    public int siguiente;
}

public struct HistoryJsonRoot
{
    public string historia;
    public string idioma;
    public string atributo;
    public int nivel;
    public bool aleatoria;
    public System.Collections.Generic.List<Decision> decisiones;
    public System.Collections.Generic.List<Decision> decisiones_respuesta;
}

[System.Serializable]
public struct Historia
{
    public string personaje;
    public string desc;
    public int coste;
    public string imagen;
    public string atributo;
    public string archivo;
}
[System.Serializable]
public struct LevelsJsonRoot
{
    public string lenguaje;
    public int num_historias;
    public Historia[] historias;
}
