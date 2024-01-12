package com.example.garcia.ioc.garcia_o_tk.user;

/**
 * author: Claudio Garcia Otero
 * Objecte UserInfo per poder obtenir les dades de l'usuari.
 */
public class UserInfo {

    private final String userName;
    private final String gymName;
    private final String name;



    public UserInfo(String userName, String gymName, String name){
        this.userName = userName;
        this.gymName = gymName;
        this.name = name;
    }
    public String getUserName() {
        return userName;
    }

    public String getGymName() {
        return gymName;
    }

    public String getName() {
        return name;
    }
}
