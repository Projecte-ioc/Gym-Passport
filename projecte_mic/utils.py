import jwt
import psycopg2
from flask import jsonify


class Connexion:
    USER = 'isard'
    PASSWORD = 'pirineus'
    HOST = '127.0.0.1'
    PORT = 5432
    DATABASE = 'gympassportdb'
    SK = 'PROBANDOprobando'

    def get_connection_values(self):
        db_params = {
            'dbname': self.DATABASE,
            'user': self.USER,
            'password': self.PASSWORD,
            'host': self.HOST,
            'port': self.PORT
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
        print(type(token))
        payload = jwt.decode(token, self.SK, algorithms=['HS256'])
        return jsonify(payload)

    def format_records(self, records, column_names):
        formatted_records = []
        for record in records:
            formatted_record = {column_names[i]: record[i] for i in range(len(column_names))}
            formatted_records.append(formatted_record)
        return formatted_records