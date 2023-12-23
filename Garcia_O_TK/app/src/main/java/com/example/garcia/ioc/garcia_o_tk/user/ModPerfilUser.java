package com.example.garcia.ioc.garcia_o_tk.user;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.Toast;

import androidx.appcompat.app.AppCompatActivity;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class ModPerfilUser extends AppCompatActivity {

    private EditText name, pass;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_mod_perfil_user);

        Intent intent = getIntent();
        String uName = intent.getStringExtra("Name");
        String authToken = intent.getStringExtra("TOKEN");

        name = findViewById(R.id.editTextNewNamePerfilUser);
        pass = findViewById(R.id.editTextNewPass);

        String nameS = name.getText().toString();
        String pasS = pass.getText().toString();
        String rol = "normal";

        JSONObject jsonObject = new JSONObject();
        try {
            jsonObject.put("name", nameS);
            jsonObject.put("pswd_app", pasS);
            jsonObject.put("rol_user", rol);
            jsonObject.put("user_name", uName);
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        Button btnMd = findViewById(R.id.button_send);
        btnMd.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if (!nameS.isEmpty() && !pasS.isEmpty()) {
                    ExecutorService executor = Executors.newSingleThreadExecutor();
                    executor.execute(() -> {

                        ApiRequests.updateClient(ModPerfilUser.this, authToken, jsonObject, new ApiRequests.ApiListener() {
                            @Override
                            public void onLoginSuccess(String token, Context context) {
                                Toast.makeText(ModPerfilUser.this, "Dades modificades correctament.", Toast.LENGTH_LONG).show();
                                finish();
                            }

                            @Override
                            public void onLoginError(String error, Context context) {
                                Toast.makeText(ModPerfilUser.this, "No s'han pogut modificar les dades.", Toast.LENGTH_LONG).show();

                            }
                        });
                    });

                }
            }
        });
    }
}