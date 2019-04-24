using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posicion
{

    Vector2 posicionMonstruo;
    Vector2 posicionObjetivo;
    Vector2 direccionObjetivo;

    int profundidad;
    float velocidad;

    public float coste;
    public Posicion padre;

    public Vector2 direccionPosicion;

    public Posicion(Vector2 posicionMonstruo, Vector2 posicionObjetivo, Vector2 direccionPosicion, Vector2 direccionObjetivo, int profundidad, float velocidad, Posicion padre)
    {
        this.posicionMonstruo = posicionMonstruo;
        this.posicionObjetivo = posicionObjetivo;
        this.direccionObjetivo = direccionObjetivo;
        this.direccionPosicion = direccionPosicion;
        this.profundidad = profundidad;
        this.velocidad = velocidad;
        this.padre = padre;
        coste = distancia(posicionObjetivo) + profundidad;
    }


    public List<Posicion> generaHijos()
    {
        List<Posicion> hijos = new List<Posicion>();

        hijos.Add(generaHijoGenerico(Vector2.up));

        hijos.Add(generaHijoGenerico(Vector2.right));

        hijos.Add(generaHijoGenerico(Vector2.down));

        hijos.Add(generaHijoGenerico(Vector2.left));


        hijos.Add(generaHijoGenerico(new Vector2(1, 1)));

        hijos.Add(generaHijoGenerico(new Vector2(-1, 1)));

        hijos.Add(generaHijoGenerico(new Vector2(-1, -1)));

        hijos.Add(generaHijoGenerico(new Vector2(1, -1)));

        return hijos;
    }

    Posicion generaHijoGenerico(Vector2 direccion)
    {
        float posicionX = posicionMonstruo.x + (direccion.x * velocidad);
        float posicionY = posicionMonstruo.y + (direccion.y * velocidad);

        Vector2 destino = new Vector2(posicionX, posicionY);

        if (distancia(posicionObjetivo) < distancia(destino) && (direccion.x == direccionObjetivo.x && direccion.y == direccionObjetivo.y))
        {
            destino = posicionObjetivo;
        }

        Posicion hijo = new Posicion(destino, posicionObjetivo, direccion, direccionObjetivo, profundidad + 1, velocidad, this);
        return hijo;
    }

    public float distancia(Vector2 objetivo)
    {
        float x1 = posicionMonstruo.x, x2 = objetivo.x;
        float y1 = posicionMonstruo.y, y2 = objetivo.y;

        return Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));
    }

    public Vector2 getPosicion()
    {
        return posicionMonstruo;
    }

    public override bool Equals(object obj)
    {
        if (this == obj)
            return true;

        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        Posicion p = (Posicion)obj;

        if (posicionMonstruo.x == p.posicionMonstruo.x && posicionMonstruo.y == p.posicionMonstruo.y &&
            posicionObjetivo.x == p.posicionObjetivo.x && posicionObjetivo.y == p.posicionObjetivo.y)
        {
            return true;
        }
        else
            return false;
    }
}
