import base64
from Crypto.Cipher import AES
from Crypto.Util.Padding import unpad
from cryptography.hazmat.primitives.ciphers.aead import AESGCM
import jwt
import psycopg2
from flask import jsonify
from dotenv import load_dotenv
import os


class Connexion:
    load_dotenv()

    def validate_rol_user(self, token: str):
        data = self.get_elements_of_token(token)
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

    def get_elements_filtered(self, filter, table, what_filter, selector, start=None, end=None):
        connex, cursor = self.get_connection_to_db()

        # Construye la parte básica de la consulta SQL
        query_sql = f"SELECT {selector} FROM {table} WHERE {what_filter} = %s"

        # Si start y end están definidos, aplica paginación
        if start is not None and end is not None:
            query_sql += f" LIMIT %s"
            cursor.execute(query_sql, (filter, start))
        else:
            cursor.execute(query_sql, (filter,))

        records = cursor.fetchall()
        connex.close()
        return records

    def get_elements_of_token(self, token):
        payload = jwt.decode(token, os.getenv("SK"), algorithms=['HS256'])
        print(type(payload))
        return payload

    def cipher_content(self, token):
        key_base64 = os.getenv('SK')
        key_bytes = base64.urlsafe_b64decode(key_base64.ljust(32, '='))
        aesgcm = AESGCM(key_bytes)
        plaintext = token
        print(f"plaintext = {plaintext}")
        nonce = os.urandom(12)
        ciphertext = aesgcm.encrypt(nonce, plaintext.encode(), None)
        encrypted_token = base64.urlsafe_b64encode(nonce + ciphertext).decode()
        print(f"encripted_token = {encrypted_token}")
        return encrypted_token

    def decipher_content(self, encrypted_token):
        key_base64 = os.getenv('SK')
        key_bytes = base64.urlsafe_b64decode(key_base64.ljust(32, '='))
        aesgcm = AESGCM(key_bytes)
        encrypted_data = base64.urlsafe_b64decode(encrypted_token)
        nonce = encrypted_data[:12]
        ciphertext = encrypted_data[12:]
        decrypted_token = aesgcm.decrypt(nonce, ciphertext, None).decode()
        print(f"decrypted_token = {decrypted_token}")
        print(type(decrypted_token))
        return decrypted_token
