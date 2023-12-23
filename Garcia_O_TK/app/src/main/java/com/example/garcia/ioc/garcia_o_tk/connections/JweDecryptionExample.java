package com.example.garcia.ioc.garcia_o_tk.connections;

import com.nimbusds.jose.*;
import com.nimbusds.jose.crypto.AESEncrypter;
import com.nimbusds.jose.crypto.DirectDecrypter;
import com.nimbusds.jose.crypto.factories.DefaultJWEDecrypterFactory;
import com.nimbusds.jose.jwk.JWK;
import com.nimbusds.jose.jwk.JWKSelector;
import com.nimbusds.jose.jwk.OctetSequenceKey;
import com.nimbusds.jose.jwk.source.ImmutableSecret;
import com.nimbusds.jose.jwk.source.JWKSource;
import com.nimbusds.jose.proc.JWEDecryptionKeySelector;
import com.nimbusds.jose.proc.SimpleSecurityContext;
import com.nimbusds.jose.util.Base64URL;

import java.util.List;

public class JweDecryptionExample {

    public static void main(String[] args) throws Exception {
        // JSON que contiene el objeto JWE encriptado
        String jweString = "{\"ciphertext\":\"WHU8kbOr45DcrcwFjV4xZVkVhc7gosjwEzbveK_to03792w-Dkn-VfCr493A3p9L8aKZC8QFya3gas_w9DAOhuFjF6oeTHtQhVALRO7fX03uJs2X8fYNSeW_Zh1eJpU2_iuY8_huLd76a1ENr-OGPFEZsxSRI0PSmTBuL-2WI8Hl-MlfhviAnJWuzMJAVxH2jZpAfmYmYC3dKnr4rG7-POHzB8FGlSqOcUDWYpsRCXGsbdWbbx3Zi3sJtoMK7mRuEss4fDKdX-B-lttZxEHkIg\",\"encrypted_key\":\"KH4O6vAu17_pvW2qOTyfDfm8nVHrfSYwjiSPcRgIKocIQ7jiA8ilu8oI9MT1jxD2lTC7hXrS5d3vJMnibj3p4YUu0Gq623-3\",\"iv\":\"eUzcvDi4HEXNUlELBIA9Yg\",\"protected\":\"eyJhbGciOiJBMjU2S1ciLCJlbmMiOiJBMjU2Q0JDLUhTNTEyIn0\",\"tag\":\"CLeGZhbL_fGJHtRmd4grS0IizLhaqMmeQN9hXECu4GU\"}";

        // Clave secreta utilizada para la encriptación
        String secretKey = "PROBANDOprobando";

        // Configurar el desencriptador JWE
        OctetSequenceKey octetSequenceKey = new OctetSequenceKey.Builder(Base64URL.encode(secretKey.getBytes())).build();
        DirectDecrypter directDecrypter = new DirectDecrypter(octetSequenceKey);

        // Desencriptar el objeto JWE
        JWEObject jweObject = JWEObject.parse(jweString);
        jweObject.decrypt(directDecrypter);

        // Obtener el contenido desencriptado
        Payload payload = jweObject.getPayload();
        System.out.println("Contenido desencriptado: " + payload.toString());
    }
}
