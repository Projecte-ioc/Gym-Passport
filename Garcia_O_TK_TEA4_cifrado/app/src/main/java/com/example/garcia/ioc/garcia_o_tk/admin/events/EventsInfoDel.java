package com.example.garcia.ioc.garcia_o_tk.admin.events;

/**
 * author: Claudio Garcia.
 * Objecte per obtenir les dades del llistat d'events a eliminar..
 */
public class EventsInfoDel {
    private final int id;
    private final String name;
    private final String date;

    public EventsInfoDel(int id, String name, String date) {
        this.id = id;
        this.name = name;
        this.date = date;
    }

    public int getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public String getDate() {
        return date;
    }
}
