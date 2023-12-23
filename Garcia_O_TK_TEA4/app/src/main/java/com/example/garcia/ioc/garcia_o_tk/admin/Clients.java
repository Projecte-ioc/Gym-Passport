package com.example.garcia.ioc.garcia_o_tk.admin;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;
import com.example.garcia.ioc.garcia_o_tk.connections.JWTDecoder;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

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
                                JSONArray jsonArray = new JSONArray(token);
                                List<ClientsInfo> clientsInfoList = JWTDecoder.decodeClientsInfoList(token);
                                Log.d("list",clientsInfoList.toString());
                                //Llista amb un objecte ClientsInfo.
                                List<ClientsInfo> clients = new ArrayList<>();
                                //Per cada element de la llista que representa un usuari, recorrem el JSONarray i creem un JSONobject.
                                for (int i = 0; i < jsonArray.length(); i++) {
                                    JSONObject jsonObject = jsonArray.getJSONObject(i);
                                    //Afegim a cada JSONobject les dades desitjades creant un objecte ClientsInfo per cada usuari.
                                    ClientsInfo client = new ClientsInfo(
                                            jsonObject.getInt("gym_id"),
                                            jsonObject.getInt("id"),
                                            jsonObject.getInt("log"),
                                            jsonObject.getString("name"),
                                            jsonObject.getString("password"),
                                            jsonObject.getString("role_user"),
                                            jsonObject.getString("user_name")
                                    );
                                    //Afegim cada objecte a la llista.
                                    clients.add(client);
                                }
                                recyclerView.setLayoutManager(new LinearLayoutManager(Clients.this));

                                //Nova instancia de l'adaptador passant per parametre la llista de clients.
                                ClientsAdapter adapter = new ClientsAdapter(clients);
                                recyclerView.setAdapter(adapter);
                                // Actualitzem les dades.
                                adapter.setClients(clients);
                                // Notifiquem els canvis.
                                adapter.notifyDataSetChanged();
                            } catch (JSONException e) {
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