package com.example.garcia.ioc.garcia_o_tk.admin.clients;
public class ClientsInfo {



    private String name;
    private String role;
    private String userName;



    public ClientsInfo( String name,  String role, String userName ) {

        this.name = name;
        this.role = role;
        this.userName = userName;
    }


    public String getName() {
        return name;
    }



    public String getRole() {
        return role;
    }

    public String getUserName() {
        return userName;
    }





    public void setName(String name) {
        this.name = name;
    }







}
