package com.example.garcia.ioc.garcia_o_tk.admin.events;

/**
 * author: Claudio Garcia.
 * Objecte per obtenir les dades del llistat d'events.
 */

public class EventsInfo {

    private final String evName;
    private final String date;
    private final String hour;
    private final String attendes;
    private final String location;

    public EventsInfo(String evName, String date, String hour,  String attendes, String location) {
        this.evName = evName;
        this.date = date;
        this.hour = hour;
        this.attendes = attendes;
        this.location = location;
    }

    public String getEvName() {
        return evName;
    }

    public String getDate() {
        return date;
    }

    public String getHour() {
        return hour;
    }


    public String getAttendes() {
        return attendes;
    }

    public String getLocation() {
        return location;
    }
}
