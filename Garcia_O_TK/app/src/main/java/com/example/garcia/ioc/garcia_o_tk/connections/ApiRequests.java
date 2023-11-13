package com.example.garcia.ioc.garcia_o_tk.connections;


import android.content.Context;
import android.util.Log;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;

/**
 * author: Claudio Garcia Otero.
 * Classe per gestionar les sol·licituds a l'API.
 */
public class ApiRequests {

    /**
     * Interfície que controla els events possibles (success o error).
     */
    public interface ApiListener {
        void onLoginSuccess(String token);
        void onLoginError(String error);
    }

    /**
     * Métode per conectar-se a l'API de login mitjançant un tipus de sol·licitud POST, ens tornarà un TOKEN d'autenticació.
     *
     * @param context context de l'aplicació.
     * @param username Nom d'usuari.
     * @param password Contrasenya d'usuari.
     * @param listener interficie.
     */
    public static void loginRequest(Context context, String username, String password, final ApiListener listener) {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:4000/login";
        //Objecte JSON on afegirem l'usuari i la contrasenya.
        JSONObject jsonBody = new JSONObject();
        try {
            jsonBody.put("pswd_app", password);
            jsonBody.put("user_name", username);
            Log.d("js", jsonBody.toString());
        } catch (JSONException e) {
            e.printStackTrace();
        }
       //LLancem la sol·licitud que te per paràmetres el tipus, l'url de l'API i el JSON que hem creat.
        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.POST, url, jsonBody,
                response -> {
                    try {
                        //Si es correcte emmagatzemarem la resposta (TOKEN) en un String.
                        String token = response.getString("token");
                        Log.i("tk",token);
                        listener.onLoginSuccess(token);
                    } catch (JSONException e) {
                        listener.onLoginError("Error de parsing.");
                    }
                },
                error -> listener.onLoginError("Error en la solicitud"));

        //Afegim la sol·licitud a la cua.
        requestQueue.add(jsonObjectRequest);
    }

    /**
     * Métode per conectar-se a l'API del perfil del gimnàs, el tipus de sol·licitud es GET. L'API rebra a la capçalera el token
     * que hem obtingut en el métode anterior per tal de diferenciar el perfil que fa la sol·lictud i tornar les dades apropiades.
     *
     * @param authToken token obtingut amb l'inici de sessió.
     * @param context context de l'aplicació.
     * @param listener interficie.
     */
    public static void getProfileInfo(Context context, String authToken, final ApiListener listener) {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:2000/profile_info";

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.GET, url, null,
                response -> {
                    try {
                        // Si tot es correcte, obtenim les dades del perfil.
                        String adToken = response.getString("adtoken");
                        listener.onLoginSuccess(adToken);
                    } catch (JSONException e) {
                        listener.onLoginError("Error de parsing");
                    }
                },
                error -> {
                    listener.onLoginError("Error en la solicitud");

                }) {
            @Override
            public Map<String, String> getHeaders() {
                //Métode que afegeix a al capçalera el TOKEN per a la autorització.
                Map<String, String> headers = new HashMap<>();
                headers.put("Authorization", authToken);
                return headers;
            }
        };

        requestQueue.add(jsonObjectRequest);
    }
}


