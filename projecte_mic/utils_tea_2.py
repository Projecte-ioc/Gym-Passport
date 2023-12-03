import base64
import hashlib
import json

from jwcrypto import jwk, jwe
from jwcrypto.common import json_encode
import jwt
import psycopg2
from flask import jsonify
from dotenv import load_dotenv
import os


class Connexion:
    load_dotenv()

    def validate_rol_user(self, token: str):
        data = self.get_elements_of_token(token).get_json(force=True)
        print(data)
        rol_user = data.get('rol_user')
        gym_name = data.get('gym_name')
        user_name = data.get('user_name')
        id = self.get_elements_filtered(gym_name.replace(' ', '-'), "gym", "name", "id")

        return rol_user, id[0][0], user_name, gym_name

    def get_connection_values(self):
        db_params = {
            'dbname': os.getenv("DATABASE"),
            'user': os.getenv("USER"),
            'password': os.getenv("PASSWORD"),
            'host': os.getenv("HOST"),
            'port': os.getenv("PORT")
        }
        return db_params

    def get_connection_to_db(self):
        connex = psycopg2.connect(**self.get_connection_values())
        cursor = connex.cursor()
        return connex, cursor

    def get_elements_filtered(self, filter, table, what_filter, selector):
        connex, cursor = self.get_connection_to_db()
        query_sql = f"SELECT {selector} FROM {table} WHERE {what_filter} = %s"

        cursor.execute(query_sql, (filter,))
        records = cursor.fetchall()
        connex.close()
        return records

    def get_elements_of_token(self, token):
        payload = jwt.decode(token, os.getenv("SK"), algorithms=['HS256'])
        return jsonify(payload)

    def convert_password_base64(self):
        salt = os.urandom(16)
        key_hash = hashlib.pbkdf2_hmac('sha256', os.getenv('SK').encode('utf-8'), salt,
                                       100000)

        base64_key = base64.urlsafe_b64encode(key_hash).decode('utf-8')
        clave_dict = {"k": base64_key, "kty": "oct"}

        pswd = jwk.JWK(k=clave_dict.get("k"), kty=clave_dict.get("kty"))
        return pswd

    def cipher_content(self, token, SK):
        ready_token = jwe.JWE(token, json_encode({"alg": "A256KW", "enc": "A256CBC-HS512"}))
        ready_token.add_recipient(SK)
        return ready_token.serialize()

    def descipher_content(self, content, SK):
        print(content)
        jwe_token = jwe.JWE()
        json_text = json.dumps(content)
        print(json_text)
        jwe_token.deserialize(json_text)
        jwe_token.decrypt(SK.export())
        payload = jwe_token.payload
        return payload
