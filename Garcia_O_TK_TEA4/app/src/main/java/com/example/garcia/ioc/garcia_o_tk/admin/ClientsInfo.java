package com.example.garcia.ioc.garcia_o_tk.admin;
public class ClientsInfo {


    private int gymId;
    private int id;
    private int log;
    private String name;
    private String password;
    private String role;
    private String userName;



    public ClientsInfo(int gymId, int id, int log, String name, String password, String role, String userName ) {


        this.gymId = gymId;
        this.id = id;
        this.log = log;
        this.name = name;
        this.password = password;
        this.role = role;
        this.userName = userName;
    }
    public int getGymId() {
        return gymId;
    }
    public int getId() {
        return id;
    }

    public int getLog() {
        return log;
    }

    public String getName() {
        return name;
    }

    public String getPassword() {
        return password;
    }

    public String getRole() {
        return role;
    }

    public String getUserName() {
        return userName;
    }

    public void setGymId(int gymId) {
        this.gymId = gymId;
    }

    public void setId(int id) {
        this.id = id;
    }

    public void setLog(int log) {
        this.log = log;
    }

    public void setName(String name) {
        this.name = name;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public void setRole(String role) {
        this.role = role;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }




}
