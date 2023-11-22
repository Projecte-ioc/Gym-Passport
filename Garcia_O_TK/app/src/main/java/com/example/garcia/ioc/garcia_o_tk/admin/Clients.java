package com.example.garcia.ioc.garcia_o_tk.admin;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class Clients extends AppCompatActivity {

    private final List<String> SELECTED_VALUES= new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_clients);

        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");

        RecyclerView recyclerView = findViewById(R.id.clients_list);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));


        ExecutorService executor = Executors.newSingleThreadExecutor();
        executor.execute(() -> ApiRequests.requestGymClients(Clients.this, authToken, new ApiRequests.ApiListener() {
            @Override
            public void onLoginSuccess(String token) {

                Log.i("tk",token);

                runOnUiThread(() -> {

                    try {
                        JSONArray jsonArray = new JSONArray(token);
                        List<ClientsInfo> clients = new ArrayList<>();

                        for (int i = 0; i < jsonArray.length(); i++) {
                            JSONObject jsonObject = jsonArray.getJSONObject(i);
                            ClientsInfo client = new ClientsInfo(
                                    jsonObject.getInt("gym_id"),
                                    jsonObject.getInt("id"),
                                    jsonObject.getInt("log"),
                                    jsonObject.getString("name"),
                                    jsonObject.getString("password"),
                                    jsonObject.getString("role"),
                                    jsonObject.getString("user_name")
                            );

                            clients.add(client);
                        }

                        Log.i("Selected Values: ", clients.toString());

                        recyclerView.setLayoutManager(new LinearLayoutManager(Clients.this));

                        // Configurar el adaptador
                        ClientsAdapter adapter = new ClientsAdapter(clients);
                        recyclerView.setAdapter(adapter);

                        // Actualizar los datos en el adaptador
                        adapter.setClients(clients);

                        // Notificar al RecyclerView que los datos han cambiado
                        adapter.notifyDataSetChanged();
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }

                });
            }

            @Override
            public void onLoginError(String error) {

                runOnUiThread(() -> Log.e("Client: ",error));
            }
        }));
    }
    public void close(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), AdminAct.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);


    }



}