package com.example.garcia.ioc.garcia_o_tk.user;

/**
 * author: Claudio Garcia Otero
 * Objecte UserInfo per poder obtenir les dades de l'usuari.
 */
public class UserInfo {

    private String userName;
    private String userRole;
    private String gymName;
    private String name;



    public UserInfo(String userName, String userRole, String gymName, String name){
        this.userName = userName;
        this.userRole = userRole;
        this.gymName = gymName;
        this.name = name;
    }
    public String getUserName() {
        return userName;
    }

    public String getUserRole() {
        return userRole;
    }

    public String getGymName() {
        return gymName;
    }

    public String getName() {
        return name;
    }
}
