package com.example.garcia.ioc.garcia_o_tk.connections;

import com.example.garcia.ioc.garcia_o_tk.admin.GymInfo;
import com.nimbusds.jwt.*;

import java.util.List;

/**
 * author: Claudio Garcia Otero.
 * Classe per descodificar els diferents tipus de TOKENS rebuts.
 */
public class JWTDecoder {

    /**
     * Métode que descodifica el TOKEN i n'obté el rol d'usuari.
     *
     * @param jwtToken TOKEN rebut.
     *
     */
    public static String getToken(String jwtToken) {
        try {
            // SignedJWT es una classe que descodifica els tokens.
            SignedJWT signedJWT = SignedJWT.parse(jwtToken);
            // Obtenim les Claims del TOKEN
            JWTClaimsSet jwtClaimsSet = signedJWT.getJWTClaimsSet();
            // De les CLaims obtingudes, ens quedem nomès amb "rol_user"
            String userRole = jwtClaimsSet.getStringClaim("rol_user");

            return userRole;
        } catch (Exception e) {
            e.printStackTrace();
            return null;
        }

    }
    /**
     * Métode que descodifica el TOKEN i n'obté les dades de perfil del gimnàs.
     *
     * @param jwtToken TOKEN rebut.
     *
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
}

  








        