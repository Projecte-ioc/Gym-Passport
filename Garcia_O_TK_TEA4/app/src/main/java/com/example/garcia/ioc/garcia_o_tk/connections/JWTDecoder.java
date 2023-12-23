package com.example.garcia.ioc.garcia_o_tk.connections;

import android.content.Context;
import android.content.res.AssetManager;
import android.util.Log;

import com.example.garcia.ioc.garcia_o_tk.admin.ClientsInfo;
import com.example.garcia.ioc.garcia_o_tk.admin.GymInfo;
import com.example.garcia.ioc.garcia_o_tk.user.UserInfo;
import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.nimbusds.jwt.JWTClaimsSet;
import com.nimbusds.jwt.SignedJWT;
import org.json.JSONObject;
import java.io.IOException;
import java.io.InputStream;
import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Properties;

import javax.crypto.SecretKey;

import io.jsonwebtoken.Claims;
import io.jsonwebtoken.Jwts;
import io.jsonwebtoken.security.Keys;

/**
 * author: Claudio Garcia Otero.
 * Classe per codificar i descodificar els TOKENS.
 */
public class JWTDecoder {


    private  Context context;
    //Variable per la paraula secreta.
    private static String SECRET_KEY_STRING ;

    private static SecretKey SECRET_KEY = null;

    /**
     * Métode que converteix un objecte JSON en un JWT.
     * @param jsonObject objecte que volem modificar.
     * @return un JWT amb les seves 3 parts.
     */
    public static String toJWT(JSONObject jsonObject, Context context) throws IOException {
        SECRET_KEY_STRING = getKeyFromConfigFile(context);

        SECRET_KEY = padSecretKey(SECRET_KEY_STRING);
        // Convertim l'objecte JSON en una cadena
        String jsonPayload = jsonObject.toString();
        // Creem un JWT signat amb la araula secreta.
        return Jwts.builder()
                .setPayload(jsonPayload)
                .signWith(SECRET_KEY)
                .compact();
    }

    public static Claims decodeJWT(String jwtToken) {
        try {
            return Jwts.parser().setSigningKey(SECRET_KEY).parseClaimsJws(jwtToken).getBody();
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    /**
     * La signatura del TOKEN exigeix un llarg determinat, amb aquest mètode aconseguim que la clau tingui el llarg óptim.
     * @param key, paraula clau amb que es signa el TOKEN, es la mateixa del server.
     * @return clau amb el llarg ptim que exigeixen els JWT per ser signats.
     */
    private static SecretKey padSecretKey(String key) {
        byte[] keyBytes = key.getBytes();
        byte[] paddedKey = Arrays.copyOf(keyBytes, 32); // 32 bytes = 256 bits
        return Keys.hmacShaKeyFor(paddedKey);
    }

    /**
     * Mètode per obtenir el rol d'usuari del TOKEN tornat pel servidor.
     * @param token TOKEN rebut.
     * @return userRole, string amb el rol d'usuari.
     */
    public static String getRoleFromToken(String token) {
        try {
            SignedJWT signedJWT = SignedJWT.parse(token);
            JWTClaimsSet jwtClaimsSet = signedJWT.getJWTClaimsSet();
            // Extraiem les Claims.
            String userRole = jwtClaimsSet.getStringClaim("rol_user");
            return  userRole ;
        } catch (Exception e) {
            e.printStackTrace();
            Log.e("tkerr", "Token: " + token);
            return null;
        }
    }



    /**
     * Mètode que descodifica el TOKEN i n'obté les dades de perfil del gimnàs.
     *
     * @param jwtToken TOKEN rebut.
     */
    public static GymInfo decodeUserGymInfo(String jwtToken) {
        try {
            SignedJWT signedJWT = SignedJWT.parse(jwtToken);

            JWTClaimsSet jwtClaimsSet = signedJWT.getJWTClaimsSet();

            // En aquest cas extraiem totes les Claims.
            String userName = jwtClaimsSet.getStringClaim("user_name");
            String userRole = jwtClaimsSet.getStringClaim("rol_user");
            String gymName = jwtClaimsSet.getStringClaim("gym_name");
            String gymAddress = jwtClaimsSet.getStringClaim("address");
            String gymPhoneNumber = jwtClaimsSet.getStringClaim("phone_number");
            List<String> gymSchedule = jwtClaimsSet.getStringListClaim("schedule");

            return new GymInfo(userName, userRole, gymName, gymAddress, gymPhoneNumber, gymSchedule);
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    /**
     * Mètode que descodifica el TOKEN i n'obté les dades de perfil d'usuari.
     *
     * @param jwtToken TOKEN rebut.
     */

    public static UserInfo decodeUserInfo(String jwtToken) {
        try {
            SignedJWT signedJWT = SignedJWT.parse(jwtToken);
            JWTClaimsSet jwtClaimsSet = signedJWT.getJWTClaimsSet();
            // Extraiem les Claims.
            String userName = jwtClaimsSet.getStringClaim("user_name");
            String userRole = jwtClaimsSet.getStringClaim("rol_user");
            String gymName = jwtClaimsSet.getStringClaim("gym_name");
            String name = jwtClaimsSet.getStringClaim("name");


            return new UserInfo(userName, userRole, gymName, name);
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }
    }

    public static List<ClientsInfo> decodeClientsInfoList(String jweToken) {
        try {
            // Decodifica el JWE y obtén el contenido JSON

            // Convierte la cadena JSON a una lista de objetos ClientsInfo
            Type listType = new TypeToken<List<ClientsInfo>>() {}.getType();
            return new Gson().fromJson(jweToken, listType);
        } catch (Exception e) {
            e.printStackTrace();
            return new ArrayList<>(); // O maneja la excepción de alguna manera adecuada
        }
    }

    public static String getKeyFromConfigFile(Context context) throws IOException {
        Properties properties = new Properties();
        AssetManager assetManager = context.getAssets();

        try (InputStream input = assetManager.open("Config.Properties")) {
            properties.load(input);
        } catch (IOException e) {
            e.printStackTrace();
            throw e;
        }

        return properties.getProperty("encryption_key");
    }

}

  








        