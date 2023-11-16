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

    def format_records(self, records, column_names):
        formatted_records = []
        for record in records:
            formatted_record = {column_names[i]: record[i] for i in range(len(column_names))}
            formatted_records.append(formatted_record)
        return formatted_records