package com.example.garcia.ioc.garcia_o_tk.connections;


import android.content.Context;
import android.util.Log;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.Volley;
import com.example.garcia.ioc.garcia_o_tk.admin.perfil.GymInfo;
import com.example.garcia.ioc.garcia_o_tk.encription.JWTDecoder;
import com.example.garcia.ioc.garcia_o_tk.encription.KeyUtils;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;
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
    public static void loginRequest(Context context, String username, String password, final ApiListener listener) {

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
            //Primer convertim l'objecte JSON en un TOKEN.
            String jwt = JWTDecoder.toJWT(jsonBody,context);
            //Tag per comprovar si hem convertit correctament en TOKEN.
            Log.i("loginTK: ",jwt);
            //Cridem el métode de keyUtils per encriptar.
            String jsonKey = keyUtils.encrypt(jwt,context);
            //Tag per comprovar que el TOKEN ha sigut encriptat.
            Log.i("encriptedJWT: ", jsonKey);
            //Donat que les requests de Volley exigeixen per paràmetre un objecte JSON, hem de crear un amb el contingut encriptat.
            JSONObject encryptedJson = new JSONObject();
            encryptedJson.put("jwe", jsonKey);
            //LLancem la sol·licitud que te per paràmetres el tipus, l'url de l'API i el JSON que hem creat.
            JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.POST, url, encryptedJson,
                    response -> {
                        //Tag per comprovar que la resposta del server es la esperada.
                        Log.d("loginOk: ", response.toString());
                        //Primer hem d'extreure el contingut encriptat de l'objecte JSON.
                        String encryptedResponse = response.optString("jwe");
                        try {
                            //Desxifrem la resposta amb el métode decrypt de keyUtils.
                            String decryptedResponse = KeyUtils.decrypt(encryptedResponse, context);
                            //Tag per comprovar la resposta desxifrada.
                            Log.d("loginOkdecrypted:", decryptedResponse);
                            listener.onLoginSuccess(decryptedResponse, context);
                        } catch (Exception e) {
                            Log.e("decryptErr: ", e.toString());
                            e.printStackTrace();
                            listener.onLoginError("Error en desxifrar.", context);
                        }

                    },
                    error -> {
                        Log.e("logiErr:", "Error: " + error.toString());

                    }
            );
            //Afegim la solicitud a la cua.
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
        //Encriptem el TOKEN d'autenticació per protegir les dades.
        String encryptedToken = keyUtils.encrypt(authToken, context);
        //LLancem la sol·licitud que te per paràmetres el tipus, l'url de l'API i el JSON que hem creat.
        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.PATCH, url, null,
                response -> {
                    //Si es correcte emmagatzemarem la resposta (TOKEN) en un String.
                    String token = response.toString();
                    listener.onLoginSuccess(token, context);
                },
                error -> listener.onLoginError("Error en la solicitud", context)) {
            public Map<String, String> getHeaders() {
                //Métode que afegeix a al capçalera el TOKEN per a la autorització.
                Map<String, String> headers = new HashMap<>();
                //Passem pel header el TOKEN encriptat.
                headers.put("Authorization", encryptedToken);
                //Tag per verificar que el header ha sigut encriptat.
                Log.i("encriptedHeader: ",headers.toString());
                return headers;
            }
        };
        //Afegim la solicitud a la cua.
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
        //Encriptem el TOKEN d'autenticació per protegir les dades.
        String encryptedToken = keyUtils.encrypt(authToken,context);
        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.GET, url, null,
                response -> {
                    Log.d("getProfileInfoOk:", response.toString());
                    //Extreiem el contingut xifrat de l'objecte JSON.
                    String encryptedResponse = response.optString("jwe");
                    try {
                        //Desxifrem el contingut amb el métode decrypt de keyUtils.
                        String decryptedResponse = KeyUtils.decrypt(encryptedResponse, context);
                        //Tag per comprovar si hem extret el TOKEN.
                        Log.d("getProfileInfoTK:", decryptedResponse);
                        listener.onLoginSuccess(decryptedResponse, context);
                    } catch (JSONException e) {
                        Log.e("getProfileInfoTKerror: ", e.toString());
                        listener.onLoginError("Error en extreure el TOKEN.", context);
                    } catch (Exception e) {
                        throw new RuntimeException(e);
                    }
                },
                error -> {
                    Log.e("getProfileInfoError: ", error.toString());
                    listener.onLoginError("Error en la solicitud", context);

                }) {
            @Override
            public Map<String, String> getHeaders() {
                //Métode que afegeix a la capçalera el TOKEN per a la autorització.
                Map<String, String> headers = new HashMap<>();
                //Passem pel header el TOKEN encriptat.
                headers.put("Authorization", encryptedToken);
                Log.i("encriptedHeader: ",headers.toString());
                return headers;
            }
        };
        //Afegim la solicitud a la cua.
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
        //Encriptem el TOKEN d'autenticació per protegir les dades.
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
            //Convertim en TOKEN l'objecte JSON.
            String jwt = JWTDecoder.toJWT(jsonBody, context);
            //Tag per comprovar si s'ha fet la conversió correctament.
            Log.i("updateGymDataTK: ", jwt);
            //Encriptem el TOKEN per protegir les dades.
            String jsonKey = keyUtils.encrypt(jwt, context);
            JSONObject encryptedJson = new JSONObject();
            encryptedJson.put("jwe", jsonKey);
            Log.i("updateGymDataTKencrypted: ", encryptedJson.toString());
            JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.PATCH, url, encryptedJson,
                    response -> {
                        Log.i("updateGymDataOk: ", response.toString());
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
                    //Passem pel header el TOKEN encriptat.
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
     * Métode per conectar-se a l'API que ens torna un llistat dels clients del gimnàs mitjançant una solicitud GET.
     * Només precisa del TOKEN d'autenticació per tornar les dades.
     *
     * @param context   context de l'aplicació.
     * @param authToken token obtingut amb l'inici de sessió.
     * @param listener  interficie.
     */
    public static void requestGymClients(Context context, String authToken, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:2000/consultar_clientes_gym";
        //Encriptem el TOKEN d'autenticació per protegir les dades.
        String encryptedToken = keyUtils.encrypt(authToken, context);
        JsonObjectRequest jsonObjectRequest= new JsonObjectRequest(Request.Method.GET, url, null,
                response -> {
                    Log.d("requestGymClientsOk:",  response.toString());
                    String encryptedResponse = response.optString("jwe");
                    try {
                        //Desxifrem el contingut amb el métode decrypt de keyUtils.
                        String decryptedResponse = KeyUtils.decrypt(encryptedResponse, context);
                        listener.onLoginSuccess(decryptedResponse, context);
                    } catch (Exception e) {
                        throw new RuntimeException(e);
                    }
                    },
                error -> {
                    Log.d("requestGymClientsError:",  error.toString());
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
                //Passem pel header el TOKEN encriptat.
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
    public static void insertClient(Context context, String authToken, JSONObject request, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:3000/insert_client";
        //Encriptem el TOKEN d'autenticació per protegir les dades.
        String encryptedToken = keyUtils.encrypt(authToken, context);
        //Convertim en TOKEN l'objecte JSON.
        String jwt = JWTDecoder.toJWT(request,context);
        Log.i("insertClientTK",jwt);
        //Encriptem el TOKEN.
        String jsonKey = keyUtils.encrypt(jwt,context);
        JSONObject encryptedJson = new JSONObject();
        encryptedJson.put("jwe", jsonKey);

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.POST, url, encryptedJson,
                response -> {
                    Log.i("insertClientOk: ", response.toString());
                    listener.onLoginSuccess(response.toString(), context);
                },
                error -> {
                    Log.d("insertClientError:", error.toString());
                    listener.onLoginError("Error en la sol·licitud.", context);
                    if (error.networkResponse != null) {
                        Log.e("AppLogs", "Codi de resposta: " + error.networkResponse.statusCode);
                        try {
                            String responseBody = new String(error.networkResponse.data, "utf-8");
                            Log.e("AppLogs", "Contingut de l'error: " + responseBody);
                        } catch (UnsupportedEncodingException e) {
                            e.printStackTrace();
                        }
                    }

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
     * Métode per conectar-se a l'API que permet eliminar clients mitjançant una solicitud delete.
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
            String encryptedToken = keyUtils.encrypt(authToken, context);

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

                    Map<String, String> headers = new HashMap<>();
                    headers.put("Authorization", encryptedToken);
                    return headers;
                }
            };

            requestQueue.add(jsonObjectRequest);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
            listener.onLoginError("Error en la solicitud.", context);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    /** Métode per conectar-se a l'API que permet modificar el nom i la contrasenya a un usuari normal. Fa servir un métode put
     * i passem per paràmetre un objecte JSON amb les dades requerides.
     * @param context context de l'aplicació.
     * @param authToken TOKEN d'auntenticació.
     * @param request objecte JSON  amb les dades a modificar.
     * @param listener interficie.
     */
    public static void updateClient(Context context, String authToken, JSONObject request, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);

        String url = "http://10.2.190.11:3000/update_data_client";
        //Xifrem el TOKEN d'aunteticació.
        String encryptedToken = keyUtils.encrypt(authToken, context);
        //Passem a TOKEN l'objecte JSON.
        String jwt = JWTDecoder.toJWT(request,context);
        //Tag per comprovar que el TOKEN sigui correcte.
        Log.i("updateClientTK: ",jwt);
        //Xifrem el TOKEN.
        String jsonKey = keyUtils.encrypt(jwt,context);
        Log.i("updateClientTKencrypted: ",jsonKey);
        JSONObject encryptedJson = new JSONObject();
        encryptedJson.put("jwe", jsonKey);

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.PUT, url, encryptedJson,
                response -> {
                    Log.d("updateClientOk: ", response.toString());
                    listener.onLoginSuccess(response.toString(), context);
                },
                error -> {
                    Log.e("updateClientError: ", error.toString());
                    listener.onLoginError("Error en la solicitud", context);
                }) {
            @Override
            public Map<String, String> getHeaders() {
                //Afegim el token xifrat al header.
                Map<String, String> headers = new HashMap<>();
                headers.put("Authorization", encryptedToken);
                return headers;
            }
        };

        requestQueue.add(jsonObjectRequest);
    }

    /** Métode que es conecta a l'API que retorna el llistat dels events existents. Fa servir una solicitud GET i passem per paràmetre
     *  el TOKEN d'autenticació.
     * @param context context de l'aplicació.
     * @param authToken TOKEN d'auntenticació.
     * @param listener interficie.
     */

    public static void requestEvents(Context context, String authToken, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);

        String url = "http://10.2.190.11:6000/obtener_eventos";
        //Xifrem el TOKEN d'aunteticació.
        String encryptedToken = keyUtils.encrypt(authToken, context);

        JsonObjectRequest jsonObjectRequest= new JsonObjectRequest(Request.Method.GET, url, null,
                response -> {
                    Log.d("requestEventsOk: ",  response.toString());
                    String encryptedResponse = response.optString("jwe");
                    try {
                        String decryptedResponse = KeyUtils.decrypt(encryptedResponse, context);
                        Log.d("Event", decryptedResponse);
                        listener.onLoginSuccess(decryptedResponse, context);
                    } catch (Exception e) {
                        throw new RuntimeException(e);
                    }
                    },
                error -> {

                    Log.d("requestEventsError: ",  error.toString());
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

    /** Métode que es conecta a l'API per inserir un event nou. Fa servir una solicitud POST i passem per parametre les dades amb l'event nou.
     * @param context context de l'aplicació.
     * @param authToken TOKEN d'auntenticació.
     * @param request objecte JSON  amb les dades a inserir.
     * @param listener interficie.
     */
    public static void insertEvent(Context context, String authToken, JSONObject request, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:6000/insertar_evento";
        //Xifrem el TOKEN d'aunteticació.
        String encryptedToken = keyUtils.encrypt(authToken, context);
        //Passem a TOKEN l'objecte JSON.
        String jwt = JWTDecoder.toJWT(request,context);
        Log.i("insertEventTK: ",jwt);
        //Xifrem el TOKEN.
        String jsonKey = keyUtils.encrypt(jwt,context);
        Log.i("insertEventTKencripted: ",jsonKey);
        JSONObject encryptedJson = new JSONObject();
        encryptedJson.put("jwe", jsonKey);

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.POST, url, encryptedJson,
                response -> {
                    Log.i("insertEventOk: ", response.toString());
                    listener.onLoginSuccess(response.toString(), context);
                },
                error -> {
                    Log.d("insertEventError: ", error.toString());
                    listener.onLoginError("Error en la solicitud.", context);
                    if (error.networkResponse != null) {
                        Log.e("AppLogs", "Codi de resposta: " + error.networkResponse.statusCode);
                        try {
                            String responseBody = new String(error.networkResponse.data, "utf-8");
                            Log.e("AppLogs", "Contingut de l'error: " + responseBody);
                        } catch (UnsupportedEncodingException e) {
                            e.printStackTrace();
                        }
                    }

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

    /** Métode que es conecta a l'API per esborrar un event. Fa servir una solicitud DELETE i passem per paràmetre l'id
     *  identificador de l'event.
     * @param context context de l'aplicació.
     * @param authToken TOKEN d'auntenticació.
     * @param id string amb l'id identificador de l'event.
     * @param listener interficie.
     */

    public static void deleteEvent(Context context, String authToken, String id, final ApiListener listener) {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        try {
            // Codifiquem l'idper afegir-lo correctament a l'URL.
            String encodedId = URLEncoder.encode(id, "UTF-8");

            String url = "http://10.2.190.11:6000/eliminar_evento?event_id="+id ;
            //Xifrem el TOKEN d'aunteticació.
            String encryptedToken = keyUtils.encrypt(authToken, context);

            JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.DELETE, url, null,
                    response -> {
                        Log.d("event esborrat: ", response.toString());
                        listener.onLoginSuccess(response.toString(), context);
                    },
                    error -> {
                        Log.e("Error en esborrar event: ", error.toString());
                        listener.onLoginError("Error en la solicitud", context);
                    }) {
                @Override
                public Map<String, String> getHeaders() {

                    Map<String, String> headers = new HashMap<>();
                    headers.put("Authorization", encryptedToken);
                    return headers;
                }
            };

            requestQueue.add(jsonObjectRequest);
        } catch (UnsupportedEncodingException e) {
            e.printStackTrace();
            listener.onLoginError("Error en la sol·licitud.", context);
        } catch (Exception e) {
            throw new RuntimeException(e);
        }
    }

    /** Métode que es conecta a l'API per modificar events. Fa servir una solicitud PATCH i passem per paràmetre
     *  l'id identificador de l'event.
     * @param context context de l'aplicació.
     * @param authToken TOKEN d'auntenticació.
     * @param id objecte JSON  amb l'id identificador de l'event.
     * @param listener interficie.
     */

    public static void modEvent(Context context, String authToken,String id, JSONObject request, final ApiListener listener) throws Exception {
        RequestQueue requestQueue = Volley.newRequestQueue(context);
        String url = "http://10.2.190.11:6000/modificar_evento?event_id="+id;
        //Xifrem el TOKEN d'aunteticació.
        String encryptedToken = keyUtils.encrypt(authToken, context);
        //Passem a TOKEN l'objecte JSON.
        String jwt = JWTDecoder.toJWT(request,context);
        Log.i("modEventTK:",jwt);
        //Xifrem el TOKEN.
        String jsonKey = keyUtils.encrypt(jwt,context);
        Log.i("modEventTKencrypted: ",jsonKey);
        JSONObject encryptedJson = new JSONObject();
        encryptedJson.put("jwe", jsonKey);

        JsonObjectRequest jsonObjectRequest = new JsonObjectRequest(Request.Method.PATCH, url, encryptedJson,
                response -> {
                    listener.onLoginSuccess(response.toString(), context);
                },
                error -> {
                    Log.d("Insert Error", error.toString());
                    listener.onLoginError("Error en la solicitud.", context);
                    if (error.networkResponse != null) {
                        Log.e("AppLogs", "Codi de resposta: " + error.networkResponse.statusCode);
                        try {
                            String responseBody = new String(error.networkResponse.data, "utf-8");
                            Log.e("AppLogs", "Contingut de l'error: " + responseBody);
                        } catch (UnsupportedEncodingException e) {
                            e.printStackTrace();
                        }
                    }

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




}




