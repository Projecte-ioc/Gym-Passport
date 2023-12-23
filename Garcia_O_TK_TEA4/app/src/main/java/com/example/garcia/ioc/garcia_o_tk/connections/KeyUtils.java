package com.example.garcia.ioc.garcia_o_tk.connections;



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

    //private static final String keyBase64 = "__PROBANDO__probando__";

    public String encrypt(String jsonString, Context context) throws Exception {
        String keyBase64 = getKeyFromConfigFile(context);
        // Decodificar la clave base64
        byte[] keyBytes = Base64.getUrlDecoder().decode(keyBase64);

        // Generar un nonce aleatorio
        byte[] nonce = generateRandomNonce();

        // Configurar el cifrado AES-GCM
        Cipher cipher = Cipher.getInstance("AES/GCM/NoPadding");
        SecretKeySpec keySpec = new SecretKeySpec(keyBytes, "AES");
        GCMParameterSpec gcmParameterSpec = new GCMParameterSpec(128, nonce);
        cipher.init(Cipher.ENCRYPT_MODE, keySpec, gcmParameterSpec);

        // Convertir la cadena a bytes y cifrarla
        byte[] jsonBytes = jsonString.getBytes(StandardCharsets.UTF_8);
        byte[] encryptedJsonBytes = cipher.doFinal(jsonBytes);

        // Concatenar el nonce y el texto cifrado
        byte[] encryptedToken = new byte[nonce.length + encryptedJsonBytes.length];
        System.arraycopy(nonce, 0, encryptedToken, 0, nonce.length);
        System.arraycopy(encryptedJsonBytes, 0, encryptedToken, nonce.length, encryptedJsonBytes.length);

        // Codificar el resultado en base64
        return Base64.getUrlEncoder().encodeToString(encryptedToken);
    }
    public static String decrypt(String encryptedTokenBase64, Context context) throws Exception {
        String keyBase64 = getKeyFromConfigFile(context);
        // Decodificar la clave base64
        byte[] keyBytes = Base64.getUrlDecoder().decode(keyBase64);

        // Decodificar el JWE desde base64
        byte[] encryptedToken = Base64.getUrlDecoder().decode(encryptedTokenBase64);

        // Obtener el nonce y el texto cifrado desde el JWE
        byte[] nonce = new byte[12];
        System.arraycopy(encryptedToken, 0, nonce, 0, 12);
        byte[] ciphertext = new byte[encryptedToken.length - 12];
        System.arraycopy(encryptedToken, 12, ciphertext, 0, ciphertext.length);

        // Configurar el descifrador con la clave y el nonce
        Cipher cipher = Cipher.getInstance("AES/GCM/NoPadding");
        SecretKeySpec keySpec = new SecretKeySpec(keyBytes, "AES");
        GCMParameterSpec gcmParameterSpec = new GCMParameterSpec(128, nonce);
        cipher.init(Cipher.DECRYPT_MODE, keySpec, gcmParameterSpec);

        // Descifrar utilizando la clave y el nonce
        byte[] decryptedJsonBytes = cipher.doFinal(ciphertext);
        String decryptedJson = new String(decryptedJsonBytes, StandardCharsets.UTF_8);

        return decryptedJson;
    }

    private byte[] generateRandomNonce() {
        SecureRandom secureRandom = new SecureRandom();
        byte[] nonce = new byte[12];
        secureRandom.nextBytes(nonce);
        return nonce;
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
