package com.example.garcia.ioc.garcia_o_tk.user;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;
import com.example.garcia.ioc.garcia_o_tk.encription.JWTDecoder;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

/**
 * author: Claudio Garcia Otero.
 * Classe per mostrar les dades de l'usuari.
 */
public class PerfilUser extends AppCompatActivity {

    private EditText name, uName, gymName;

    private String userNameS;

    @Override
    protected void onCreate(Bundle savedInstanceState) {

        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_perfil_user);
        //Obtenim eL TOKEN de l'activitat principal.
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");

        name = findViewById(R.id.editTextNewNamePerfilUser);
        uName = findViewById(R.id.editTextNewPass);
        gymName = findViewById(R.id.editTextGymNamePerfilUser);
        //Nou fil per fer la sol·licitud a l'API.
        ExecutorService executor = Executors.newSingleThreadExecutor();
        executor.execute(() -> {
            try {
                ApiRequests.getProfileInfo(PerfilUser.this, authToken, new ApiRequests.ApiListener() {
                    @Override
                    public void onLoginSuccess(String token, Context context) {
                        //Asignem a userinfo el token decodificat.
                        UserInfo userInfo = JWTDecoder.decodeUserInfo(token);
                        //Obtenim les dades de l'objecte userInfo.
                        userNameS = userInfo.getUserName();
                        String gymNameS = userInfo.getGymName();
                        String nameS = userInfo.getName();
                        //Ubiquem les dades als editText corresponent.
                        name.setText(nameS);
                        uName.setText(userNameS);
                        gymName.setText(gymNameS);
                        //Botó per passar a l'activity de modificació de dades, passem al intent "user_name" doant que no es pot modificar.
                        Button btnMod = findViewById(R.id.button_mod_perfil_user);
                        btnMod.setOnClickListener(view -> {
                            finish();
                            Intent intent1 = new Intent(getApplicationContext(), ModPerfilUser.class);
                            intent1.putExtra("Name", userNameS);
                            intent1.putExtra("TOKEN",authToken);
                            startActivity(intent1);


                        });
                    }

                    @Override
                    public void onLoginError(String error, Context context) {

                        Log.e("Error de perfil.", error);

                    }
                });
            } catch (Exception e) {
                throw new RuntimeException(e);
            }
        });
    }

    /*
        Métode que finalitza l'execució d'aquesta activity i torna al menú principal si es prem el botó corresponent.
     */
    public void close(View view) {
        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");
        finish();
        Intent intent2 = new Intent(getApplicationContext(), UserAct.class);
        intent2.putExtra("TOKEN", authToken);
        startActivity(intent2);


    }
}