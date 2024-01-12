package com.example.garcia.ioc.garcia_o_tk.encription;



import java.nio.charset.StandardCharsets;
import java.security.SecureRandom;
import java.util.Base64;
import javax.crypto.Cipher;
import javax.crypto.spec.GCMParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import android.content.Context;
import android.content.res.AssetManager;
import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

/**
 * author: Claudio Garcia Otero.
 * Classe per fer xifrar i desxifrar.
 */
public class KeyUtils {

    /** Métode per xifrar, per facilitar la operativa rep per paràmatre el JSON en format string
     * @param jsonString JSON que volem xifrar.
     * @param context context de l'aplicació.
     * @return String xifrat.
     * @throws Exception
     */


    public String encrypt(String jsonString, Context context) throws Exception {
        //Cridem el métode getKeyFromConfigFile per obtenir la clau.
        String keyBase64 = getKeyFromConfigFile(context);
        //Decodifiquem la clau en base64.
        byte[] keyBytes = Base64.getUrlDecoder().decode(keyBase64);
        //Generem un número aleatori de xifratge que es farà servir un únic cop.
        byte[] nonce = generateRandomNonce();
        //Cofigurem el xifratge AES-GCM
        Cipher cipher = Cipher.getInstance("AES/GCM/NoPadding");
        SecretKeySpec keySpec = new SecretKeySpec(keyBytes, "AES");
        GCMParameterSpec gcmParameterSpec = new GCMParameterSpec(128, nonce);
        cipher.init(Cipher.ENCRYPT_MODE, keySpec, gcmParameterSpec);

        //Passem la cadena obtinguda a bytes i la xifrem
        byte[] jsonBytes = jsonString.getBytes(StandardCharsets.UTF_8);
        byte[] encryptedJsonBytes = cipher.doFinal(jsonBytes);

        //Concatenem el nonce amb el text xifrat
        byte[] encryptedToken = new byte[nonce.length + encryptedJsonBytes.length];
        System.arraycopy(nonce, 0, encryptedToken, 0, nonce.length);
        System.arraycopy(encryptedJsonBytes, 0, encryptedToken, nonce.length, encryptedJsonBytes.length);

        //Codifiquem el resultat en base64
        return Base64.getUrlEncoder().encodeToString(encryptedToken);
    }

    /** Métode que segueix el mateix procediment que el xifrat però a l'inversa. Rebra per paràmetre un String xifrat.
     * @param encryptedTokenBase64 String xifrat en base64 i amb la mateixa clau
     * @param context context de l'aplicació.
     * @return token desxifrat.
     * @throws Exception
     */
    public static String decrypt(String encryptedTokenBase64, Context context) throws Exception {
        //Cridem el métode getKeyFromConfigFile per obtenir la clau.
        String keyBase64 = getKeyFromConfigFile(context);
        //Decodifiquem la clau en base64.
        byte[] keyBytes = Base64.getUrlDecoder().decode(keyBase64);
        // Decodifiquem el TOKEN.
        byte[] encryptedToken = Base64.getUrlDecoder().decode(encryptedTokenBase64);
        // Obtenim el nonce.
        byte[] nonce = new byte[12];
        System.arraycopy(encryptedToken, 0, nonce, 0, 12);
        byte[] ciphertext = new byte[encryptedToken.length - 12];
        System.arraycopy(encryptedToken, 12, ciphertext, 0, ciphertext.length);

        //Cofigurem el xifratge AES-GCM
        Cipher cipher = Cipher.getInstance("AES/GCM/NoPadding");
        SecretKeySpec keySpec = new SecretKeySpec(keyBytes, "AES");
        GCMParameterSpec gcmParameterSpec = new GCMParameterSpec(128, nonce);
        cipher.init(Cipher.DECRYPT_MODE, keySpec, gcmParameterSpec);

        //Desxifrem fent servir la clau i el nonce.
        byte[] decryptedJsonBytes = cipher.doFinal(ciphertext);

        return new String(decryptedJsonBytes, StandardCharsets.UTF_8);
    }

    /*
      Métode per generar un número d'us únic (nonce) aleatori.
     */
    private byte[] generateRandomNonce() {
        SecureRandom secureRandom = new SecureRandom();
        byte[] nonce = new byte[12];
        secureRandom.nextBytes(nonce);
        return nonce;
    }
    /*
        Métode per obtenir de l'entorn la clau, d'aquesta manera la clau no es exposada al codi font.
     */
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
