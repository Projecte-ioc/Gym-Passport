package com.example.garcia.ioc.garcia_o_tk.admin.perfil;

import java.util.List;

/**
 * author: Claudio Garcia Otero
 * Objecte GymInfo per poder obtenir les dades del gimnás.
 */
public class GymInfo {

    private String userName;
    private String userRole;
    private String gymName;
    private String gymAddress;
    private String gymPhoneNumber;
    private List gymScheduleString;


    public GymInfo(String userName, String userRole, String gymName, String gymAddress, String gymPhoneNumber, List gymScheduleString) {
        this.userName = userName;
        this.userRole = userRole;
        this.gymName = gymName;
        this.gymAddress = gymAddress;
        this.gymPhoneNumber = gymPhoneNumber;
        this.gymScheduleString = gymScheduleString;

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

    public String getGymAddress() {
        return gymAddress;
    }

    public String getGymPhoneNumber() {
        return gymPhoneNumber;
    }

    public List getGymScheduleString() {
        return gymScheduleString;
    }


}
