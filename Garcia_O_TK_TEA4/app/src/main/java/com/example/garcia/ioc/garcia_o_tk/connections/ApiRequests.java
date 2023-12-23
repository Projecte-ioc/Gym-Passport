package com.example.garcia.ioc.garcia_o_tk.connections;


import android.content.Context;
import android.util.Log;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;
import com.example.garcia.ioc.garcia_o_tk.admin.GymInfo;
import com.nimbusds.jose.JOSEException;
import com.nimbusds.jose.JWEHeader;
import com.nimbusds.jose.JWEObject;
import com.nimbusds.jose.Payload;
import com.nimbusds.jose.crypto.DirectDecrypter;
import com.nimbusds.jose.util.Base64URL;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
import java.nio.charset.StandardCharsets;
import java.text.ParseException;
import java.util.HashMap;
import java.util.List;
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
        void onLoginSuccess(String token, Context context);

        void onLoginError(String error, Context context);
    }

    static KeyUtils keyUtils = new KeyUtils();


    /**
     * Métode per conectar-se a l'API de login mitjançant un tipus de sol·licitud POST, ens tornarà un TOKEN d'autenticació.
     *
     * @param context  context de l'aplicació.
     * @param username Nom d'usuari.
     * @param password Contrasenya d'usuari.
     * @param listener interficie.
     */
    public static void loginRequest(Context context, String username, String password, final ApiListener listener) throws Exception {

        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:4000/login";
        //Objecte JSON on afegirem l'usuari i la contrasenya.
        JSONObject jsonBody = new JSONObject();

        try {
            jsonBody.put("user_name", username);
            jsonBody.put("pswd_app", password);
            Log.d("js", jsonBody.toString());


        } catch (JSONException e) {
            e.printStackTrace();
        }
        try {
            String jwt = JWTDecoder.toJWT(jsonBody,context);
            Log.i("jwt",jwt);
            String jsonKey = keyUtils.encrypt(jwt,context);
            JSONObject encryptedJson = new JSONObject();
            encryptedJson.put("jwe", jsonKey);
            Log.i("objeto",encryptedJson.toString());
            //LLancem la sol·licitud que te per paràmetres el tipus, l'url de l'API i el JSON que hem creat.
            JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.POST, url, encryptedJson,
                    response -> {
                        Log.d("serverR", response.toString());
                        // Extraer el valor cifrado del campo "jwe"
                        String encryptedResponse = response.optString("jwe");

                        // Descifrar la respuesta cifrada JWE
                        try {
                            // Suponiendo que el método decrypt acepta la respuesta cifrada como parámetro
                            String decryptedResponse = KeyUtils.decrypt(encryptedResponse, context);
                            Log.d("serve", decryptedResponse);
                            listener.onLoginSuccess(decryptedResponse, context);
                        } catch (Exception e) {
                            e.printStackTrace();
                            listener.onLoginError("Error en el manejo de la respuesta", context);
                        }

                    },
                    error -> {
                        Log.e("Server Response", "Error: " + error.toString());

                    }
            );

            // Agrega la solicitud a la cola de solicitudes
            requestQueue.add(jsonObjectRequest);
        } catch (Exception e) {
            e.printStackTrace();
            throw new RuntimeException(e);
        }
    }


    /**
     * Métode per fer logout de l'API, segons el TOKEN que ens identifica com a usuari x.
     *
     * @param context   context de l'aplicació.
     * @param authToken TOKEN d'aunteticació tornat a la capçalera.
     * @param listener  interficie.
     */

    public static void logoutRequest(Context context, String authToken, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:4000/logout";
        String encryptedToken = keyUtils.encrypt(authToken, context);
        //LLancem la sol·licitud que te per paràmetres el tipus, l'url de l'API i el JSON que hem creat.
        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.PATCH, url, null,
                response -> {
                    //Si es correcte emmagatzemarem la resposta (TOKEN) en un String.
                    String token = response.toString();
                    Log.i("logoutk", token);
                    listener.onLoginSuccess(token, context);
                },
                error -> listener.onLoginError("Error en la solicitud", context)) {
            public Map<String, String> getHeaders() {
                //Métode que afegeix a al capçalera el TOKEN per a la autorització.
                Map<String, String> headers = new HashMap<>();
                headers.put("Authorization", encryptedToken);
                return headers;
            }


        };

        //Afegim la sol·licitud a la cua.
        requestQueue.add(jsonObjectRequest);
    }

    /**
     * Métode per conectar-se a l'API del perfil del gimnàs, el tipus de sol·licitud es GET. L'API rebra a la capçalera el token
     * que hem obtingut en el métode anterior per tal de diferenciar el perfil que fa la sol·lictud i tornar les dades apropiades.
     *
     * @param authToken token obtingut amb l'inici de sessió.
     * @param context   context de l'aplicació.
     * @param listener  interficie.
     */
    public static void getProfileInfo(Context context, String authToken, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:3000/profile_info";
        String encryptedToken = keyUtils.encrypt(authToken,context);
        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.GET, url, null,
                response -> {
                    Log.d("InfoPerfil:", response.toString());
                    String encryptedResponse = response.optString("jwe");
                    Log.d("Info:", encryptedResponse);
                    try {
                        String decryptedResponse = KeyUtils.decrypt(encryptedResponse, context);
                        Log.d("tk:", decryptedResponse);
                        GymInfo gymInfo = JWTDecoder.decodeUserGymInfo(decryptedResponse);
                        Log.d("info:", gymInfo.toString());
                        Log.d("Gym Info", "User Name: " + gymInfo.getUserName());

                        listener.onLoginSuccess(decryptedResponse, context);

                    } catch (JSONException e) {
                        Log.e("profileError", e.toString());
                        listener.onLoginError("Error de parsing", context);
                    } catch (Exception e) {
                        throw new RuntimeException(e);
                    }
                },
                error -> {
                    Log.e("Error perfil: ", error.toString());
                    listener.onLoginError("Error en la solicitud", context);

                }) {
            @Override
            public Map<String, String> getHeaders() {
                //Métode que afegeix a la capçalera el TOKEN per a la autorització.
                Map<String, String> headers = new HashMap<>();
                headers.put("Authorization", encryptedToken);
                return headers;
            }
        };

        requestQueue.add(jsonObjectRequest);
    }

    /**
     * Mètode per conectar-se a l'API per actualitzar les dades del gimnàs mitjançant un metode POST.
     * Es podràn modificar totes les dades menys el nom del gimnàs
     *
     * @param context        context de l'aplicació.
     * @param authToken      token obtingut amb l'inici de sessió.
     * @param newAddress     string amb la nova direcció.
     * @param newPhoneNumber string amb el nou telefon.
     * @param newSchedule    List amb el nou horari.
     * @param listener       interficie.
     */
    public static void updateGymData(Context context, String authToken, String newAddress, String newPhoneNumber, List newSchedule, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);

        String url = "http://10.2.190.11:2000/update_gym";
        String encryptedToken = keyUtils.encrypt(authToken, context);

        // L'API precisa d'un objecte JSON per poder modificar les dades, creem un objecte JSON on assignem els parells-clau y l'afegim l'array d'horaris..
        JSONObject jsonBody = new JSONObject();
        JSONArray jsonArray = new JSONArray(newSchedule);
        try {
            jsonBody.put("address", newAddress);
            jsonBody.put("phone_number", newPhoneNumber);
            jsonBody.put("schedule", jsonArray);
        } catch (JSONException e) {
            e.printStackTrace();
        }
        try {
            String jwt = JWTDecoder.toJWT(jsonBody, context);
            Log.i("jwt", jwt);
            String jsonKey = keyUtils.encrypt(jwt, context);
            JSONObject encryptedJson = new JSONObject();
            encryptedJson.put("jwe", jsonKey);
            Log.i("enc", encryptedJson.toString());


            JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.PATCH, url, encryptedJson,
                    response -> {
                        try {
                            String message = response.getString("message");
                            listener.onLoginSuccess(message, context);
                        } catch (JSONException e) {
                            listener.onLoginError("Error en la sol·licitud.", context);
                        }
                    },
                    error -> {
                        String errorMessage = "Error en la sol·licitud.";
                        if (error.networkResponse != null) {
                            errorMessage = "Error de red: " + error.networkResponse.statusCode;
                            Log.e("NetworkError", new String(error.networkResponse.data));
                        }
                        listener.onLoginError(errorMessage, context);
                    }) {
                @Override
                public Map<String, String> getHeaders() {
                    //Métode que afegeix a la capçalera el TOKEN per a la autorització.
                    Map<String, String> headers = new HashMap<>();
                    headers.put("Authorization", encryptedToken);
                    return headers;
                }
            };

            requestQueue.add(jsonObjectRequest);
        } catch (Exception e) {
            e.printStackTrace();
            throw new RuntimeException(e);
        }
    }

    /**
     * Métode per conectar-se a l'API que ens torna un llistat dels clients del gimnàs mitjançant una sol3licitud GET.
     * Només precisa del TOKEN d'autenticació per tornar les dades.
     *
     * @param context   context de l'aplicació.
     * @param authToken token obtingut amb l'inici de sessió.
     * @param listener  interficie.
     */
    public static void requestGymClients(Context context, String authToken, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);

        String url = "http://10.2.190.11:2000/consultar_clientes_gym";
        String encryptedToken = keyUtils.encrypt(authToken, context);

        JsonObjectRequest jsonObjectRequest= new JsonObjectRequest(Request.Method.GET, url, null,
                response -> {
                    Log.d("ServerResponse", "Response: " + response.toString());
                    String encryptedResponse = response.optString("jwe");
                    try {
                        String decryptedResponse = KeyUtils.decrypt(encryptedResponse, context);
                        Log.d("Response", decryptedResponse);
                        listener.onLoginSuccess(decryptedResponse, context);
                    } catch (Exception e) {
                        throw new RuntimeException(e);
                    }


                },
                error -> {

                    Log.d("ServerResponse", "Response: " + error.toString());
                    String errorMessage = "Error en la solicitud";
                    try {
                        if (error.networkResponse != null) {
                            errorMessage = "Error de red: " + error.networkResponse.statusCode;
                            Log.e("NetworkError", new String(error.networkResponse.data));
                        }
                    } catch (Exception e) {
                        Log.e("NetworkError", "Error al procesar la respuesta de la red");
                    }
                    listener.onLoginError(errorMessage,context);
                }) {
            @Override
            public Map<String, String> getHeaders() {
                //Métode que afegeix a la capçalera el TOKEN per a la autorització.
                Map<String, String> headers = new HashMap<>();
                headers.put("Authorization", encryptedToken);
                return headers;
            }
        };

        requestQueue.add(jsonObjectRequest);
    }

    /**
     * Métode per conectar-se a l'API que permet inserir clients mitjançant una sol·licitud POST.
     *
     * @param context   context de l'aplicació.
     * @param request   objecte JSON que contindrà les dades del nou usuari.
     * @param authToken token obtingut amb l'inici de sessió.
     * @param listener  interficie.
     */
    public static void insertClient(Context context, String authToken, JSONObject request, final ApiListener listener) {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:3000/insert_client";

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.POST, url, request,
                response -> {
                    listener.onLoginSuccess(response.toString(), context);
                },
                error -> {
                    Log.d("Error", error.toString());
                    listener.onLoginError("Error en la sol·licitud.", context);

                }) {
            @Override
            public Map<String, String> getHeaders() {
                //Métode que afegeix a la capçalera el TOKEN per a la autorització.
                Map<String, String> headers = new HashMap<>();
                headers.put("Authorization", authToken);
                return headers;
            }
        };

        requestQueue.add(jsonObjectRequest);
    }

    /**
     * Métode per conectar-se a l'API que permet eliminar clients mitjançant una sol·licitud delete.
     *
     * @param context   context de l'aplicació.
     * @param name      user_name de l'usuari a esborrar.
     * @param authToken token obtingut amb l'inici de sessió.
     * @param listener  interficie.
     */
    public static void deleteClient(Context context, String authToken, String name, final ApiListener listener) {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        try {
            // Codifiquem l'usuari per afegir-lo correctament a l'URL.
            String encodedName = URLEncoder.encode(name, "UTF-8");
            String url = "http://10.2.190.11:3000/delete_client?user_name=" + encodedName;

            JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.DELETE, url, null,
                    response -> {
                        Log.d("Usuari esborrat: ", response.toString());
                        listener.onLoginSuccess(response.toString(), context);
                    },
                    error -> {
                        Log.e("Error en esborrar client: ", error.toString());
                        listener.onLoginError("Error en la solicitud", context);
                    }) {
                @Override
                public Map<String, String> getHeaders() {
                    // Método que añade a la cabecera el TOKEN para la autorización.
                    Map<String, String> headers = new HashMap<>();
                    headers.put("Authorization", authToken);
                    return headers;
                }
            };

            requestQueue.add(jsonObjectRequest);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
            listener.onLoginError("Error en la sol·licitud.", context);
        }
    }

    /**
     * @param
     * @param
     * @param
     * @param
     */
    public static void updateClient(Context context, String authToken, JSONObject request, final ApiListener listener) {
        RequestQueue requestQueue = Volley.newRequestQueue(context);

        String url = "http://10.2.190.11:3000/update_data_client";

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.PUT, url, request,
                response -> {
                    Log.d("Usuari actualitzat: ", response.toString());
                    listener.onLoginSuccess(response.toString(), context);
                },
                error -> {
                    Log.e("Error en actualitzar client: ", error.toString());
                    listener.onLoginError("Error en la solicitud", context);
                }) {
            @Override
            public Map<String, String> getHeaders() {
                // Método que añade a la cabecera el TOKEN para la autorización.
                Map<String, String> headers = new HashMap<>();
                headers.put("Authorization", authToken);
                return headers;
            }
        };

        requestQueue.add(jsonObjectRequest);
    }
}




