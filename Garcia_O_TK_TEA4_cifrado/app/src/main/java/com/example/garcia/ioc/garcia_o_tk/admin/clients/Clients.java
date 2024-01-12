package com.example.garcia.ioc.garcia_o_tk.admin.clients;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.admin.AdminAct;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;
import com.example.garcia.ioc.garcia_o_tk.encription.JWTDecoder;
import com.nimbusds.jwt.JWT;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.text.ParseException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * author: Claudio Garcia Otero.
 * Classe que ens tornarà un llistat amb tots els clients/usuaris del gimnàs.
 */

public class Clients extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_clients);
        //Obtenim el TOKEN de l'activitat principal
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        //Instanciem el RecyclerView per mostrar les dades en forma de llistat
        RecyclerView recyclerView = findViewById(R.id.clients_list);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
        //Nou fil per fer la sol·licitud a la API.
        ExecutorService executor = Executors.newSingleThreadExecutor();
        executor.execute(() -> {
            try {
                ApiRequests.requestGymClients(Clients.this, authToken, new ApiRequests.ApiListener() {
                    @Override
                    public void onLoginSuccess(String token, Context context) {
                        //Tornem al fil de la interfciie per fer servir la resposta.
                        runOnUiThread(() -> {

                            try {
                                JWT jwt = JWTDecoder.decodeJWT(token);

                                if (jwt != null) {

                                    // Accede a las claims individualmente
                                    List<JSONObject> clientsJsonList = new ArrayList<>();

                                    // Obtén las claims como un String JSON
                                    String claimsJsonString = jwt.getJWTClaimsSet().toString();
                                    Log.d("Claims", claimsJsonString);
                                    try {
                                        // Convertir el String JSON a un JSONObject
                                        JSONObject claimsJsonObject = new JSONObject(claimsJsonString);

                                        // Obtener el JSONArray dentro de la clave "results"
                                        JSONArray jsonArray = claimsJsonObject.getJSONArray("results");

                                        // Iterar sobre el JSONArray y extraer solo las claims deseadas
                                        for (int i = 0; i < jsonArray.length(); i++) {
                                            JSONObject jsonObject = jsonArray.getJSONObject(i);

                                            // Crea un nuevo JSONObject con las claims deseadas
                                            JSONObject clientJson = new JSONObject();
                                            clientJson.put("name", jsonObject.getString("name"));
                                            clientJson.put("role_user", jsonObject.getString("role_user"));
                                            clientJson.put("user_name", jsonObject.getString("user_name"));

                                            Log.d("Client JSON", clientJson.toString());

                                            // Añade el nuevo JSONObject a la lista
                                            clientsJsonList.add(clientJson);
                                        }
                                    } catch (JSONException e) {
                                        e.printStackTrace();
                                        Log.e("Error parsing JSONArray", e.getMessage());
                                    }
                                    // Crea la lista de ClientsInfo a partir de los JSONObjects
                                    List<ClientsInfo> clients = new ArrayList<>();
                                    for (JSONObject clientJson : clientsJsonList) {

                                        ClientsInfo client = new ClientsInfo(

                                                clientJson.getString("name"),
                                                clientJson.getString("role_user"),
                                                clientJson.getString("user_name")
                                        );
                                        clients.add(client);
                                        Log.d("Client ", clientJson.toString());
                                    }
                                    // Configura el RecyclerView y el adaptador
                                    recyclerView.setLayoutManager(new LinearLayoutManager(Clients.this));
                                    ClientsAdapter adapter = new ClientsAdapter(clients);
                                    recyclerView.setAdapter(adapter);

                                    // Actualiza y notifica cambios en el adaptador
                                    adapter.setClients(clients);
                                    adapter.notifyDataSetChanged();
                                } else {
                                    Log.e("Error decoding JWT", "JWT es nulo");
                                }
                            } catch (JSONException | ParseException e) {
                                e.printStackTrace();
                            }

                        });
                    }

                    @Override
                    public void onLoginError(String error, Context context) {

                        runOnUiThread(() -> Log.e("Client: ",error));
                    }
                });
            } catch (Exception e) {
                throw new RuntimeException(e);
            }
        });
    }
    //Mètode que torna el TOKEN a l'activitat principal.
    public void close(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), AdminAct.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);


    }



}