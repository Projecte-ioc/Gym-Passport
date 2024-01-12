package com.example.garcia.ioc.garcia_o_tk.admin.events;

import androidx.appcompat.app.AppCompatActivity;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;

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
 * author: Claudio Garcia.
 * Activity per mostrar en cardViews el llistat d'events existents.
 */

public class EventsList extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_events_list);
        //Obtenim el TOKEN de l'activitat principal
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        //Instanciem el RecyclerView per mostrar les dades en forma de llistat
        RecyclerView recyclerView = findViewById(R.id.Events_list);
        recyclerView.setLayoutManager(new LinearLayoutManager(this));
        //Nou fil per fer la sol·licitud a la API.
        ExecutorService executor = Executors.newSingleThreadExecutor();
        executor.execute(() -> {
            try {
                ApiRequests.requestEvents(EventsList.this, authToken, new ApiRequests.ApiListener() {
                    @Override
                    public void onLoginSuccess(String token, Context context) {
                        //Tornem al fil de la interfciie per fer servir la resposta.
                        runOnUiThread(() -> {

                            try {
                                //Decodifiquem el TOKEN obtingut.
                                JWT jwt = JWTDecoder.decodeJWT(token);
                                if (jwt != null) {

                                    List<JSONObject> eventsJsonList = new ArrayList<>();
                                    //Obtenim les claims com un string.
                                    String claimsJsonString = jwt.getJWTClaimsSet().toString();
                                    Log.d("Claims", claimsJsonString);
                                    try {
                                        //Convertim les claims en un nou objecte JSON.
                                        JSONObject claimsJsonObject = new JSONObject(claimsJsonString);

                                        JSONArray jsonArray = claimsJsonObject.getJSONArray("results");

                                        //Iterem sobre l'array i obtenim les claims
                                        for (int i = 0; i < jsonArray.length(); i++) {
                                            JSONObject jsonObject = jsonArray.getJSONObject(i);
                                            //Creem un nou objecte per cada fila de dades.
                                            JSONObject eventJson = new JSONObject();
                                            eventJson.put("name", jsonObject.getString("name"));
                                            eventJson.put("date", jsonObject.getString("date"));
                                            eventJson.put("hour", jsonObject.getString("hour"));
                                            eventJson.put("qty_max_attendes", jsonObject.getString("qty_max_attendes"));
                                            eventJson.put("whereisit", jsonObject.getString("whereisit"));

                                            Log.d("Client JSON", eventJson.toString());

                                            //Afegim l'objecte a la llista.
                                            eventsJsonList.add(eventJson);
                                        }
                                    } catch (JSONException e) {
                                        e.printStackTrace();
                                        Log.e("Error parsing JSONArray", e.getMessage());
                                    }
                                    //creem un List de l'objecte EventsInfo
                                    List<EventsInfo> events = new ArrayList<>();
                                    for (JSONObject eventJson : eventsJsonList) {

                                        EventsInfo event = new EventsInfo(

                                                eventJson.getString("name"),
                                                eventJson.getString("date"),
                                                eventJson.getString("hour"),
                                                eventJson.getString("qty_max_attendes"),
                                                eventJson.getString("whereisit")
                                        );
                                        events.add(event);
                                        Log.d("Client ", eventJson.toString());
                                    }
                                    // Configurem el RecyclerView y l'adaptador
                                    recyclerView.setLayoutManager(new LinearLayoutManager(EventsList.this));
                                    EventsAdapter adapter = new EventsAdapter(events);
                                    recyclerView.setAdapter(adapter);

                                    //Afegim l'objecte a l'adaptador i notifiquem els canvis.
                                    adapter.setEvents(events);
                                    adapter.notifyDataSetChanged();
                                } else {
                                    Log.e("Error decoding JWT", "JWT es nul");
                                }
                            } catch (JSONException | ParseException e) {
                                e.printStackTrace();
                            }

                        });
                    }

                    @Override
                    public void onLoginError(String error, Context context) {

                        runOnUiThread(() -> Log.e("Eventslist: ",error));
                    }
                });
            } catch (Exception e) {
                throw new RuntimeException(e);
            }
        });
    }

    /*
        Métode que finalitza l'execució d'aquesta activity i torna al menú principal d'events si es prem el botó corresponent.
     */
    public void cls(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), EventsAdmin.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);
    }

}