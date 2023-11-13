package com.example.garcia.ioc.garcia_o_tk.admin;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.widget.TextView;

import androidx.appcompat.app.AppCompatActivity;

import com.example.garcia.ioc.garcia_o_tk.R;
import com.example.garcia.ioc.garcia_o_tk.connections.ApiRequests;
import com.example.garcia.ioc.garcia_o_tk.connections.JWTDecoder;

import java.util.List;
import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;

public class PerfilAdmin extends AppCompatActivity {

    private TextView name, dir, num, time;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_perfil_admin);

        Intent intent = getIntent();
        String authToken = intent.getStringExtra("TOKEN");

        name = findViewById(R.id.text_header);
        dir = findViewById(R.id.editTextAddress);
        num = findViewById(R.id.editTextPhone);
        time = findViewById(R.id.editTextTime);

        ExecutorService executor = Executors.newSingleThreadExecutor();

        executor.execute(new Runnable() {
            @Override
            public void run() {

                if (authToken != null) {
                    // Llamar al método para obtener la información del perfil
                    ApiRequests.getProfileInfo(PerfilAdmin.this, authToken, new ApiRequests.ApiListener() {
                        @Override
                        public void onLoginSuccess(String token) {
                            // Manejar el éxito al obtener la información del perfil
                            Log.d("ProfileInfo", "Token del perfil: " + token);
                            // Realizar acciones con el token del perfil
                            GymInfo gymInfo = JWTDecoder.decodeUserGymInfo(token);

                            if (gymInfo != null) {
                                // Acceder a los datos decodificados
                                String userName = gymInfo.getUserName();
                                String userRole = gymInfo.getUserRole();
                                String gymName = gymInfo.getGymName();
                                String gymAddress = gymInfo.getGymAddress();
                                String gymPhoneNumber = gymInfo.getGymPhoneNumber();
                                List gymSchedule = gymInfo.getGymScheduleString();


                                // Realizar acciones con la información obtenida
                                // Por ejemplo, imprimir los datos en el registro
                                Log.i("ProfileInfo", "Nombre de usuario: " + userName);
                                Log.i("ProfileInfo", "Rol de usuario: " + userRole);
                                Log.i("ProfileInfo", "Nombre del gimnasio: " + gymName);
                                Log.i("ProfileInfo", "Dirección del gimnasio: " + gymAddress);
                                Log.i("ProfileInfo", "Teléfono del gimnasio: " + gymPhoneNumber);
                                Log.i("ProfileInfo", "Horario: " + gymSchedule);

                                name.setText(gymName.toUpperCase());
                                dir.setText(gymAddress);
                                num.setText(gymPhoneNumber);
                                time.setText(gymSchedule.toString());


                                // También puedes utilizar estos datos para inicializar vistas, etc.
                            } else {
                                // Manejar el caso en que no se pudo decodificar la información del token
                                Log.e("ProfileInfo", "Error al decodificar la información del token");
                            }
                        }

                        @Override
                        public void onLoginError(String error) {
                            // Manejar errores al obtener la información del perfil
                            Log.e("ProfileInfoError", "Error al obtener información del perfil: " + error);
                        }
                    });
                } else {
                    // Manejar el caso cuando no se recibe un token válido
                    Log.e("ProfileInfo", "No se recibió un token válido");
                }
            }
        });
    }
}