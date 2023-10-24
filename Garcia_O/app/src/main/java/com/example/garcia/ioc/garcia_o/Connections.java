package com.example.garcia.ioc.garcia_o;


import android.content.Context;
import android.util.Log;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

/**
 * Author: Claudio Garcia Otero.
 * Classe encarregada de les connexions, de moment només te un mètode per obtenir
 * l'array JSON per poder fer el login. L'array es un mock que he fet servir per simular les connexions.
 */
public class Connections {

    private final String apiUrl;

    private int tipoRespuesta;

    public Connections(String apiUrl) {
        this.apiUrl = apiUrl;

    }

    /**
     * Realitza la sol·licitud a l'API (pel moment JSONServer).
     *
     * @param user     Nom de l'usuari.
     * @param userPsw  Contrasenya.
     * @param callback interficie que controla la resposta.
     */
    public void logFrom(String user, String userPsw, final Callback callback) {

        //Fem servir la biblioteca volley per fer una sol·licitud a la API.
        RequestQueue requestQueue = Volley.newRequestQueue(callback.getContext());
        //Mitjançant la sol·licitud GET i la url obtenim l'array JSON.
        JsonArrayRequest jsonArrayRequest = new JsonArrayRequest(Request.Method.GET, apiUrl, null,
                new Response.Listener<JSONArray>() {

                    @Override
                    //Controlem la resposta.
                    public void onResponse(JSONArray response) {
                        try {
                            //Indico la posició de l'array on es troben les credencials oportunes.
                            JSONObject jsonObject0 = response.getJSONObject(0);
                            JSONObject jsonObject1 = response.getJSONObject(1);
                            //Asignem els valors a 2 tipus de Strings diferents, un per usuari admin i
                            //l'altre per usuari normal.
                            String nombre = jsonObject0.getString("nombre");
                            String ps = jsonObject0.getString("pswd-app");

                            String nombre1 = jsonObject1.getString("nombre");
                            String ps1 = jsonObject1.getString("pswd-app");
                            //Si l'introdüit per l'usuari coincideix amb admin, tornem 1, si es usuari normal 2,
                            //i si no coincideix amb cap, 0.
                            if ((user.equals(nombre)) && (userPsw.equals(ps))) {
                                tipoRespuesta = 1;

                            } else if ((user.equals(nombre1)) && (userPsw.equals(ps1))) {
                                tipoRespuesta = 2;
                            } else {
                                tipoRespuesta = 0;

                            }
                            //Asignem el tipus de resposta el mètode de la interficie "onResponseReceived".
                            callback.onResponseReceived(tipoRespuesta);

                        } catch (JSONException e) {
                            e.printStackTrace();
                            Log.e("Error Resp:", "Error: " + e.getMessage());
                            //Control·lem els errors en el tipus de resposta obtinguda.
                        }
                    }
                },
                new Response.ErrorListener() {
                    @Override
                    public void onErrorResponse(VolleyError error) {
                        String fail= error.getMessage();
                        callback.onErrorResponse(fail );
                        assert fail != null;
                        Log.e("Error Conn:", fail);
                        //Control·lem els errors en la connexió.

                    }
                });

        //Afegim a la cua de sol·licituds
        requestQueue.add(jsonArrayRequest);


    }

    public interface Callback {
        /**
         * @param tipoRespuesta Tipus de resposta per iniciar sessió.
         */
        void onResponseReceived(int tipoRespuesta);

        /**
         * @param errorMessage Missatge d'error en la sol·licitud.
         */
        void onErrorResponse(String errorMessage);

        /**
         * @return Obté el context de l'aplicació on s'esta cridant el mètode.
         */
        Context getContext();
    }


}
